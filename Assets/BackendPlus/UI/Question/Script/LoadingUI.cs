// Copyright 2013-2023 AFI, Inc. All Rights Reserved.

using System;
using BackEnd;
using LitJson;
using UnityEngine;

namespace BackendPlus.Module.Question {
    public class LoadingUI : MonoBehaviour {

        [SerializeField] private GameObject loadingObject = null;
        
        private float _imageSpeed = 30.0f; // 이미지 움직임  속도
        private const float _maxY = 30, _minY = 0; // 움직이는 위치

        private bool _isOpen = false; // 열려있는지 체크 여부

        private float _time = 0; // 현재시간
        private const float _timeOut = 15; // 타임아웃 시간

        // =====================================================================================
        //  Unity Default Function
        // =====================================================================================
        void Update() {
            
            // OpenUI 외에는 작동 못하게 중지
            if (_isOpen == false) {
                return;
            }

            // 활성화 동안 시간 체크
            _time += Time.deltaTime;

            // 타임아웃 시간보다 지났다면 창 강제종료하도록 활성화
            if (_time > _timeOut) {
                BackendPlus.Question.UI.AddAlertConfirmButtonAction(BackendPlus.Question.CloseUI);
                BackendPlus.Question.UI.OpenAlertUI(BackendPlus.Question.UI.questionErrorText.loadingTimeOutError);
                CloseUI();
                return;
            }
            
            // 로딩 움직임
            loadingObject.transform.localPosition += loadingObject.transform.up * (_imageSpeed * Time.deltaTime);

            // y가 최소값 미만이거나, 최댓값 이상일 경우 반대값 제공
            if (loadingObject.transform.localPosition.y < _minY || loadingObject.transform.localPosition.y > _maxY) {
                _imageSpeed *= -1;
            }
        }

        // =====================================================================================
        //  Public Function
        // =====================================================================================
        public void Reset() {
            // 로딩 아이콘 좌표 초기화
            var pos = loadingObject.transform.localPosition;
            loadingObject.transform.localPosition = new Vector3(pos.x, _minY, pos.y);
        }
        
        public void OpenUI() {
            _isOpen = true;
            _time  = 0;
            gameObject.SetActive(true);
        }

        public void CloseUI() {
            _isOpen = false;
            gameObject.SetActive(false);
        }
    }
}
