using System.Collections;
using System.Collections.Generic;
using SuperScrollView;
using UnityEngine;

public class HardItemData {
    public string mLevelID;

    //解锁关卡数
    public int mUnlockLevelCount;
}

public class UIHardMode : MonoBehaviour {
    public LoopGridView mLoopGridView;
    private List<HardItemData> hardItems = new List<HardItemData>();

    public GameObject m_HardModeRoot;

    //通知玩家有新的困难模式关卡的委托事件
    public delegate void NewHardModeLevel(string msg);

    public NewHardModeLevel OnNewHardModeLevel;

    //关闭红点委托
    public delegate void CloseRedPoint(string message);

    public CloseRedPoint CloseRedPointEvent;
    public GameObject m_UICoin;

    void Start() {
        hardItems.Add(new HardItemData {
            mLevelID = "1",
            mUnlockLevelCount = 16,
        });
        hardItems.Add(new HardItemData {
            mLevelID = "2",
            mUnlockLevelCount = 20,
        });
        hardItems.Add(new HardItemData {
            mLevelID = "3",
            mUnlockLevelCount = 25,
        });
        // 初始化数据
        for (int i = 0; i < 267; i++) {
            hardItems.Add(new HardItemData {
                mLevelID = (i + 4).ToString(),
                mUnlockLevelCount = 25 + (i + 1) * 10,
            });
        }

        // 设置列表项数量
        // mLoopGridView.Padding.left = (Screen.width - 960) / 2;
        mLoopGridView.InitGridView(hardItems.Count, OnGetItemByRowColumn);
    }

    LoopGridViewItem OnGetItemByRowColumn(LoopGridView gridView, int index, int row, int column) {
        if (index < 0) {
            return null;
        }


        LoopGridViewItem item = null;

        //get the data to showing
        HardItemData itemData = hardItems[index];
        if (itemData == null) {
            return null;
        }

        item = gridView.NewListViewItem("HardItem");
        var itemScript = item.GetComponent<HardItem>();
        itemScript.SetItemData(itemData, this);
        itemScript.Init();
        return item;
    }

    public void OnCloseBtnClicked() {
        m_HardModeRoot.SetActive(false);
        CloseRedPointEvent?.Invoke("Hard");
    }

    public void OnHardModeBtn() {
        m_HardModeRoot.SetActive(true);
        StartCoroutine(LogHelper.LogToServer("ClickModule", new Dictionary<string, object>() {
            { "ModuleId", "C5" }
        }));
    }
}
