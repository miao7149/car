using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using SuperScrollView;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISwitchSkin : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> BtnMaskList;
    public LoopGridView mLoopGridView;
    DecorationType mCurrentType = DecorationType.Car;
    public GameObject m_SkinRoot;
    public GameObject m_SkinMap;
    public GameObject m_RawImage;
    public GameObject m_SwitchSkinRoot;
    //通知玩家有新的皮肤的委托
    public delegate void NewSkin(string msg);
    public NewSkin OnNewSkin;
    //关闭红点委托
    public delegate void CloseRedPoint(string message);
    public CloseRedPoint CloseRedPointEvent;
    //主界面汽车
    public GameObject m_Car;
    //金币UI
    public GameObject m_UICoin;
    /////////////////////////////////////////////多语言设置，文本物体
    //标题
    public TMP_Text m_Title;
    //汽车按钮文本
    public TMP_Text m_CarText;
    //尾巴按钮文本
    public TMP_Text m_TailText;
    //地图按钮文本
    public TMP_Text m_MapText;
    //汽车遮罩按钮文本
    public TMP_Text m_CarMaskText;
    //尾巴遮罩按钮文本
    public TMP_Text m_TailMaskText;
    //地图遮罩按钮文本
    public TMP_Text m_MapMaskText;
    void Start()
    {
        mLoopGridView.InitGridView(GlobalManager.Instance.mTrucksAppearanceDict.Count, OnGetItemByRowColumn);
        mLoopGridView.RefreshAllShownItem();
        BtnMaskList[0].SetActive(true);
        Instantiate(Resources.Load<GameObject>("Prefabs/" + GlobalManager.Instance.PlayerCarSkinName), m_SkinRoot.transform);
        Instantiate(Resources.Load<GameObject>("Prefabs/" + GlobalManager.Instance.PlayerCarTrailName), m_SkinRoot.transform);
        m_SkinMap.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/Skin/" + GlobalManager.Instance.PlayerMapSkinName);
        CheckNewSkin();
        SetLanguage();
    }
    public void SetLanguage()
    {
        m_Title.text = GlobalManager.Instance.GetLanguageValue("Skin");
        m_CarText.text = GlobalManager.Instance.GetLanguageValue("Vehicle");
        m_TailText.text = GlobalManager.Instance.GetLanguageValue("Exhaust");
        m_MapText.text = GlobalManager.Instance.GetLanguageValue("Street");
        m_CarMaskText.text = GlobalManager.Instance.GetLanguageValue("Vehicle");
        m_TailMaskText.text = GlobalManager.Instance.GetLanguageValue("Exhaust");
        m_MapMaskText.text = GlobalManager.Instance.GetLanguageValue("Street");
    }
    LoopGridViewItem OnGetItemByRowColumn(LoopGridView gridView, int index, int row, int column)
    {
        if (index < 0)
        {
            return null;
        }
        LoopGridViewItem item = null;
        SkinItemData itemData = new();
        if (mCurrentType == DecorationType.Car)
        {
            itemData = GlobalManager.Instance.mTrucksAppearanceDict[index + 1];
        }
        else if (mCurrentType == DecorationType.Tail)
        {
            itemData = GlobalManager.Instance.mTrucksTrailDict[index + 1];
        }
        else if (mCurrentType == DecorationType.Terrain)
        {
            itemData = GlobalManager.Instance.TerrainMappingDict[index + 1];
        }
        item = gridView.NewListViewItem("SkinItem");
        var itemScript = item.GetComponent<SkinItem>();
        itemScript.Init(itemData, mCurrentType, index, this);
        return item;
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void OnSwitchBtn(string type)
    {
        mCurrentType = (DecorationType)Enum.Parse(typeof(DecorationType), type);
        switch (mCurrentType)
        {
            case DecorationType.Car:
                BtnMaskList[0].SetActive(true);
                BtnMaskList[1].SetActive(false);
                BtnMaskList[2].SetActive(false);
                mLoopGridView.SetListItemCount(GlobalManager.Instance.mTrucksAppearanceDict.Count, false);
                mLoopGridView.RefreshAllShownItem();
                break;
            case DecorationType.Tail:
                BtnMaskList[0].SetActive(false);
                BtnMaskList[1].SetActive(true);
                BtnMaskList[2].SetActive(false);
                mLoopGridView.SetListItemCount(GlobalManager.Instance.mTrucksTrailDict.Count, false);
                mLoopGridView.RefreshAllShownItem();
                break;
            case DecorationType.Terrain:
                BtnMaskList[0].SetActive(false);
                BtnMaskList[1].SetActive(false);
                BtnMaskList[2].SetActive(true);
                mLoopGridView.SetListItemCount(GlobalManager.Instance.TerrainMappingDict.Count, false);
                mLoopGridView.RefreshAllShownItem();
                break;
            default:
                break;
        }
        mLoopGridView.RefreshAllShownItem();
    }
    //更改皮肤
    public void ChangeSkin(DecorationType type, string skinName)
    {
        if (type == DecorationType.Terrain)
        {
            m_SkinMap.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/Skin/" + skinName);
        }
        else
        {
            m_RawImage.transform.DOMoveX(Screen.width + m_RawImage.GetComponent<RectTransform>().sizeDelta.x / 2, 0.5f).OnComplete(() =>
            {
                if (type == DecorationType.Car)
                {
                    //通过查找m_SkinRoot下的所有子物体，找到Tag为SkinCar的物体，然后删除物体，再加载新的皮肤
                    foreach (Transform child in m_SkinRoot.transform)
                    {
                        if (child.tag == "SkinCar")
                        {
                            Destroy(child.gameObject);
                        }
                    }
                    GameObject skinCar = Instantiate(Resources.Load<GameObject>("Prefabs/" + skinName));
                    skinCar.transform.SetParent(m_SkinRoot.transform);
                    skinCar.transform.localPosition = Vector3.zero;
                    //通过查找m_Car下的所有子物体，找到Tag为SkinCar的物体，然后删除物体，再加载新的皮肤
                    foreach (Transform child in m_Car.transform)
                    {
                        if (child.tag == "SkinCar")
                        {
                            Destroy(child.gameObject);
                        }
                    }
                    GameObject carSkin = Instantiate(Resources.Load<GameObject>("Prefabs/" + skinName));
                    carSkin.transform.SetParent(m_Car.transform);
                    carSkin.transform.localPosition = Vector3.zero;
                    carSkin.transform.localScale = new Vector3(1, 1, 1);
                }
                else if (type == DecorationType.Tail)
                {
                    foreach (Transform child in m_SkinRoot.transform)
                    {
                        if (child.tag == "SkinTail")
                        {
                            Destroy(child.gameObject);
                        }
                    }
                    GameObject skinTail = Instantiate(Resources.Load<GameObject>("Prefabs/" + skinName));
                    skinTail.transform.SetParent(m_SkinRoot.transform);
                    skinTail.transform.localPosition = Vector3.zero;
                }
                m_RawImage.GetComponent<RectTransform>().anchoredPosition = new Vector3(-Screen.width + m_RawImage.GetComponent<RectTransform>().sizeDelta.x / 2, m_RawImage.transform.localPosition.y, m_RawImage.transform.localPosition.z);
                m_RawImage.transform.DOMoveX(Screen.width / 2, 0.5f);
            });
        }
    }
    public void OnOpenSwitchBtn()
    {
        m_SwitchSkinRoot.SetActive(true);
    }
    public void OnCloseBtn()
    {
        m_SwitchSkinRoot.SetActive(false);
        CloseRedPointEvent?.Invoke("Skin");
    }
    //检测是否有新皮肤
    public void CheckNewSkin()
    {
        if (GlobalManager.Instance.mTrucksAppearanceDict.Count > 0)
        {
            foreach (var item in GlobalManager.Instance.mTrucksAppearanceDict)
            {
                CheckItemData(item.Value, item.Key - 1, DecorationType.Car);
            }
        }
        if (GlobalManager.Instance.mTrucksTrailDict.Count > 0)
        {
            foreach (var item in GlobalManager.Instance.mTrucksTrailDict)
            {
                CheckItemData(item.Value, item.Key - 1, DecorationType.Tail);
            }
        }
        if (GlobalManager.Instance.TerrainMappingDict.Count > 0)
        {
            foreach (var item in GlobalManager.Instance.TerrainMappingDict)
            {
                CheckItemData(item.Value, item.Key - 1, DecorationType.Terrain);
            }
        }
    }
    public void CheckItemData(SkinItemData skinItemData, int index, DecorationType decorationType)
    {
        var str = PlayerPrefs.GetString(decorationType.ToString() + index.ToString(), "0");
        if (index == 0)
        {
            str = "1";
        }
        else
        {
            str = PlayerPrefs.GetString(decorationType.ToString() + index.ToString(), "0");
        }
        if (str != "1") //未解锁
        {
            //通过下划线分割字符串
            string[] unlockConditions = skinItemData.UnlockConditions.Split('_');
            if (unlockConditions[0] == "1")//普通关卡
            {
                if (GlobalManager.Instance.CurrentLevel >= int.Parse(unlockConditions[1]))//可解锁
                {
                    OnNewSkin("Skin");
                    return;
                }
            }
            else if (unlockConditions[0] == "2")//困难关卡
            {
                if (GlobalManager.Instance.CurrentHardLevel >= int.Parse(unlockConditions[1]))
                {
                    OnNewSkin("Skin");
                    return;
                }
            }
        }
    }
}
