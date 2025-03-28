// Copyright 2013-2023 AFI, Inc. All Rights Reserved.

using System;
using System.Collections.Generic;
using BackendPlus.UI.Question;
using LitJson;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace BackendPlus.Module.Question {
    public class QuestionUI : MonoBehaviour {
        [SerializeField] private TMP_Text titleText = null;
        
        [SerializeField] private Button closeButton = null;
        
        [SerializeField] private QuestionUIChangeButton registerButton = null;
        [SerializeField] private QuestionUIChangeButton myQuestionListButton = null;

        [SerializeField] private RegisterQuestionUI registerQuestionUI = null;
        [SerializeField] private MyQuestionListUI myQuestionListUI = null;

        [SerializeField] private LoadingUI loadingUI = null;
        [SerializeField] private QuestionAlertUI questionAlertUI = null;

        private Queue<Action> _queueAction = new Queue<Action>();

        public Vector2 ScreenSize { get; private set; } = Vector2.zero;

        public QuestionErrorText questionErrorText;
        
        // =====================================================================================
        //  Unity Default Function
        // =====================================================================================
        void Update() {
            if (_queueAction.Count > 0) {
                _queueAction.Dequeue().Invoke();
            }
        }
        
        // =====================================================================================
        //  Public Function
        // =====================================================================================
        public void Initialize(JsonData textJson) {
            ScreenSize = GetComponent<RectTransform>().sizeDelta;
            // 우측상단 X 버튼 초기화
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(CloseUI);

            questionErrorText = new QuestionErrorText(textJson["errorText"]);
            
            titleText.text = textJson["uiTitle"].ToString();
            
            // 문의 알림
            questionAlertUI.Initialize(textJson);
            questionAlertUI.CloseUI();
            
            // 각 UI 초기화
            registerQuestionUI.Initialize(textJson["registerUI"]);
            myQuestionListUI.Initialize(textJson["getListUI"]);

            // 문의하기 카테고리 버튼 초기화
            registerButton.Initialize(textJson["selectRegisterButton"].ToString());
            registerButton.SetClickAction(() => {
                registerQuestionUI.OpenUI();
                myQuestionListUI.CloseUI();
                myQuestionListButton.ResetLine();
                
                registerQuestionUI.GetDefaultForm();
            });

            // 문의내역 카테고리 버튼 초기화
            myQuestionListButton.Initialize(textJson["selectGetMyListButton"].ToString());
            myQuestionListButton.SetClickAction(() => {
                myQuestionListUI.OpenUI();
                registerQuestionUI.CloseUI();
                registerButton.ResetLine();
                
                myQuestionListUI.GetMyQuestionList();
            });
        }
        
        public void Reset() {
            questionAlertUI.Reset();
            registerQuestionUI.Reset();
            myQuestionListUI.Reset();
            loadingUI.Reset();
            registerButton.ExecuteButton();
            
            _queueAction.Clear();
        }

        public void OpenUI() {
            Reset();
            gameObject.SetActive(true);
        }

        public void CloseUI() {
            gameObject.SetActive(false);
        }

        public void EnqueueFunction(Action action) {
            _queueAction.Enqueue(action);
        }
        
        
        public void ActiveLoadingUI(bool isActive) {
            BackendPlus.Question.UI.EnqueueFunction(() => {
                if (isActive == true) {
                    loadingUI.OpenUI();
                } else {
                    loadingUI.CloseUI();
                }
            });
        }
        
        public void OpenAlertUI(string alertText) {
            questionAlertUI.OpenUI(alertText);
        }

        public void AddAlertConfirmButtonAction ( UnityAction action) {
            questionAlertUI.AddAction(action);
        }
        
        public void AddAlertUIError(string errorMessage) {
            questionAlertUI.AddError(errorMessage);
        }
    }
}