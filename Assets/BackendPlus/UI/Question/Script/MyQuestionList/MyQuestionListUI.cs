// Copyright 2013-2023 AFI, Inc. All Rights Reserved.

using System;
using System.Collections.Generic;
using LitJson;
using TMPro;
using UnityEngine;

namespace BackendPlus.Module.Question {
    public class MyQuestionListUI : MonoBehaviour {
        [SerializeField] private TMP_Text listSmallAlertText = null;
        [SerializeField] private RectTransform questionScrollViewRectTransform = null;
        [SerializeField] private RectTransform questionListParentObject = null;
        [SerializeField] private RectTransform startYEmptyObject = null;
        [SerializeField] private GameObject questionItemObject = null;
        [SerializeField] private QuestionDetailUI questionDetailUI = null;

        private List<QuestionItem> _questionItemList = new List<QuestionItem>();

        private bool _isDataLoading = false;
        
        private float _itemStartY = 110;
        private float _itemHeight = 230;
        private const float _itemOffsetX = 100;
        private const float _maxFirstKeyLoadingDistance = 10;

        private string _readyFlagString;
        private string _doneFlagString;

        // =====================================================================================
        //  Unity Default Function
        // =====================================================================================
        void Update() {
            CheckFirstKeyCall();
        }
        
        // =====================================================================================
        //  Public Function
        // =====================================================================================
        public void Initialize(JsonData textJson) {
            questionDetailUI.Initialize();

            _itemStartY = Mathf.Abs(startYEmptyObject.rect.height) + Mathf.Abs(startYEmptyObject.anchoredPosition.y) + 20;
            _itemHeight = questionItemObject.GetComponent<RectTransform>().rect.height + 20; // 20은 각 여유공간

            listSmallAlertText.text = textJson["extraAlert"].ToString();
            _readyFlagString = textJson["answerStatusReady"].ToString();
            _doneFlagString = textJson["answerStatusDone"].ToString();
        }

        public void Reset() {
            questionDetailUI.Reset();
        }

        public void OpenUI() {
            gameObject.SetActive(true);
        }
        public void CloseUI() {
            gameObject.SetActive(false);
        }

        // 내 문의 내역 불러오기(최초 1회)
        public void GetMyQuestionList() {
            BackendPlus.Question.UI.ActiveLoadingUI(true);
            _isDataLoading = true;
            
            BackendPlus.Question.Data.GetQuestionList(callback => { ResetQuestionList("GetMyQuestionList", callback); });
        }
        
        // 페이징으로 내 문의 내역 불러오기
        public void GetFirstKeyQuestionList() {
            BackendPlus.Question.UI.ActiveLoadingUI(true);
            _isDataLoading = true;

            BackendPlus.Question.Data.GetQuestionListByFirstKey(callback => { ResetQuestionList("GetFirstKeyQuestionList", callback); });
        }
        
        // =====================================================================================
        //  Private Function
        // =====================================================================================
        
        // 스크롤이 최하단까지 내려갔는지 체크하는 함수
        private void CheckFirstKeyCall() {

            // 데이터가 불러오는 중이면
            if (_isDataLoading) {
                return;
            }

            // 더 이상 불러올 페이지가 존재하지 않으면 중지
            if (BackendPlus.Question.Data.isFirstKeyFinish) {
                return;
            }

            // 데이터가 하나도 없다면 중지
            if (BackendPlus.Question.Data.questionItemList.Count <= 0) {
                return;
            }
            
            float maxContentHeight = questionListParentObject.rect.height - questionScrollViewRectTransform.rect.height;

            // UI의 크기가 변경되는 것은 약간의 프레임이 지난 후에 반영이 된다.
            // 따라서 크기가 변경된 프레임과 동일한 프레임에서 크기를 참조하면 안된다.(크기 변경 이전값으로 참조됨)
            // 스크롤바를 내릴 경우 questionListParentObject.anchoredPosition.y의 좌표가 내려간다. maxContentHeight만큼 크기를 받았기 때문에 가까울 수록 끝에 가깝다.
            if (Math.Abs(questionListParentObject.anchoredPosition.y - maxContentHeight) < _maxFirstKeyLoadingDistance) {
                GetFirstKeyQuestionList();
            }

        }

