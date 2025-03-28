// Copyright 2013-2023 AFI, Inc. All Rights Reserved.

using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using BackEnd;
using LitJson;

namespace BackendPlus.Module.Question {
    public class QuestionTypeSelectUI : MonoBehaviour {
        [Header("문의유형 외부 버튼(테두리밖)")] 
        [SerializeField] private Button outerButton = null; // 터치 시 

        [Header("문의유형 내부 버튼")] 
        [SerializeField] private GameObject questionTypeButton = null;

        [Header("문의유형 내부 버튼 생성 위치")] 
        [SerializeField] private Transform questionTypeUIParent = null;

        [SerializeField] private RectTransform scrollViewRectTransform = null;
        
        private RegisterQuestionUI _registerQuestionUI = null;

        // =====================================================================================
        //  Public Function
        // =====================================================================================

        public void Initialize(JsonData textJson) {
            ReSize();
            
            outerButton.onClick.RemoveAllListeners();
            outerButton.onClick.AddListener( () => { gameObject.SetActive(false); });

            float totalHeight = 0;
            
            foreach (QuestionType questionType in Enum.GetValues(typeof(QuestionType))) {

                // 계정, 버그, 신고 등 문의유형 제거 하고 싶을 경우 해당 if문을 이용해주세요
                // // ======================= if문 추가 ======================
                // if (questionType == QuestionType.Account ||
                //     questionType == QuestionType.Bug || 
                //     questionType == QuestionType.Event) {
                //     continue;
                // }
                // // ======================= 추가 완료 ======================
                
                
                var button = Instantiate(questionTypeButton, questionTypeUIParent, true);
                button.transform.localScale = new Vector3(1, 1, 1);
                string questionTypeByText = textJson["typeSelect"][questionType.ToString()].ToString();
                button.GetComponentInChildren<TMP_Text>().text = questionTypeByText;

                button.GetComponent<Button>().onClick.AddListener( () => {
                    _registerQuestionUI.ChangeQuestionType(questionType, questionTypeByText);
                    gameObject.SetActive(false);
                });

                totalHeight += button.GetComponent<RectTransform>().sizeDelta.y;
            }
            
            if (totalHeight < scrollViewRectTransform.rect.height) {
                float marginWidth = scrollViewRectTransform.offsetMin.x;
                float marginHeight = (gameObject.GetComponent<RectTransform>().rect.height - totalHeight) / 2;
                
                
                scrollViewRectTransform.offsetMin = new Vector2(marginWidth,marginHeight);
                scrollViewRectTransform.offsetMax = new Vector2(-marginWidth,-marginHeight);
            }
        }
        
        // 문의 등록 오브젝트 내부 변수에 할당하는 함수
        public void SetRegisterQuestionUI(RegisterQuestionUI registerQuestionUI) {
            _registerQuestionUI = registerQuestionUI;
        }

        public void OpenUI() {
            gameObject.SetActive(true);
        }

        public void CloseUI() {
            gameObject.SetActive(false);
        }

        // =====================================================================================
        //  Private Function
        // =====================================================================================
        private void ReSize() {
            Vector2 screenSize = BackendPlus.Question.UI.ScreenSize;
            if (screenSize.x > screenSize.y) {
                float widthMargin = screenSize.x * 0.1f;
                float heightMargin = screenSize.y * 0.05f;
                
                scrollViewRectTransform.offsetMin = new Vector2(widthMargin,heightMargin);
                scrollViewRectTransform.offsetMax = new Vector2(-widthMargin,-heightMargin);
            } else {
                float widthMargin = screenSize.x * 0.05f;
                float heightMargin = screenSize.y * 0.1f;

                scrollViewRectTransform.offsetMin = new Vector2(widthMargin,heightMargin);
                scrollViewRectTransform.offsetMax = new Vector2(-widthMargin,-heightMargin);
            }
        }
    }
}