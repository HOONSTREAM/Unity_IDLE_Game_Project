// Copyright 2013-2023 AFI, Inc. All Rights Reserved.

using System;
using BackendPlus.Module.Question;
using LitJson;
using UnityEngine;

namespace BackendPlus {
    public static class Question {
        private static QuestionUI     _questionUI; // UI 스크립트
        private static QuestionData   _questionData; // UI에서 생성되는 데이터 스크립트

        private static GameObject _questionUIObject; // Resource에서 불러온 UI 오브젝트
        private static JsonData _questionUITextJson; // Resource에서 불러온 UI 내 텍스트
        private static string _languageJsonName = "Korean";
        
        private static bool _isOpenFailed = false; // UI 초기화 시 UI 활성화 못하도록 제어
        public static QuestionData Data => _questionData;
        public static QuestionUI UI => _questionUI;

        /// <summary>
        /// UI의 고정 텍스트에 적용할 언어 json 설정 함수
        /// </summary>
        /// <param name="jsonName">BackendUI/Question에 위치한 json 파일의 이름</param>
        public static void SetLanguageJsonName(string jsonName) {
            _languageJsonName = jsonName;
            try {
                var textAsset = Resources.Load<TextAsset>($"BackendUI/Question/{jsonName}");
                _questionUITextJson = JsonMapper.ToObject(textAsset.text);

                _languageJsonName = jsonName;

            } catch (Exception e) {
                throw new Exception($"{jsonName} LanguageFile Not Found\n\n{e}");
            }
            
            DestroyUI();
        }
        
        /// <summary>
        /// 문의내역 불러오기 페이징 기능 활성화 함수
        /// </summary>
        /// <param name="firstKeyActive">문의내역 10개 불러오기 이후 스크롤을 내렸을 때 자동으로 이후의 문의내역을 불러올것인지 여부</param>
        public static void SetFirstKeyActive(bool firstKeyActive) {
            _questionData.SetFirstKeyActive(firstKeyActive);
        }
        
        /// <summary>
        /// 문의내역 불러오기 재호출 딜레이 시간 조정(기본 300초)
        /// </summary>
        /// <param name="delaySeconds">문의 내역 불러오기를 초기화할 주기(초 단위)</param>
        public static void SetQuestionListReloadDelaySeconds(int delaySeconds) {
            _questionData.SetQuestionListReloadDelaySeconds(delaySeconds);
        }

        /// <summary>
        /// 일대일문의 UI 활성화 함수
        /// </summary>
        public static void OpenUI() {
            if (_isOpenFailed) {
                Debug.LogError("1대1문의창을 열 수 없습니다.");
            }

            if (_questionData == null) {
                _questionData = new QuestionData();
            }

            try {
                if (_questionUITextJson == null) {
                    var textAsset = Resources.Load<TextAsset>($"BackendUI/Question/{_languageJsonName}");
                    _questionUITextJson = JsonMapper.ToObject(textAsset.text);
                }
            } catch (Exception e) {
                Debug.LogError("QuestionUI 텍스트 생성 중 에러가 발생하였습니다. : " + e);
                _isOpenFailed = true;
                return;
            }

            try {
                if (_questionUI == null) {
                    if (_questionUIObject == null) {
                        _questionUIObject = Resources.Load<GameObject>("BackendUI/Question/QuestionUI");
                    }
                    // 오브젝트 생성
                    var insObj = GameObject.Instantiate(_questionUIObject);
                    _questionUI = insObj.GetComponent<QuestionUI>();

                    
                    try {
                        _questionUI.Initialize(_questionUITextJson);
                    } catch (Exception e) {
                        Debug.LogError("QuestionUI 초기화 중 에러가 발생하였습니다. : " + e);
                        _isOpenFailed = true;
                        GameObject.Destroy(_questionUI.gameObject);
                    }
                }

                _questionUI.OpenUI();
            } catch (Exception e) {
                Debug.LogError("QuestionUI 생성 중 에러가 발생하였습니다. : " + e);
            }
        }

        /// <summary>
        /// 일대일문의 UI 비활성화 함수
        /// </summary>
        public static void CloseUI() {
            if (_questionUI == null) {
                return;
            }

            _questionUI.CloseUI();
        }

        /// <summary>
        /// 해당 Scene에 존재하는 일대일문의 관련 메모리 해제 함수
        /// </summary>
        public static void DestroyUI() {
            if (_questionUI != null) {
                GameObject.Destroy(_questionUI.gameObject);
                _questionUI = null;
            }
        }

        /// <summary>
        /// 모든 일대일문의 관련 메모리 해제 함수
        /// </summary>
        public static void DestroyAll() {
            _questionData = null;
            _questionUIObject = null;
            _questionUITextJson = null;
            DestroyUI();
        }
    }
}