using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using SuperScrollView;
using UnityEngine;
using UnityEngine.UI;

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
    void Start()
    {
        mLoopGridView.InitGridView(GlobalManager.Instance.mTrucksAppearanceDict.Count, OnGetItemByRowColumn);
        BtnMaskList[0].SetActive(true);
        Instantiate(Resources.Load<GameObject>("Prefabs/" + GlobalManager.Instance.PlayerCarSkinName), m_SkinRoot.transform);
        Instantiate(Resources.Load<GameObject>("Prefabs/" + GlobalManager.Instance.PlayerCarTrailName), m_SkinRoot.transform);
        m_SkinMap.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/Skin/" + GlobalManager.Instance.PlayerMapSkinName);
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
    }
}
