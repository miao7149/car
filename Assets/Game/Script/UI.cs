using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    void Start()
    {
        ShowCarHitPeopleRoot(false);
        ShowActionOverRoot(false);
        ShowFinishRoot(false);
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
    //点击重试按钮
    public void OnTouchRetry()
    {
        GameManager.Instance.ContinueGame();
    }
    public void OnContinue()//点击继续按钮
    {

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
}
