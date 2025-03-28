// Copyright 2013-2023 AFI, Inc. All Rights Reserved.

using System;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace BackendPlus.Module.Question {
    
    public class QuestionItemData {
        public string title { get; }
        public string content{ get; }
        public string inDate{ get; }
        public string flag{ get; private set; }

        public string answer { get; } = string.Empty;
        
        public QuestionItemData(JsonData json) {
            title = json["title"]?.ToString();
            content = json["content"].ToString();

            inDate = json["inDate"]?.ToString();

            if (json.ContainsKey("answer")) {
                answer = json["answer"]?.ToString();
            }
        }

        private QuestionItemData(string title, string content) {
            this.title = title;
            this.content = content;

            this.inDate = DateTime.Now.ToString();
        }

        public static QuestionItemData CreateLocalQuestionData(string title, string content) {
            return new QuestionItemData(title, content);
        }

        public void SetFlag(string flag) {
            this.flag = flag;
        }
    }
    public class QuestionItem : MonoBehaviour {

        [SerializeField] private TMP_Text titleText  = null;
        
        [SerializeField] private TMP_Text dateText = null;
        [SerializeField] private TMP_Text isAnswerText = null;
        [SerializeField] private Button button = null;

        public QuestionItemData questionItemData { get; private set; }
        
        private MyQuestionListUI _myQuestionListUI;
        
        public void Initialize(QuestionItemData itemData, QuestionDetailUI questionDetailUI, string readyFlag, string doneFlag) {
            try {
                questionItemData = itemData;
                titleText.text = questionItemData.title;
                dateText.text = DateTime.Parse(questionItemData.inDate).ToString("yyyy-MM-dd:H:mm:ss");
                itemData.SetFlag(string.IsNullOrEmpty(questionItemData.answer) ? readyFlag : doneFlag);
                isAnswerText.text = itemData.flag;
                
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => {
                    questionDetailUI.OpenUI(questionItemData);
                });
                
            } catch (Exception e) {
                Debug.LogError(e);
            }
        }
    }
}