using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using SuperScrollView;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGameVictoryPage : MonoBehaviour
{
    // Start is called before the first frame update
    //完成界面根节点
    public GameObject FinishRoot;
    //初始完成界面UI
    public GameObject m_Beginner;
    //进阶完成界面UI
    public GameObject m_Advance;
    //竞速赛完成界面UI
    public GameObject m_Racing;
    /************************************************************************/
    //初始完成界面子节点UI
    //右上角金币
    public GameObject m_TargetUI;
    // 金币图片的预制体
    public GameObject m_CoinPrefab;
    public GameObject m_CoinContainer; // 存放金币图片的容器
    float frameDuration = 0.05f; // 每帧的持续时间
    //金币数量文本
    public TMP_Text m_FinishCoinText;
    //光环
    public GameObject m_Halo;
    /************************************************************************/
    //进阶完成界面子节点UI
    //金币
    public GameObject m_AdvanceCoin;
    //奖杯
    public GameObject m_AdvanceCup;
    public LoopListView2 loopListView;
    //当前晋升描述
    public GameObject m_CurrentPromotionDesc;
    //当前晋升人数
    public GameObject m_CurrentPromotionNum;
    //奖杯图片
    public GameObject m_RankImage;
    //倒计时文本
    public TMP_Text m_CountDownText;
    //七天倒计时（秒）
    private const float mCountDownTime = 7 * 24 * 60 * 60;
    private double remainingTime; // 剩余时间
    //段位图片
    public Sprite[] m_RankSprites;

    /************************************************************************/
    //竞速赛完成界面子节点UI
    //玩家列表
    public List<GameObject> m_PlayerList;
    //玩家车辆图片
    public Sprite m_PlayerIcon;
    //其他玩家车辆图片
    public Sprite m_OtherPlayerIcon;
    void Start()
    {
        //初始化界面
        FinishRoot.SetActive(false);
        m_Beginner.SetActive(false);
        m_Advance.SetActive(false);
        m_Racing.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    //显示竞速赛完成界面
    public void ShowRacingFinishRoot()
    {
        FinishRoot.SetActive(true);
        m_Racing.SetActive(true);
        m_TargetUI.SetActive(true);
        GlobalManager.Instance.RefreshPlayerInfoList();
        var playerInfoList = GlobalManager.Instance._playerInfoList;
        for (int i = 0; i < m_PlayerList.Count; i++)
        {
            if (m_PlayerList[i] != null && playerInfoList[i] != null)
            {
                m_PlayerList[i].transform.GetChild(1).GetComponent<TMP_Text>().text = playerInfoList[i].Name;
                m_PlayerList[i].transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = playerInfoList[i].CompleteLevel.ToString() + "/30";
                if (playerInfoList[i].Id >= 0)
                {
                    m_PlayerList[i].GetComponent<Image>().sprite = m_OtherPlayerIcon;
                }
                else
                {
                    m_PlayerList[i].GetComponent<Image>().sprite = m_PlayerIcon;
                }
            }
        }
        m_Racing.transform.DOLocalMoveY(m_Racing.transform.localPosition.y + m_Racing.GetComponent<RectTransform>().sizeDelta.y, 0.5f);
    }
    //显示进阶完成界面
    public void ShowAdvanceFinishRoot(int coinCount = 0, int trophyCount = 0)
    {
        GlobalManager.Instance.PlayerCoin += coinCount;
        GlobalManager.Instance._selfPlayerInfo.TrophyProp.Trophy.Count += trophyCount;
        FinishRoot.SetActive(true);
        m_Advance.SetActive(true);
        m_TargetUI.SetActive(true);
        GlobalManager.Instance.RefreshTrophyRankingList();
        loopListView.InitListView(GlobalManager.Instance._trophyRankingList.Count, OnGetItemByIndex);
        var PlayerListIndex = GlobalManager.Instance.GetRankIndex();
        if (PlayerListIndex > 29)
        {
            m_CurrentPromotionNum.GetComponent<TMP_Text>().text = (PlayerListIndex - 29).ToString();
        }
        else
        {
            m_CurrentPromotionNum.GetComponent<TMP_Text>().text = "0";

        }
        m_RankImage.GetComponent<Image>().sprite = m_RankSprites[GlobalManager.Instance.CurrentRank - 1];//设置段位图片
        m_RankImage.GetComponent<Image>().SetNativeSize();
        m_CurrentPromotionDesc.GetComponent<TMP_Text>().text = "There are                 people left to reach the promotion rank";
        //获取开始时间
        string startDate = PlayerPrefs.GetString("RankingStartDate");
        if (startDate != "")
        {
            GlobalManager.Instance.StartDate = Convert.ToDateTime(startDate);
            //计算剩余时间
            remainingTime = mCountDownTime - (DateTime.Now - GlobalManager.Instance.StartDate).TotalSeconds;
            if (remainingTime <= 0)
            {
                m_CountDownText.text = "已结束";
            }
            else
            {
                //更新倒计时文本，只显示天数和小时数
                int days = (int)(remainingTime / (24 * 60 * 60));
                int hours = (int)((remainingTime % (24 * 60 * 60)) / (60 * 60));
                m_CountDownText.text = $"{days}天 {hours}小时";
            }
            if (coinCount > 0 && trophyCount > 0)
            {
                m_AdvanceCoin.transform.GetChild(0).GetComponent<TMP_Text>().text = "X" + coinCount.ToString();
                m_AdvanceCup.transform.GetChild(0).GetComponent<TMP_Text>().text = "X" + trophyCount.ToString();
            }
        }
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
    //显示完成界面
    public void ShowBeginnerFinishRoot(int coinCount = 0)
    {
        FinishRoot.SetActive(true);
        m_TargetUI.SetActive(true);
        m_Beginner.SetActive(true);
        if (coinCount > 0)
        {
            m_FinishCoinText.text = "X" + coinCount.ToString();
            m_Halo.SetActive(true);
            //顺时针一直旋转
            m_Halo.transform.DORotate(new Vector3(0, 0, -360), 5f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
            m_FinishCoinText.transform.parent.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                GlobalManager.Instance.PlayerCoin += coinCount;
                CreateAndAnimateCoins(coinCount);
            });
        }
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
    }
}
