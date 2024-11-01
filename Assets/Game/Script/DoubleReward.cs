using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DoubleReward : MonoBehaviour {
    // Start is called before the first frame update
    //双倍奖励按钮
    public GameObject m_DoubleReward;

    public GameObject m_DoubleRewardRoot;

    //是否已经领取双倍奖励
    bool mIsGetDoubleReward;

    //领取时间
    public DateTime mGetDoubleRewardTime;

    //双倍奖励持续15分钟
    public const int DoubleRewardTime = 30 * 60;

    //双倍奖励背景
    public GameObject m_DoubleRewardBg;

    //双倍奖励倒计时根节点
    public GameObject m_DoubleRewardCountDownRoot;

    //倒计时文本
    public Text m_DoubleRewardCountDownText;
    /////////////////////////////////////////////多语言设置，文本物体
    //双倍奖励标题

    void Start() {
        if (GlobalManager.Instance.CurrentLevel < 18) {
            m_DoubleReward.SetActive(false);
            return;
        }

        int show = PlayerPrefs.GetInt("doublereward", 0);
        if (show == 0) {
            PlayerPrefs.SetInt("doublereward", 1);
            PlayerPrefs.Save();
            m_DoubleRewardRoot.SetActive(true);
        }

        if (PlayerPrefs.HasKey("GetDoubleRewardTime") == false && PlayerPrefs.HasKey("IsGetDoubleReward") == false) {
            mIsGetDoubleReward = false;
            PlayerPrefs.SetInt("IsGetDoubleReward", 0);
            mGetDoubleRewardTime = DateTime.MinValue;
            PlayerPrefs.SetString("GetDoubleRewardTime", mGetDoubleRewardTime.ToString());
        }
        else {
            mGetDoubleRewardTime = DateTime.Parse(PlayerPrefs.GetString("GetDoubleRewardTime"));
            if (DateTime.Now.DayOfYear != mGetDoubleRewardTime.DayOfYear) //如果当前时间和领取时间不一致
            {
                mIsGetDoubleReward = false;
                PlayerPrefs.SetInt("IsGetDoubleReward", 0);
            }
            else {
                mIsGetDoubleReward = PlayerPrefs.GetInt("IsGetDoubleReward") == 1;
            }
        }

        //如果已经领取双倍奖励，并且当前时间和领取时间相差小于15分钟
        if (mIsGetDoubleReward && (DateTime.Now - mGetDoubleRewardTime).TotalSeconds < DoubleRewardTime) {
            GlobalManager.Instance.IsDoubleReward = true;
            m_DoubleRewardBg.SetActive(true);
            m_DoubleRewardCountDownRoot.SetActive(true);
        }
        else {
            m_DoubleRewardBg.SetActive(false);
            m_DoubleRewardCountDownRoot.SetActive(false);
            mIsGetDoubleReward = false;
        }

        m_DoubleReward.SetActive(!mIsGetDoubleReward);
        SetLanguage();
    }

    public void SetLanguage() {
    }

    // Update is called once per frame
    void Update() {
        if (GlobalManager.Instance.IsDoubleReward == false)
            return;
        if (mIsGetDoubleReward && (DateTime.Now - mGetDoubleRewardTime).TotalSeconds < DoubleRewardTime) {
            //倒计时，显示分钟和秒
            int remainingTime = DoubleRewardTime - (int)(DateTime.Now - mGetDoubleRewardTime).TotalSeconds;
            int minute = remainingTime / 60;
            int second = remainingTime % 60;
            m_DoubleRewardCountDownText.text = minute.ToString("00") + ":" + second.ToString("00");
        }
        else {
            m_DoubleRewardCountDownRoot.SetActive(false);
            m_DoubleRewardBg.SetActive(false);
            GlobalManager.Instance.IsDoubleReward = false;
        }
    }

    public void OnDoubleRewardBtn() {
        m_DoubleRewardRoot.SetActive(!m_DoubleRewardRoot.activeSelf);
        StartCoroutine(LogHelper.LogToServer("ClickModule", new Dictionary<string, object>() {
            { "ModuleId", "C6" }
        }));
    }

    //领取双倍奖励按钮
    public void OnGetDoubleRewardBtn() {
        // ApplovinSDKManager.Instance().rewardAdsManager.ShowRewardedAd("C6", () => {
        mIsGetDoubleReward = true;
        PlayerPrefs.SetInt("IsGetDoubleReward", 1);
        mGetDoubleRewardTime = DateTime.Now;
        PlayerPrefs.SetString("GetDoubleRewardTime", mGetDoubleRewardTime.ToString());
        m_DoubleReward.SetActive(false);
        m_DoubleRewardRoot.SetActive(false);
        m_DoubleRewardBg.SetActive(true);
        m_DoubleRewardCountDownRoot.SetActive(true);
        GlobalManager.Instance.IsDoubleReward = true;
        // }, () => {
        //     TipsManager.Instance.ShowTips(GlobalManager.Instance.GetLanguageValue("AdNotReady"));
        // });
    }
}