        // QuestionData 정보를 이용하여 데이터 불러오기
        private void ResetQuestionList(string actionName, QuestionResult callback) {
            try { 
                BackendPlus.Question.UI.ActiveLoadingUI(false);

                if (callback.isSuccess == false) {
                    BackendPlus.Question.UI.AddAlertUIError(callback.errorMessage);

                    // 뒤끝 에러이면서 통신 에러일경우
                    if (callback.IsBackendError() && 
                        (callback.backendErrorInfo.IsServerError() || callback.backendErrorInfo.IsClientRequestFailError())) { {
                            BackendPlus.Question.UI.OpenAlertUI(
                                string.Format(BackendPlus.Question.UI.questionErrorText.normalError, actionName) + "\n" + 
                                BackendPlus.Question.UI.questionErrorText.tryError);
                        }
                    } else {
                        BackendPlus.Question.UI.OpenAlertUI(string.Format(BackendPlus.Question.UI.questionErrorText.normalError, actionName));
                    }
                    
                    questionListParentObject.anchoredPosition = new Vector2(questionListParentObject.anchoredPosition.x, 0);

                    return;
                }

                int visibleItemCount = BackendPlus.Question.Data.questionItemList.Count;

                // 데이터의 갯수만큼 UI 아이템을 추가
                while (_questionItemList.Count < visibleItemCount) {
                    var item = Instantiate(questionItemObject, questionListParentObject.transform, true);
                    item.transform.localScale = new Vector3(1, 1, 1);

                    float topY = _itemStartY + _itemHeight * _questionItemList.Count;

                    RectTransform itemRect = item.GetComponent<RectTransform>();
                    itemRect.offsetMax = new Vector2(-_itemOffsetX, -topY);
                    itemRect.offsetMin = new Vector2(_itemOffsetX, -topY - _itemHeight);

                    _questionItemList.Add(item.GetComponent<QuestionItem>());
                }


                // 생성이 완료된 수만큼 UI 크기를 확장
                float totalHeight = _itemHeight * visibleItemCount + _itemStartY;
                
                // 재배치전 
                float originalY = questionListParentObject.anchoredPosition.y;
                
                questionListParentObject.offsetMax = new Vector2(0, 0);
                questionListParentObject.offsetMin = new Vector2(0, -totalHeight);
                
                // UI가 재배치되며 원래 위치하던 좌표가 변경될 수 있다.
                questionListParentObject.anchoredPosition = new Vector2(questionListParentObject.anchoredPosition.x, originalY);

                // 미리 만들어진 UI에 생성된 item들에게 차례대로 데이터를 부여
                // 리스트를 불러왔을 때, 새롭게 추가된 문의가 있을 수도 있고, 답변이 달린 문의가 있을 수 있으므로, 완전히 교체한다.
                for (int i = 0; i < visibleItemCount; i++) {
                    var index = i;
                    _questionItemList[index].gameObject.SetActive(true);
                    _questionItemList[index].Initialize(BackendPlus.Question.Data.questionItemList[index], questionDetailUI,_readyFlagString,_doneFlagString);
                }
                
                // 그 이후로 안보이는 것들은 제거
                for (int i = visibleItemCount; i < _questionItemList.Count; i++) {
                    var index = i;
                    _questionItemList[index].gameObject.SetActive(false);
                }
                
                
            } catch (Exception e) {
                BackendPlus.Question.UI.AddAlertUIError(e.ToString());
                BackendPlus.Question.UI.OpenAlertUI(string.Format(BackendPlus.Question.UI.questionErrorText.normalError, actionName));
            } finally {
                _isDataLoading = false;
            }
        }
    }
}