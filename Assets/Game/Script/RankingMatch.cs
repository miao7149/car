using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using SuperScrollView;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class RankingMatch : MonoBehaviour
{
    // Start is called before the first frame update
    public LoopListView2 loopListView;
    bool IsInitLoopListView = false;
    public GameObject m_RankingMatchRoot;
    //奖杯图片
    public GameObject m_RankImage;
    //倒计时文本
    public TMP_Text m_CountDownText;
    //七天倒计时（秒）
    private const float mCountDownTime = 7 * 24 * 60 * 60;
    private double remainingTime; // 剩余时间
    //继续按钮
    public GameObject m_ContinueBtn;
    //领取按钮
    public GameObject m_GetBtn;
    //结算界面
    public GameObject m_SettlementRoot;
    //标题
    public GameObject m_Title;
    //排位列表
    public GameObject m_RankList;
    //详情按钮
    public GameObject m_DetailBtn;
    //段位按钮
    public GameObject m_RankBtn;
    //段位图片列表
    public Sprite[] m_RankSprites;
    //金币奖励物体
    public GameObject m_GoldReward;
    //道具奖励物体
    public GameObject m_ItemReward;
    //当前排名描述
    public GameObject m_CurrentRankDesc;
    //当前段位描述
    public GameObject m_CurrentRankDesc2;
    //当前晋升描述
    public GameObject m_CurrentPromotionDesc;
    //当前晋升人数
    public GameObject m_CurrentPromotionNum;
    public GameObject m_CoinPrefab; // 金币预制体
    public GameObject m_TargetUI; // 目标UI
    float frameDuration = 0.05f; // 每帧的持续时间
    public GameObject m_CoinContainer; // 金币容器
    //主界面
    public GameObject m_MainRoot;
    //段位界面
    public GameObject m_RankRoot;
    //汽车
    public GameObject m_Car;
    //段位物体列表
    public GameObject[] m_RankItems;
    void Start()
    {
        if (GlobalManager.Instance.CurrentLevel >= 15 && GlobalManager.Instance.mIsStartRankingMatch == false)
        {
            GlobalManager.Instance.mIsStartRankingMatch = true;
            GlobalManager.Instance.StartDate = DateTime.Now;
            //保存开始时间
            PlayerPrefs.SetString("RankingStartDate", GlobalManager.Instance.StartDate.ToString());
            PlayerPrefs.SetInt("IsStartRankingMatch", 1);
            m_RankingMatchRoot.SetActive(true);
            RefreshRankingMatch();
        }
        else
        {
            RefreshRankingMatch();
        }
    }
    public void RefreshRankingMatch()
    {
        //获取开始时间
        string startDate = PlayerPrefs.GetString("RankingStartDate");
        if (startDate != "")
        {
            GlobalManager.Instance.StartDate = Convert.ToDateTime(startDate);
            //计算剩余时间
            remainingTime = mCountDownTime - (DateTime.Now - GlobalManager.Instance.StartDate).TotalSeconds;
            if (remainingTime <= 0)
            {
                //如果排位赛时间到，主动弹出排位赛界面
                remainingTime = 0;
                m_RankingMatchRoot.SetActive(true);
                m_ContinueBtn.SetActive(false);
                m_Title.SetActive(false);
                m_GetBtn.SetActive(true);
                m_CountDownText.text = "已结束";
            }
            else
            {
                m_ContinueBtn.SetActive(true);
                m_GetBtn.SetActive(false);
                m_MainRoot.SetActive(true);
                m_RankRoot.SetActive(false);
                //更新倒计时文本，只显示天数和小时数
                int days = (int)(remainingTime / (24 * 60 * 60));
                int hours = (int)((remainingTime % (24 * 60 * 60)) / (60 * 60));
                m_CountDownText.text = $"{days}天 {hours}小时";
            }
            m_RankImage.GetComponent<Image>().sprite = m_RankSprites[GlobalManager.Instance.CurrentRank - 1];//设置段位图片
            m_RankImage.GetComponent<Image>().SetNativeSize();
            GlobalManager.Instance.RefreshTrophyRankingList();
            if (IsInitLoopListView == false)
            {
                IsInitLoopListView = true;
                loopListView.InitListView(GlobalManager.Instance._trophyRankingList.Count, OnGetItemByIndex);
            }
            var PlayerListIndex = GlobalManager.Instance.GetRankIndex();
            if (PlayerListIndex > 29)
            {
                m_CurrentPromotionNum.GetComponent<TMP_Text>().text = (PlayerListIndex - 29).ToString();
                if (remainingTime <= 0)
                    GlobalManager.Instance.CurrentRank--;
            }
            else
            {
                m_CurrentPromotionNum.GetComponent<TMP_Text>().text = "0";
                if (remainingTime <= 0)
                    GlobalManager.Instance.CurrentRank++;

            }
            m_CurrentPromotionDesc.GetComponent<TMP_Text>().text = "There are                 people left to reach the promotion rank";
        }
        else
        {
            GlobalManager.Instance.mIsStartRankingMatch = false;
            PlayerPrefs.SetInt("IsStartRankingMatch", 0);
        }
    }
    // 获取列表项
    LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
    {
        if (index < 0 || index >= GlobalManager.Instance._trophyRankingList.Count)
        {
            return null;
        }
        LoopListViewItem2 item = null;
        // 获取或创建列表项
        if (index == 29)
        {
            item = listView.NewListViewItem("RankItemUp");
        }
        else if (index == 59 && GlobalManager.Instance.CurrentRank > 1)
        {
            item = listView.NewListViewItem("RankItemDown");
        }
        else
        {
            item = listView.NewListViewItem("RankItem");
        }
        item.GetComponent<RankingMatchItem>().Init(GlobalManager.Instance._trophyRankingList[index], index + 1);
        return item;
    }
    // Update is called once per frame
    void Update()
    {
    }
    public void OnCloseBtn()
    {
        if (remainingTime <= 0)
        {
            GlobalManager.Instance.StartDate = DateTime.Now;
            //保存开始时间
            PlayerPrefs.SetString("RankingStartDate", GlobalManager.Instance.StartDate.ToString());
        }
        m_RankingMatchRoot.SetActive(false);
    }
    public void OnRankingMatchBtn()
    {
        if (GlobalManager.Instance.CurrentLevel < 15)
        {
            return;
        }
        m_RankingMatchRoot.SetActive(true);
        RefreshRankingMatch();
        var PlayerListIndex = GlobalManager.Instance.GetRankIndex();
        StartCoroutine(SmoothScrollToIndex(PlayerListIndex, 1.5f));
    }
    private IEnumerator SmoothScrollToIndex(int targetIndex, float duration)
    {
        int startIndex = 0;
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;
            int currentIndex = (int)Mathf.Lerp(startIndex, targetIndex, t);
            loopListView.MovePanelToItemIndex(currentIndex, loopListView.GetComponent<RectTransform>().rect.height / 2);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        loopListView.MovePanelToItemIndex(targetIndex, loopListView.GetComponent<RectTransform>().rect.height / 2);
    }
    //领取按钮
    public void OnGetBtn()
    {
        var propArray = GlobalManager.Instance._selfPlayerInfo.TrophyProp.PropArray;
        m_GoldReward.GetComponent<Image>().DOFade(0, 0.3f).OnComplete(() =>
        {
            if (propArray.Length == 1)
            {
                GlobalManager.Instance.PlayerCoin += propArray[0].Count;
            }
            else if (propArray.Length == 2)
            {
                GlobalManager.Instance.PlayerCoin += propArray[0].Count;
                GlobalManager.Instance.ItemCount += propArray[1].Count;
            }
            CreateAndAnimateCoins(propArray[0].Count); // 假设金币数量为50
        });
        m_ItemReward.transform.DOScale(1.2f, 0.3f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            m_ItemReward.transform.DOScale(0.3f, 0.6f).SetEase(Ease.OutQuad);
            m_ItemReward.GetComponent<Image>().DOFade(0, 0.6f);
        });
    }
    private void CreateAndAnimateCoins(int goldAmount)
    {
        AudioManager.Instance.PlayCoinSettle();
        int coinCount = Mathf.Min(goldAmount, 30);
        for (int i = 0; i < coinCount; i++)
        {
            GameObject coin = Instantiate(m_CoinPrefab, m_CoinContainer.transform);
            if (coin == null)
            {
                return;
            }
            coin.transform.localPosition = Vector3.zero; // 初始位置
            Sequence sequence = DOTween.Sequence();
            // 随机散开
            Vector3 randomPosition = new Vector3(UnityEngine.Random.Range(-200, 200), UnityEngine.Random.Range(-200, 200), 0);
            coin.transform.DOLocalMove(randomPosition, 0.5f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                // 等待0.1秒
                DOVirtual.DelayedCall(0.3f, () =>
                {
                    // 飞向目标UI
                    float randomDuration = UnityEngine.Random.Range(0.3f, 0.8f); // 随机飞行时间
                    coin.transform.DOMove(m_TargetUI.transform.position, randomDuration).SetEase(Ease.InQuad).OnComplete(() =>
                    {
                        m_TargetUI.GetComponent<UICoin>().UpdateCoin();
                        coin.SetActive(false);
                        sequence.Kill();
                    });
                });
            });
            // 序列帧动画
            foreach (var sprite in GlobalManager.Instance.m_CoinSprites)
            {
                sequence.AppendCallback(() => coin.GetComponent<Image>().sprite = sprite);
                sequence.AppendInterval(frameDuration);
            }
            sequence.SetLoops(-1, LoopType.Restart);
        }
        DOVirtual.DelayedCall(2f, () =>
       {
           m_SettlementRoot.SetActive(false);
           m_Title.SetActive(true);
           m_RankList.SetActive(true);
           m_DetailBtn.SetActive(true);
           ShowRankPage();
       });
    }
    //打开领取界面
    public void OnOpenGetPageBtn()
    {
        m_SettlementRoot.SetActive(true);
        m_RankList.SetActive(false);
        m_DetailBtn.SetActive(false);
        m_RankBtn.SetActive(false);
        m_GetBtn.SetActive(false);
        ShowRewardItem();
    }
    //获取奖励
    public void ShowRewardItem()
    {
        var _selfPlayerInfo = GlobalManager.Instance._selfPlayerInfo;
        var propArray = _selfPlayerInfo.TrophyProp.PropArray;
        if (propArray.Length == 1)//只有金币
        {
            m_GoldReward.SetActive(true);
            m_ItemReward.SetActive(false);
            m_GoldReward.transform.localPosition = new Vector3(0, m_GoldReward.transform.localPosition.y, 0);
            m_GoldReward.transform.GetChild(0).GetComponent<TMP_Text>().text = "X" + propArray[0].Count.ToString();
        }
        else if (propArray.Length == 2)//金币和道具
        {
            if (propArray[2].Type == Prop.Ballon)
            {
                m_GoldReward.SetActive(true);
                m_ItemReward.SetActive(true);
                m_GoldReward.transform.GetChild(0).GetComponent<TMP_Text>().text = "X" + propArray[0].Count.ToString();
                m_ItemReward.transform.GetChild(0).GetComponent<TMP_Text>().text = "X" + propArray[1].Count.ToString();
            }
            else
            {
                m_GoldReward.SetActive(true);
                m_ItemReward.SetActive(false);
                m_GoldReward.transform.localPosition = new Vector3(0, m_GoldReward.transform.localPosition.y, 0);
                m_GoldReward.transform.GetChild(0).GetComponent<TMP_Text>().text = "X" + propArray[0].Count.ToString();
            }
        }
        var PlayerListIndex = GlobalManager.Instance.GetRankIndex();
        m_CurrentRankDesc.GetComponent<TMP_Text>().text = "当前排名：" + PlayerListIndex;
        m_CurrentRankDesc2.GetComponent<TMP_Text>().text = "当前段位：" + GlobalManager.Instance.CurrentRank;
    }
    //显示排位赛详情
    public void ShowRankPage()
    {
        m_MainRoot.SetActive(false);
        m_RankRoot.SetActive(true);
        m_Car.transform.DOMoveY(m_RankItems[GlobalManager.Instance.CurrentRank - 1].transform.position.y, 0.5f).SetEase(Ease.OutQuad);
    }
}
