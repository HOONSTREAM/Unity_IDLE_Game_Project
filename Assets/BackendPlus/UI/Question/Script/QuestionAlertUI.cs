// Copyright 2013-2023 AFI, Inc. All Rights Reserved.

using System.Collections;
using LitJson;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BackendPlus.Module.Question {
    public class QuestionAlertUI : MonoBehaviour {
        
        [SerializeField] private RectTransform totalBoardRectTransform = null;

        [SerializeField] private Button confirmButton = null;
        [SerializeField] private TMP_Text alertText = null;

        [SerializeField] private Button errorButton = null;
        [SerializeField] private GameObject errorPanel = null;
        [SerializeField] private Button errorExitButton = null;
        [SerializeField] private TMP_Text errorText = null;
        [SerializeField] private RectTransform  viewPortRectTransform= null;

        // =====================================================================================
        //  Public Function
        // =====================================================================================
        public void Initialize(JsonData textJson) {
            ReSize();
            
            errorButton.onClick.RemoveAllListeners();
            errorButton.onClick.AddListener(() => {
                errorPanel.SetActive(true);
                StartCoroutine(SetErrorTextBoxSize());
            });
            
            errorExitButton.onClick.RemoveAllListeners();
            errorExitButton.onClick.AddListener(() => errorPanel.SetActive(false));

            confirmButton.GetComponentInChildren<TMP_Text>().text = textJson["confirmButton"].ToString();
        }

        public void Reset() {
            // 문의 등록 시, 모든 창을 닫는 Action이 추가되므로, Reset 시에는 버튼 클릭하면 Alert 창만 닫혀야 합니다.
            confirmButton.onClick.RemoveAllListeners();
            confirmButton.onClick.AddListener(CloseUI);
            
            alertText.text = string.Empty;
            
            errorButton.gameObject.SetActive(false);
            errorPanel.SetActive(false);
            errorText.text = string.Empty;
        }
        
        public void OpenUI(string text) {
            gameObject.SetActive(true);
            alertText.text = text;
        }
        
        public void CloseUI() {
            gameObject.SetActive(false);
            Reset();
        }
        
        public void AddAction(UnityAction action) {
            confirmButton.onClick.AddListener(action);
        }
        
        public void AddError(string text) {
            errorButton.gameObject.SetActive(true);
            errorText.text += text;
        }

        
        // =====================================================================================
        //  Private Function
        // =====================================================================================
        private IEnumerator SetErrorTextBoxSize() {
            float defaultHeight = errorText.rectTransform.sizeDelta.y;

            var waitFrame = new WaitForFixedUpdate();

            int count = 0;
            while (true) {
                if (errorText.rectTransform.sizeDelta.y != defaultHeight) {
                    viewPortRectTransform.sizeDelta = new Vector2(0, errorText.rectTransform.sizeDelta.y);
                    break;
                }

                count++;
                if (count > 10000) {
                    break;
                }
                yield return waitFrame;
            }
        }


        
        private void ReSize() {
            Vector2 screenSize = BackendPlus.Question.UI.ScreenSize;
            if (screenSize.x > screenSize.y) {
                float widthMargin = screenSize.x * 0.1f;
                float heightMargin = screenSize.y * 0.1f;
                
                totalBoardRectTransform.offsetMin = new Vector2(widthMargin,heightMargin);
                totalBoardRectTransform.offsetMax = new Vector2(-widthMargin,-heightMargin);
            } else {
                float widthMargin = screenSize.x * 0.05f;
                float heightMargin = screenSize.y * 0.3f;

                totalBoardRectTransform.offsetMin = new Vector2(widthMargin,heightMargin);
                totalBoardRectTransform.offsetMax = new Vector2(-widthMargin,-heightMargin);
            }
        }
    }
}