// Copyright 2013-2023 AFI, Inc. All Rights Reserved.

using LitJson;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BackendPlus.Module.Question {
    public class QuestionUIChangeButton : MonoBehaviour {
        private RectTransform _innerImageRectTransform;
        private Button _button;

        private UnityAction _clickAction;
        
        // =====================================================================================
        //  Public Function
        // =====================================================================================
        
        public void Initialize(string buttonName) {
            var images = GetComponentsInChildren<Image>();
            _innerImageRectTransform = images[images.Length-1].GetComponent<RectTransform>();
            _button = GetComponent<Button>();
            GetComponentInChildren<TMP_Text>().text = buttonName;
        }
        
        public void SetClickAction(UnityAction action) {
            _clickAction = action;
            _button.onClick.RemoveAllListeners();
            
            _button.onClick.AddListener( () => {
                _clickAction.Invoke();
                
                // 버튼의 검은색 bottom 없애기
                _innerImageRectTransform.offsetMin = new Vector2(_innerImageRectTransform.offsetMin.x, 0);
            });
        }

        // 버튼을 수동으로 실행하는 함수
        public void ExecuteButton() {
            _button.onClick.Invoke();
        }

        // 버튼의 검은색 bottom 라인 초기화
        public void ResetLine() {
            _innerImageRectTransform.offsetMin = new Vector2(_innerImageRectTransform.offsetMin.x, 4);
        }
    }
}