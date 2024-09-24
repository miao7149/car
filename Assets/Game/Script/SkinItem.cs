using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinItem : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject m_Bg;
    public GameObject m_Icon;
    public GameObject m_Lock;
    //解锁条件描述
    public GameObject m_UnLockDesc;
    //解锁按钮
    public GameObject m_UnLockBtn;
    //使用中按钮
    public GameObject m_InUseBtn;
    //使用按钮
    public GameObject m_UseBtn;
    //背景图片列表
    public List<Sprite> m_BgList;
    DecorationType type;
    int index;
    SkinItemData skinItemData;
    UISwitchSkin mUISwitchSkin;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Init(SkinItemData skinItemData, DecorationType type, int index, UISwitchSkin uISwitchSkin)
    {
        m_UseBtn.SetActive(false);
        m_InUseBtn.SetActive(false);
        m_Lock.SetActive(false);
        m_UnLockBtn.SetActive(false);
        m_UnLockDesc.SetActive(false);
        m_Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/Skin/" + skinItemData.SpriteName);
        this.type = type;
        this.index = index;
        this.skinItemData = skinItemData;
        mUISwitchSkin = uISwitchSkin;
        var str = PlayerPrefs.GetString(type.ToString() + index.ToString(), "0");
        if (index == 0)
        {
            str = "1";
        }
        else
        {
            str = PlayerPrefs.GetString(type.ToString() + index.ToString(), "0");
        }

        if (str == "1")//已解锁
        {
            if (type == DecorationType.Car)
            {
                if (GlobalManager.Instance.PlayerCarSkinName == skinItemData.ModelName)//使用中
                {
                    m_InUseBtn.SetActive(true);
                    m_Bg.GetComponent<Image>().sprite = m_BgList[0];
                }
                else
                {
                    m_UseBtn.SetActive(true);
                    m_Bg.GetComponent<Image>().sprite = m_BgList[1];
                }
            }
            else if (type == DecorationType.Tail)
            {
                if (GlobalManager.Instance.PlayerCarTrailName == skinItemData.ModelName)//使用中
                {
                    m_InUseBtn.SetActive(true);
                    m_Bg.GetComponent<Image>().sprite = m_BgList[0];
                }
                else
                {
                    m_UseBtn.SetActive(true);
                    m_Bg.GetComponent<Image>().sprite = m_BgList[1];
                }
            }
            else if (type == DecorationType.Terrain)
            {
                if (GlobalManager.Instance.PlayerMapSkinName == skinItemData.ModelName)//使用中
                {
                    m_InUseBtn.SetActive(true);
                    m_Bg.GetComponent<Image>().sprite = m_BgList[0];
                }
                else
                {
                    m_UseBtn.SetActive(true);
                    m_Bg.GetComponent<Image>().sprite = m_BgList[1];
                }
            }
        }
        else //未解锁
        {
            //通过下划线分割字符串
            string[] unlockConditions = skinItemData.UnlockConditions.Split('_');
            if (unlockConditions[0] == "1")//普通关卡
            {
                if (GlobalManager.Instance.CurrentLevel >= int.Parse(unlockConditions[1]))
                {
                    m_UnLockBtn.SetActive(true);
                    m_Bg.GetComponent<Image>().sprite = m_BgList[1];
                    mUISwitchSkin.OnNewSkin("Skin");
                }
                else
                {
                    m_Bg.GetComponent<Image>().sprite = m_BgList[2];
                    m_Lock.SetActive(true);
                    m_UnLockDesc.SetActive(true);
                    m_UnLockDesc.GetComponent<TMP_Text>().text = "通关第" + unlockConditions[1] + "关解锁";
                }
            }
            else if (unlockConditions[0] == "2")//困难关卡
            {
                if (GlobalManager.Instance.CurrentHardLevel >= int.Parse(unlockConditions[1]))
                {
                    m_UnLockBtn.SetActive(true);
                    m_Bg.GetComponent<Image>().sprite = m_BgList[1];
                    mUISwitchSkin.OnNewSkin("Skin");
                }
                else
                {
                    m_Bg.GetComponent<Image>().sprite = m_BgList[2];
                    m_Lock.SetActive(true);
                    m_UnLockDesc.SetActive(true);
                    m_UnLockDesc.GetComponent<TMP_Text>().text = "通关困难关卡第" + unlockConditions[1] + "关解锁";
                }
            }
        }

    }
    //解锁按钮
    public void OnUnLockBtnClick()
    {
        if (GlobalManager.Instance.PlayerCoin < skinItemData.Coins) //金币不足
        {
            return;
        }
        GlobalManager.Instance.PlayerCoin -= skinItemData.Coins;
        PlayerPrefs.SetString(type.ToString() + index.ToString(), "1");
        //隐藏解锁按钮
        m_UnLockBtn.SetActive(false);
        //显示使用按钮
        m_UseBtn.SetActive(true);
        mUISwitchSkin.mLoopGridView.RefreshAllShownItem();
    }
    //使用按钮
    public void OnUseBtnClick()
    {
        m_UseBtn.SetActive(false);
        m_InUseBtn.SetActive(true);
        if (type == DecorationType.Car)
        {
            GlobalManager.Instance.PlayerCarSkinName = skinItemData.ModelName;
        }
        else if (type == DecorationType.Tail)
        {
            GlobalManager.Instance.PlayerCarTrailName = skinItemData.ModelName;
        }
        else if (type == DecorationType.Terrain)
        {
            GlobalManager.Instance.PlayerMapSkinName = skinItemData.ModelName;
        }
        GlobalManager.Instance.SaveGameData();
        mUISwitchSkin.ChangeSkin(type, skinItemData.ModelName);
        mUISwitchSkin.mLoopGridView.RefreshAllShownItem();
    }
}
