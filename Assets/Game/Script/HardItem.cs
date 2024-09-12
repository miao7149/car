using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class HardItem : MonoBehaviour
{
    // Start is called before the first frame update
    //已完成关卡根节点
    public GameObject m_CompletedRoot;
    //已解锁关卡根节点
    public GameObject m_UnlockRoot;
    //可解锁关卡根节点
    public GameObject m_CanUnlockRoot;
    //未解锁关卡根节点
    public GameObject m_LockRoot;
    //关卡ID
    public TMP_Text m_LevelID;
    //HardItemData
    private HardItemData mHardItemData;
    enum HardItemState
    {
        //已完成
        Completed,
        //已解锁
        Unlock,
        //可解锁
        CanUnlock,
        //未解锁
        Lock
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Init()
    {
        var hardLevelStatus = PlayerPrefs.GetInt("HardLevelStatus" + mHardItemData.mLevelID, -1);//0已完成 1已解锁 
        //已完成
        if (GlobalManager.Instance.CurrentLevel > mHardItemData.mUnlockLevelCount && hardLevelStatus == 0)
        {
            m_CompletedRoot.SetActive(true);
            m_UnlockRoot.SetActive(false);
            m_CanUnlockRoot.SetActive(false);
            m_LockRoot.SetActive(false);
        }
        else if (GlobalManager.Instance.CurrentLevel > mHardItemData.mUnlockLevelCount && hardLevelStatus == 1)
        {
            //已解锁
            m_CompletedRoot.SetActive(false);
            m_UnlockRoot.SetActive(true);
            m_CanUnlockRoot.SetActive(false);
            m_LockRoot.SetActive(false);
        }
        else if (GlobalManager.Instance.CurrentLevel > mHardItemData.mUnlockLevelCount)
        {
            //可解锁
            m_CompletedRoot.SetActive(false);
            m_UnlockRoot.SetActive(false);
            m_CanUnlockRoot.SetActive(true);
            m_LockRoot.SetActive(false);
        }
        else
        {
            //未解锁
            m_CompletedRoot.SetActive(false);
            m_UnlockRoot.SetActive(false);
            m_CanUnlockRoot.SetActive(false);
            m_LockRoot.SetActive(true);
            m_LockRoot.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = "Complete\nlevel " + mHardItemData.mUnlockLevelCount.ToString();
        }
    }
    public void SetItemData(HardItemData data)
    {
        mHardItemData = data;
        m_LevelID.text = data.mLevelID;
    }
    //重玩按钮
    public void OnReplayBtn()
    {
        GlobalManager.Instance.GameType = GameType.ChallengeHard;
        GlobalManager.Instance.CurrentHardLevel = int.Parse(mHardItemData.mLevelID);
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }
    //已解锁按钮
    public void OnPlayBtn()
    {
        GlobalManager.Instance.GameType = GameType.ChallengeHard;
        GlobalManager.Instance.CurrentHardLevel = int.Parse(mHardItemData.mLevelID) - 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }
    //可解锁按钮
    public void OnUnlockBtn()
    {
        if (GlobalManager.Instance.PlayerCoin >= 100)
        {
            GlobalManager.Instance.PlayerCoin -= 100;
            PlayerPrefs.SetInt("HardLevelStatus" + mHardItemData.mLevelID, 1);
            GlobalManager.Instance.SaveGameData();
            Init();
        }
    }
}
