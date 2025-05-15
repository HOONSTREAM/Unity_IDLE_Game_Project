using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class ADS_Manager
{
    private bool TestMode = false; //TODO
   
    public readonly string REWARD_ANDROID_ID = "ca-app-pub-7827703662920334/5684289752";
    public readonly string REWARD_TEST_ID = "ca-app-pub-3940256099942544/5224354917";

    
    RewardedAd _rewardad; // 보상형광고

    AdRequest _adRequest;
    Action _rewardedCallback;

    public void Init()
    {
        MobileAds.Initialize(initStatus => { });
        PrePareADS();
    }

    private void PrePareADS()
    {      
        string reward;

        if(TestMode)
        {           
            reward = REWARD_TEST_ID;
        }

        else
        {                      
            reward = REWARD_ANDROID_ID;
        }

        _adRequest = new AdRequest();
        _adRequest.Keywords.Add("unit-admob-sample");

        RewardedAd.Load(reward, _adRequest, OnAdRewardCallBack);
       
    }

    private void OnAdRewardCallBack(RewardedAd ad, LoadAdError error)
    {
        if(error != null)
        {
            Debug.LogError("보상형 광고 준비에 실패하였습니다 : " + error);

            return;
        }

        Debug.Log("보상형 광고 준비에 성공하였습니다 : " + ad.GetResponseInfo());
        _rewardad = ad;
        Register_EventHandler(_rewardad);
    }


    private void Register_EventHandler(RewardedAd ad)
    {
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("보상형 광고가 닫혔습니다.");
            PrePareADS();
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("보상형 광고 시청에 실패하였습니다 : " + error);
            PrePareADS();
        };


        ad.OnAdPaid += (AdValue adValue) =>
        {
            if(_rewardedCallback != null)
            {
                _rewardedCallback?.Invoke();
                _rewardedCallback = null;
            }
            
        };
    }

    public void ShowRewardedAds(Action rewardCallback)
    {     
        _rewardedCallback = rewardCallback;

        if(Data_Manager.Main_Players_Data.isBuyADPackage == true)
        {            
            Base_Canvas.instance.Get_TOP_Popup().Initialize("광고 제거 대상입니다.");
            _rewardedCallback?.Invoke();
            _rewardedCallback = null;
            return;
        }


        if(_rewardad != null && _rewardad.CanShowAd())
        {           
            _rewardad.Show((Reward reward) =>
            {
                Debug.Log(String.Format(reward.Type + " : ", reward.Amount));

                
                if (_rewardedCallback != null)
                {
                    _rewardedCallback?.Invoke();
                    _rewardedCallback = null;
                }
            });
        }

        else
        {           
            PrePareADS();
        }
    }

    
}
