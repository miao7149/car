using System;
using System.Collections;
using System.Collections.Generic;
using SuperScrollView;
using UnityEngine;

public class UISwitchSkin : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> BtnMaskList;
    public LoopGridView mLoopGridView;
    DecorationType mCurrentType = DecorationType.Car;
    void Start()
    {
        mLoopGridView.InitGridView(GlobalManager.Instance.mTrucksAppearanceDict.Count, OnGetItemByRowColumn);
        BtnMaskList[0].SetActive(true);
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
        itemScript.Init(itemData, mCurrentType, index);
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
}
