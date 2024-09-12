﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SuperScrollView
{
    public class ButtonPanelGridViewLoad
    {
        public LoopGridView mLoopGridView;
        public DataSourceMgr<ItemData> mDataSourceMgr;
        public int mExtraHeaderItemCount = 0;
        public int mExtraFooterItemCount = 0;
        Button mSetCountButton;
        InputField mSetCountInput;
        Button mScrollToButton;
        InputField mScrollToInput;
        Button mAddButton;
        InputField mAddInput;
        Button mBackButton;        

        public void Start()
        {
            mSetCountButton = GameObject.Find("ButtonPanel/ButtonGroupSetCount/SetCountButton").GetComponent<Button>();
            mSetCountInput = GameObject.Find("ButtonPanel/ButtonGroupSetCount/SetCountInputField").GetComponent<InputField>();
            mSetCountButton.onClick.AddListener(OnSetCountButtonClicked);
            
            mScrollToButton = GameObject.Find("ButtonPanel/ButtonGroupScrollTo/ScrollToButton").GetComponent<Button>();
            mScrollToInput = GameObject.Find("ButtonPanel/ButtonGroupScrollTo/ScrollToInputField").GetComponent<InputField>();
            mScrollToButton.onClick.AddListener(onScrollToButtonClicked);

            mAddButton = GameObject.Find("ButtonPanel/ButtonGroupAdd/AddButton").GetComponent<Button>();
            mAddButton.onClick.AddListener(OnAddButtonClicked);
            mAddInput = GameObject.Find("ButtonPanel/ButtonGroupAdd/AddInputField").GetComponent<InputField>();

            mBackButton = GameObject.Find("ButtonPanel/BackButton").GetComponent<Button>();
            mBackButton.onClick.AddListener(OnBackButtonClicked);
        }              

        void OnSetCountButtonClicked()
        {
            int count = 0;
            if (int.TryParse(mSetCountInput.text, out count) == false)
            {
                return;
            }
            if (count < 0)
            {
                return;
            }
            mDataSourceMgr.SetDataTotalCount(count);
            int extraCount = mExtraHeaderItemCount + mExtraFooterItemCount;
            mLoopGridView.SetListItemCount(count+extraCount, false);
            mLoopGridView.RefreshAllShownItem();
        }

        void onScrollToButtonClicked()
        {
            int itemIndex = 0;
            if (int.TryParse(mScrollToInput.text, out itemIndex) == false)
            {
                return;
            }
            if((itemIndex < 0) || (itemIndex >= mDataSourceMgr.TotalItemCount))
            {
                return;
            }    
            int tmpIndex = itemIndex + mExtraHeaderItemCount;    
            mLoopGridView.MovePanelToItemByIndex(tmpIndex, 0);
        }

        void OnAddButtonClicked()
        {
            int itemIndex = 0;
            if (int.TryParse(mAddInput.text, out itemIndex) == false)
            {
                return;
            }
            
            if((itemIndex < 0) || (itemIndex > mDataSourceMgr.TotalItemCount))
            {
                return;
            }       
            ItemData newData = mDataSourceMgr.InsertData(itemIndex);
            newData.mDesc = newData.mDesc +" [New]";
            int extraCount = mExtraHeaderItemCount + mExtraFooterItemCount;
            mLoopGridView.SetListItemCount(mDataSourceMgr.TotalItemCount+extraCount, false);
            mLoopGridView.RefreshAllShownItem();
        }
        
        void OnBackButtonClicked()
        {
            ButtonPanelMenuList.BackToMainMenu();
        }         
    }
}
