// // Copyright 2013-2022 AFI, Inc. All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Reflection;
using BackEnd;

namespace BackendPlus.Module.Question {
    public class QuestionData {
        public string defaultForm = string.Empty;
        
        public List<QuestionItemData> questionItemList = new List<QuestionItemData>(); // 내 문의 리스트
        
        private string _questionDataFirstKey = string.Empty; // 페이징 키
        private bool _firstKeyActive = true;
        public bool isFirstKeyFinish = true; // firstKey가 null일 경우(public 용)

        private int myQuestionListReloadSeconds = 300; // 내 문의 리스트 불러오기 대기시간(초)

        
        // 각 호출에 대한 응답값을 저장하는 Dictionary. 키값은 함수 이름
        private Dictionary<string, QuestionResult> dataResultDictionary = new Dictionary<string, QuestionResult>();
        
        
        public void SetFirstKeyActive(bool firstKeyActive) {
            _firstKeyActive = firstKeyActive;
        }

        public void SetQuestionListReloadDelaySeconds(int delaySeconds) {
            myQuestionListReloadSeconds = delaySeconds;
        }
        
        // =====================================================================================
        //  Public Function
        // =====================================================================================
        // 결과를 Dictionary에 저장
        public void AddResultData(string functionName, QuestionResult questionResult) {
            if (dataResultDictionary.ContainsKey(functionName)) {
                dataResultDictionary[functionName] = questionResult;
            } else {
                dataResultDictionary.Add(functionName, questionResult);
            }
        }
        
        // 문의양식 불러오기
        public void GetDefaultForm(Action<QuestionResult> afterLoad) {
            string functionName = MethodBase.GetCurrentMethod()?.Name;
            
            // 한번이라도 실행한 적이 있다면
            if (dataResultDictionary.ContainsKey(functionName)) {
                    afterLoad.Invoke(dataResultDictionary[functionName]);
                    return;
                
            }

            Backend.Question.GetDefaultQuestionForm( bro => {
                try {
                    var callback = bro;

                    if (callback.IsSuccess()) {
                        defaultForm = callback.GetReturnValuetoJSON()["form"].ToString();
                    } 
                    
                    AddResultData(functionName, new QuestionResult(callback));

                } catch (Exception e) {
                    AddResultData(functionName, new QuestionResult(e));
                } finally {
                    BackendPlus.Question.UI.EnqueueFunction(() => afterLoad.Invoke(dataResultDictionary[functionName]));
                }
            });
        }

        // 요청을 보내는 함수는 데이터를 저장해둘 필요가 없기 때문에 캐싱 없이 성공/실패를 보내준다.
        public void SendQuestion(QuestionType questionType, string title, string content, Action<QuestionResult> afterSend) {
            Backend.Question.SendQuestion(questionType, title, content, bro => {
                try {
                    var callback = bro;

                    if (callback.IsSuccess() && questionItemList.Count > 0) {
                        // 성공일 경우에는 로컬 데이터 삽입
                        // 실제 데이터는 문의내역 재호출 주기가 올때 보임
                        questionItemList.Insert(0, QuestionItemData.CreateLocalQuestionData(title,content));
                    }

                    BackendPlus.Question.UI.EnqueueFunction(() => afterSend.Invoke(new QuestionResult(callback)));

                } catch (Exception e) {
                    BackendPlus.Question.UI.EnqueueFunction(() => afterSend.Invoke(new QuestionResult(e)));
                }
            });
        }
        
