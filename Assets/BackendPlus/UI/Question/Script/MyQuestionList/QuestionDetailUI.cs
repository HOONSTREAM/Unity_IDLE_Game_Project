// Copyright 2013-2023 AFI, Inc. All Rights Reserved.

using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BackendPlus.Module.Question {
    public class QuestionDetailUI : MonoBehaviour {

        [SerializeField] private Button closeButton = null;
        [SerializeField] private RectTransform mainRectTransform = null;

        
        [SerializeField] private TMP_Text titleText = null;
        [SerializeField] private TMP_Text contentText = null;

        [SerializeField] private TMP_Text dateText = null;
        [SerializeField] private TMP_Text isAnswerText = null;
        
        [SerializeField] private TMP_Text answerText = null;

        [SerializeField] private RectTransform answerParentRect = null;
        [SerializeField] private RectTransform answerLineRect = null;

        private float _originalContentHeight = 0;
        private float _originalAnswerHeight = 0;
        
        // =====================================================================================
        //  Public Function
        // =====================================================================================
        public void Initialize() {
            ReSize();
            
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(CloseUI);
        }

        public void Reset() {
            titleText.text = string.Empty;
            contentText.text = string.Empty;

            dateText.text = string.Empty;
            
            isAnswerText.text = string.Empty;
            answerText.text = string.Empty;

            answerParentRect.offsetMax = new Vector2(0, 0);
            answerParentRect.offsetMin = new Vector2(0, 0);
        }
        
        public void OpenUI(QuestionItemData questionItemData) {
            titleText.text = questionItemData.title;
            contentText.text = questionItemData.content;

            dateText.text = DateTime.Parse(questionItemData.inDate).ToString("yyyy-MM-dd:H:mm:ss");;
            
            isAnswerText.text = questionItemData.flag;
            answerText.text = questionItemData.answer;
            gameObject.SetActive(true);
        }

        public void CloseUI() {
            gameObject.SetActive(false);   
        }
        
        // ContentSizeFitter는 활성화 이후 몇 프레임동안 사이즈를 재조정한다.
        // 해당 프레임동안 바뀐 부분이 있다면 같이 재조정한다.
        void Update() {
            if (_originalContentHeight != contentText.rectTransform.sizeDelta.y) {
                ReSizeContent();
                _originalContentHeight = contentText.rectTransform.sizeDelta.y;
            }

            if (_originalAnswerHeight != answerText.rectTransform.sizeDelta.y) {
                ReSizeContent();
                _originalAnswerHeight = answerText.rectTransform.sizeDelta.y;
            }
        }
        
        // =====================================================================================
        //  Private Function
        // =====================================================================================
        private void ReSize() {
            Vector2 screenSize = BackendPlus.Question.UI.ScreenSize;
            if (screenSize.x > screenSize.y) {
                float widthMargin = screenSize.x * 0.1f;
                float heightMargin = screenSize.y * 0.05f;
                
                mainRectTransform.offsetMin = new Vector2(widthMargin,heightMargin);
                mainRectTransform.offsetMax = new Vector2(-widthMargin,-heightMargin);
            } else {
                float widthMargin = screenSize.x * 0.05f;
                float heightMargin = screenSize.y * 0.1f;

                mainRectTransform.offsetMin = new Vector2(widthMargin,heightMargin);
                mainRectTransform.offsetMax = new Vector2(-widthMargin,-heightMargin);
            }
        }
        
        private void ReSizeContent() {
            float totalHeight = 50;
            
            totalHeight += contentText.rectTransform.sizeDelta.y;
            
            totalHeight += 40; // 답변 구분선 위쪽 여백
            answerLineRect.anchoredPosition = new Vector2(0, -totalHeight);
            totalHeight += 40; // 답변 구분선 아래쪽 여백

            answerText.rectTransform.anchoredPosition = new Vector2(0, -totalHeight);
            
            totalHeight += answerText.rectTransform.sizeDelta.y;
            totalHeight += 50; // 답변 여유 공간

            answerParentRect.sizeDelta = new Vector2(0, totalHeight);
        }
    }
}