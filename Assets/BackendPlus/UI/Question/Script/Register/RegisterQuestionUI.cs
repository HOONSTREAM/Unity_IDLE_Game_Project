// Copyright 2013-2023 AFI, Inc. All Rights Reserved.

using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BackEnd;
using LitJson;

namespace BackendPlus.Module.Question {
    public class RegisterQuestionUI : MonoBehaviour {
        // ====================================================
        //  member
        // ====================================================

        [Header("외부 UI")]
        [SerializeField] private QuestionTypeSelectUI questionTypeSelectUI = null;

        [Header("스크롤뷰")] 
        [SerializeField] private RectTransform registerUIScroll = null;

        [Header("문의 유형")] 
        [SerializeField] private TMP_Text questionTypeSelectTitleText = null;
        [SerializeField] private Button questionTypeSelectButton = null;

        [Header("제목")] 
        [SerializeField] private TMP_Text questionTitleText = null;
        [SerializeField] private TMP_InputField questionTitleInputField = null;

        [Header("내용")] 
        [SerializeField] private TMP_Text questionContentText = null;
        [SerializeField] private TMP_InputField questionContentInputField = null;

        [Header("문의 등록")] 
        [SerializeField] private Button registerQuestionButton = null;
        [SerializeField] private TMP_Text registerStatusText = null;

        // 선택해주세요
        private TMP_Text _questionTypeSelectText = null;

        
        // 문의 유형
        private QuestionType _questionType;

        // 문의 유형 선택 여부
        private bool _isQuestionTypeSelect = false;

        private string _originalTypeSelectString = "선택해주세요";
        private string registerErrorByForm = "문의 양식을 가져올 수 없습니다!";
        private string registerErrorByType = "문의 유형을 선택해주세요.";
        private string registerErrorByTitle = "제목을 입력해주세요.";
        private string registerErrorByContent =  "내용을 입력해주세요.";
        private string registerErrorByRegister = "문의 등록에 실패했습니다";
        private string registerSuccessAlert = "문의 등록이 완료되었습니다. 확인을 누리면 창이 닫힙니다.";

        // =====================================================================================
        //  Public Function
        // =====================================================================================

        public void OpenUI() {
            gameObject.SetActive(true);
        }

        public void CloseUI() {
            gameObject.SetActive(false);
        }

        public void Initialize(JsonData textJson) {
            //문의 유형
            questionTypeSelectTitleText.text = textJson["typeTitle"].ToString();
            _originalTypeSelectString = textJson["typeDefault"].ToString();
            questionTitleText.text = textJson["titleTitle"].ToString();
            questionContentText.text = textJson["contentTitle"].ToString();
            registerQuestionButton.GetComponentInChildren<TMP_Text>().text = textJson["registerButton"].ToString();
            registerErrorByForm = textJson["registerErrorByForm"].ToString();
            registerErrorByType     = textJson["registerErrorByType"].ToString();
            registerErrorByTitle    = textJson["registerErrorByTitle"].ToString();
            registerErrorByContent  = textJson["registerErrorByContent"].ToString();
            registerStatusText.text = textJson["registerStatusText"].ToString();
            registerSuccessAlert        = textJson["registerSuccessAlert"].ToString();

            // 문의유형 선택창 초기화
            questionTypeSelectUI.Initialize(textJson);
            questionTypeSelectUI.SetRegisterQuestionUI(this);

            // 문의 유형 선택 버튼 할당
            questionTypeSelectButton.onClick.RemoveAllListeners();
            questionTypeSelectButton.onClick.AddListener(questionTypeSelectUI.OpenUI);

            // 문의 등록중 문구 할당
            _questionTypeSelectText = questionTypeSelectButton.GetComponentInChildren<TMP_Text>();

            // 문의 등록 버튼 할당
            registerQuestionButton.onClick.RemoveAllListeners();
            registerQuestionButton.onClick.AddListener(RegisterQuestion);
        }

        // 문의 작성한 내용 초기화하는 함수
        public void Reset() {
            // 문의 유형, 제목, 내용 초기화
            _questionTypeSelectText.text = _originalTypeSelectString;
            questionTitleInputField.text = string.Empty;
            questionContentInputField.text = string.Empty;

            // 문의 등록중이라는 문구 비활성화
            registerStatusText.gameObject.SetActive(false);

            questionTypeSelectUI.CloseUI();
            registerQuestionButton.interactable = true;

            registerUIScroll.localPosition = new Vector3(0, 0, 0);

            _isQuestionTypeSelect = false;
        }

        public void GetDefaultForm() {
            if (string.IsNullOrEmpty(BackendPlus.Question.Data.defaultForm) == false) {
                questionContentInputField.text = BackendPlus.Question.Data.defaultForm;
                return;
            }

            BackendPlus.Question.UI.ActiveLoadingUI(true);
            BackendPlus.Question.Data.GetDefaultForm(questionResult => {
                BackendPlus.Question.UI.ActiveLoadingUI(false);

                if (questionResult.isSuccess) {
                    questionContentInputField.text = BackendPlus.Question.Data.defaultForm;
                } else {
                    BackendPlus.Question.UI.OpenAlertUI(registerErrorByForm);
                    BackendPlus.Question.UI.AddAlertUIError(questionResult.errorMessage);
                }
            });
        }

        // questionTypeSelectUI에서 호출하는 함수
        public void ChangeQuestionType(QuestionType questionType, string questionTypeString) {
            _questionType = questionType;
            _questionTypeSelectText.text = questionTypeString;
            _isQuestionTypeSelect = true;
        }

        // =====================================================================================
        //  Private Function
        // =====================================================================================

        // 뒤끝 서버로 문의 내용 전송하는 함수
        private void RegisterQuestion() {
            string title = questionTitleInputField.text;
            string content = questionContentInputField.text;

            if (_isQuestionTypeSelect == false) {
                BackendPlus.Question.UI.OpenAlertUI(registerErrorByType);
                return;
            }

            if (string.IsNullOrEmpty(title)) {
                BackendPlus.Question.UI.OpenAlertUI(registerErrorByTitle);
                return;
            }

            if (string.IsNullOrEmpty(content)) {
                BackendPlus.Question.UI.OpenAlertUI(registerErrorByContent);
                return;
            }

            registerStatusText.gameObject.SetActive(true);

            // 버튼 클릭 비활성화(동일한 호출 반복 위험)
            registerQuestionButton.interactable = false;

            BackendPlus.Question.UI.ActiveLoadingUI(true);

            BackendPlus.Question.Data.SendQuestion(_questionType, title, content, result => {
                BackendPlus.Question.UI.ActiveLoadingUI(false);

                if (result.isSuccess == false) {
                    BackendPlus.Question.UI.OpenAlertUI(registerErrorByRegister);
                    BackendPlus.Question.UI.AddAlertUIError(result.errorMessage);
                } else {
                    BackendPlus.Question.UI.OpenAlertUI(registerSuccessAlert);
                    BackendPlus.Question.UI.AddAlertConfirmButtonAction(GetComponentInParent<QuestionUI>().CloseUI);
                }

                registerStatusText.gameObject.SetActive(false);
                registerQuestionButton.interactable = true;
            });
        }
    }
}