        public void GetQuestionList(Action<QuestionResult> afterLoad) {
            
            string functionName = MethodBase.GetCurrentMethod()?.Name;

            // 재호출할 시간이 아직 되지 않았다면 서버에서 호출하지 말고 로컬 캐싱된 값으로 대체.
            if (dataResultDictionary.ContainsKey(functionName)) {
                if (dataResultDictionary[functionName].IsReloadTime(myQuestionListReloadSeconds) == false) {
                    afterLoad.Invoke(dataResultDictionary[functionName]);
                    return;
                }
            }
            
            Backend.Question.GetQuestionList(bro => {
                try {
                    var callback = bro;
                    if (callback.IsSuccess()) {

                        questionItemList.Clear();
                        
                        for (int index = 0 ; index < callback.Rows().Count; index++) {
                            int i = index;
                            QuestionItemData questionItemData = new QuestionItemData(callback.Rows()[i]);
                            questionItemList.Add(questionItemData);
                        }
                        
                        _questionDataFirstKey = callback.FirstKeystring();

                        
                        if (_firstKeyActive) {
                            // 페이징키 활성화시에만 이후 페이징값이 있는지 확인
                            isFirstKeyFinish = string.IsNullOrEmpty(_questionDataFirstKey);
                        } else {
                            // 페이징키 비활성화시에는 페이징이 없다고 고정
                            isFirstKeyFinish = true;
                        }
                    }
                    
                    AddResultData(functionName, new QuestionResult(callback));

                } catch (Exception e) {
                    AddResultData(functionName, new QuestionResult(e));
                } finally {
                    BackendPlus.Question.UI.EnqueueFunction(() => afterLoad.Invoke(dataResultDictionary[functionName]));
                }
            });
        }

        public void GetQuestionListByFirstKey(Action<QuestionResult> afterLoad) {
            
            if (string.IsNullOrEmpty(_questionDataFirstKey)) {
                // 보통은 발생하지 않음.
                afterLoad.Invoke(new QuestionResult(new Exception(BackendPlus.Question.UI.questionErrorText.firstKeyError)));
                return;
            }
            
            string functionName = MethodBase.GetCurrentMethod()?.Name;

            Backend.Question.GetQuestionList(_questionDataFirstKey, bro => {
                try {
                    var callback = bro;
                    if (callback.IsSuccess()) {
                        int max = callback.Rows().Count;
                        for (int index = 0 ; index < max; index++) {
                            int i = index;
                            QuestionItemData questionItemData = new QuestionItemData(callback.Rows()[i]);
                            questionItemList.Add(questionItemData);
                        }
                        
                        // 불러왔는데 firstKey가 동일할 경우, 에러로 판단
                        if (_questionDataFirstKey == callback.FirstKeystring()) {
                            throw new Exception(BackendPlus.Question.UI.questionErrorText.firstKeyError);
                        }
                        
                        _questionDataFirstKey = callback.FirstKeystring();
                        isFirstKeyFinish = string.IsNullOrEmpty(_questionDataFirstKey);

                    } else {
                        isFirstKeyFinish = true;
                        _questionDataFirstKey = string.Empty;
                    }
                    
                    AddResultData(functionName, new QuestionResult(callback));

                } catch (Exception e) {
                    isFirstKeyFinish = true;
                    _questionDataFirstKey = string.Empty;
                    
                   AddResultData(functionName, new QuestionResult(e));
                } finally {
                    BackendPlus.Question.UI.EnqueueFunction(() => afterLoad.Invoke(dataResultDictionary[functionName]));
                }
            });
        }
    }
}

public class QuestionResult {
    public bool isSuccess { get; }

    public string errorMessage { get; }
    
    public DateTime updateTime { get; }

    public BackendReturnObject backendErrorInfo { get; }

    public QuestionResult(BackendReturnObject bro) {
        updateTime = DateTime.Now;
        
        backendErrorInfo = bro;
        isSuccess = backendErrorInfo.IsSuccess();
             
        if (isSuccess) {
            errorMessage = string.Empty;
        } else {
            errorMessage = backendErrorInfo.ToString();
        }
    }

    public QuestionResult(Exception e) {
        this.isSuccess = false;
        this.errorMessage = e.ToString();
        updateTime = DateTime.Now;
        backendErrorInfo = null;
    }

    public bool IsReloadTime(int afterSeconds) {
        return updateTime.AddSeconds(afterSeconds) < DateTime.Now;
    }

    public bool IsBackendError() {
        return backendErrorInfo != null;
    }
}