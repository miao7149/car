using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UI : MonoBehaviour
{
    // Start is called before the first frame update
    public Text ItemCountText;//道具数量
    public Text ActionCountText;//行动次数
    //车撞行人失败界面根节点
    public GameObject CarHitPeopleRoot;
    //步数耗尽失败界面根节点
    public GameObject ActionOverRoot;
    //完成界面根节点
    public GameObject FinishRoot;
    //设置界面根节点
    public GameObject SettingRoot;
    /////////////////////////////////////////////多语言设置，文本物体
    //步数
    public Text m_StepText;
    //游戏结束
    public Text m_GameOverText;
    //游戏结束2
    public Text m_GameOverText2;
    //再来一次
    public Text m_PlayAgainText;
    //花费金币
    public Text m_CostCoinText;
    //观看广告
    public Text m_WatchAdText;

    void Start()
    {
        ShowCarHitPeopleRoot(false);
        ShowActionOverRoot(false);
        ShowFinishRoot(false);
    }
    public void SetLanguage()
    {

    }
    // Update is called once per frame
    void Update()
    {
    }

    public void OnTouchReplay()
    {
        GameManager.Instance.InitGame();
    }
    //道具使用按钮
    public void OnUseItemBtn()
    {
        if (GlobalManager.Instance.ItemCount > 0)
        {
            GameManager.Instance.IsUseItem = true;
            Debug.Log("使用道具");
        }
        else
        {
            Debug.Log("道具不足");
            GlobalManager.Instance.ItemCount++;
            ChangeItemCount(GlobalManager.Instance.ItemCount);
        }
        AudioManager.Instance.PlayButtonClick();
    }
    //更改道具数量
    public void ChangeItemCount(int count)
    {
        ItemCountText.text = count.ToString();
    }
    //更新行动次数
    public void ChangeActionCount(int count)
    {
        ActionCountText.text = "Moves:" + count.ToString();
    }
    //复活按钮
    public void OnTouchRevive()
    {
        GameManager.Instance.ContinueGame();
        AudioManager.Instance.PlayButtonClick();
    }
    public void OnContinue()//点击继续按钮
    {
        AudioManager.Instance.PlayButtonClick();
        SceneManager.LoadScene("MenuScene");
    }
    //显示车撞行人失败界面
    public void ShowCarHitPeopleRoot(bool isShow)
    {
        CarHitPeopleRoot.SetActive(isShow);
    }
    //显示步数耗尽失败界面
    public void ShowActionOverRoot(bool isShow)
    {
        ActionOverRoot.SetActive(isShow);
    }
    //显示完成界面
    public void ShowFinishRoot(bool isShow)
    {
        FinishRoot.SetActive(isShow);
    }
    //设置按钮
    public void OnSetting()
    {
        AudioManager.Instance.PlayButtonClick();
        SettingRoot.SetActive(SettingRoot.activeSelf ? false : true);
    }
    //震动开启关闭按钮
    public void OnClickVibrate()
    {
        GlobalManager.Instance.IsVibrate = !GlobalManager.Instance.IsVibrate;
    }
    //音效开启关闭按钮
    public void OnClickSound()
    {
        GlobalManager.Instance.IsSound = !GlobalManager.Instance.IsSound;
        if (GlobalManager.Instance.IsSound)
        {
            AudioManager.Instance.ResumeBackgroundMusic();
        }
        else
        {
            AudioManager.Instance.StopBackgroundMusic();
        }
    }
}
