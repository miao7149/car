using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class RacingPlayerItemData : IComparable<RacingPlayerItemData>
{
    //名称
    public string Name;
    //ID
    public int Id;
    //关卡增长速度
    public int CompleteTime;
    //关卡
    public int CompleteLevel;
    public int CompareTo(RacingPlayerItemData obj)
    {
        int CompleteLevel = this.CompleteLevel;
        if (CompleteLevel >= 30)
        {
            if (obj == null)
            {
                throw new NullReferenceException("obj == null");
            }
            if (obj.CompleteLevel >= 30)
            {
                return this.CompleteTime - obj.CompleteTime;
            }
        }
        else if (obj == null)
        {
            throw new NullReferenceException("obj == null");
        }
        int v4 = obj.CompleteLevel;
        int v5 = v4 - CompleteLevel;
        if (v4 != CompleteLevel || CompleteLevel > 29 || v4 > 29)
        {
            return v5;
        }
        int CompleteTime = this.Id;
        int Id = obj.Id;
        return CompleteTime - Id;
    }
}
public class RacingCompetition : MonoBehaviour
{
    private const int TotalLevels = 30; // 总关卡数
    private const float TotalTime = 3600f; // 一小时倒计时
    private double remainingTime; // 剩余时间
    public List<RacingPlayerItem> m_PlayerItems;//排行榜Item
    //倒计时文本
    public TMP_Text m_CountDownText;

    //开始比赛根节点
    public GameObject m_StartCompetitionRoot;
    //比赛中根节点
    public GameObject m_CompetitionRoot;
    //游戏描述文本
    public TMP_Text m_GameDescText;
    public GameObject m_CoinPrefab; // 金币预制体
    public GameObject m_CoinContainer; // 金币容器
    public GameObject m_TargetUI; // 目标UI
    public GameObject m_RewardRoot; // 奖励根节点
    public GameObject m_Halo; // 光晕
    float frameDuration = 0.05f; // 每帧的持续时间
    private int[] rewardCoins = new int[] { 1000, 700, 500 }; // 奖励金币
    //每日参赛次数
    private int mDailyCompetitionCount;
    //参赛时间
    private DateTime mCompetitionTime;
    //参赛次数文本
    public TMP_Text m_CompetitionCountText;
    void Start()
    {
        //如果当前时间和参赛时间相差大于一天，重置每日参赛次数
        if (PlayerPrefs.HasKey("CompetitionTime"))
        {
            mCompetitionTime = DateTime.Parse(PlayerPrefs.GetString("CompetitionTime"));
            if (DateTime.Now != mCompetitionTime)
            {
                mDailyCompetitionCount = 3;
                PlayerPrefs.SetInt("DailyCompetitionCount", mDailyCompetitionCount);
            }
            else
            {
                mDailyCompetitionCount = PlayerPrefs.GetInt("DailyCompetitionCount");
            }
        }
        else
        {
            mDailyCompetitionCount = 3;
            PlayerPrefs.SetInt("DailyCompetitionCount", mDailyCompetitionCount);
        }
        m_CompetitionCountText.text = mDailyCompetitionCount.ToString() + "/3";
        if (PlayerPrefs.HasKey("StartDate") && PlayerPrefs.HasKey("IsCompetition"))
        {
            Init();
        }
    }
    //检查当前游戏状态
    public void CheckGameStatus()
    {
        if (PlayerPrefs.HasKey("StartDate") == false && PlayerPrefs.HasKey("IsCompetition") == false)//没有比赛记录
        {
            m_StartCompetitionRoot.SetActive(true);
        }
        else
        {
            m_CompetitionRoot.SetActive(true);
            Init();
        }
    }
    public void Init()
    {
        //获取开始时间
        GlobalManager.Instance.RacingStartDate = DateTime.Parse(PlayerPrefs.GetString("StartDate"));
        remainingTime = TotalTime - (DateTime.Now - GlobalManager.Instance.RacingStartDate).TotalSeconds;
        GlobalManager.Instance.RacingStartLevel = PlayerPrefs.GetInt("StartLevel");
        //获取比赛状态
        GlobalManager.Instance.IsCompetition = PlayerPrefs.GetInt("IsCompetition") == 1;
        GlobalManager.Instance.RefreshPlayerInfoList();
        RefreshPlayerItem();
        if (GlobalManager.Instance.IsCompetition)//比赛进行中
        {
            //完成数量
            int CompletionCount = 0;
            for (int i = 0; i < GlobalManager.Instance._playerInfoList.Count; i++)
            {
                if (GlobalManager.Instance._playerInfoList[i].Id >= 0)
                {
                    if (GlobalManager.Instance._playerInfoList[i].CompleteLevel >= 30)
                    {
                        CompletionCount++;
                    }
                }
            }
            if (GlobalManager.Instance._RacingSelfPlayerInfo.CompleteLevel >= 30) //玩家完成比赛,比赛结束
            {
                GlobalManager.Instance.IsCompetition = false;
                PlayerPrefs.DeleteKey("StartDate");
                PlayerPrefs.DeleteKey("IsCompetition");
                Debug.Log("比赛结束，玩家完成竞赛");
                m_RewardRoot.SetActive(true);
                //光环顺时针一直旋转
                m_Halo.transform.DORotate(new Vector3(0, 0, -360), 5f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
                DOVirtual.DelayedCall(2f, () =>
                {
                    CreateAndAnimateCoins(rewardCoins[GlobalManager.Instance.GetPlayerRankIndex()]);
                });
            }
            else if (DateTime.Now - GlobalManager.Instance.RacingStartDate < TimeSpan.FromSeconds(TotalTime) && CompletionCount < 3)//比赛未结束
            {
                GlobalManager.Instance.RefreshPlayerInfoList();
                RefreshPlayerItem();
            }
            else if (DateTime.Now - GlobalManager.Instance.RacingStartDate > TimeSpan.FromSeconds(TotalTime) || CompletionCount >= 3)//比赛结束
            {
                //比赛结束
                GlobalManager.Instance.IsCompetition = false;
                PlayerPrefs.DeleteKey("StartDate");
                PlayerPrefs.DeleteKey("IsCompetition");
                m_GameDescText.text = "比赛结束，下次好运！";
            }
        }
    }
    public void OnStartCompetition()
    {
        if (mDailyCompetitionCount == 0)
        {
            return;
        }
        //设置开始时间
        GlobalManager.Instance.RacingStartDate = DateTime.Now;
        //保存开始时间
        PlayerPrefs.SetString("StartDate", GlobalManager.Instance.RacingStartDate.ToString());
        //设置开始关卡数
        GlobalManager.Instance.RacingStartLevel = GlobalManager.Instance.CurrentLevel;
        //保存开始关卡数
        PlayerPrefs.SetInt("StartLevel", GlobalManager.Instance.RacingStartLevel);
        GlobalManager.Instance.IsCompetition = true;
        //保存比赛状态
        PlayerPrefs.SetInt("IsCompetition", GlobalManager.Instance.IsCompetition ? 1 : 0);
        //设置参赛时间
        mCompetitionTime = DateTime.Now;
        //保存参赛时间
        PlayerPrefs.SetString("CompetitionTime", mCompetitionTime.ToString());
        mDailyCompetitionCount--;
        PlayerPrefs.SetInt("DailyCompetitionCount", mDailyCompetitionCount);
        m_CompetitionRoot.SetActive(true);
        m_StartCompetitionRoot.SetActive(false);
        GlobalManager.Instance.RefreshPlayerInfoList();
        RefreshPlayerItem();
    }
    void RefreshPlayerItem()
    {
        for (int i = 0; i < m_PlayerItems.Count; i++)
        {
            var playerInfo = GlobalManager.Instance._playerInfoList[i];
            m_PlayerItems[i].Init(playerInfo);
        }
    }
    public void OnRacingBtn()
    {
        CheckGameStatus();
    }
    //继续按钮
    public void OnContinueBtn()
    {
        m_CompetitionRoot.SetActive(false);
    }
    public void OnCloseStartCompetitionBtn()
    {
        m_StartCompetitionRoot.SetActive(false);
    }
    public void OnCloseCompetitionBtn()
    {
        m_CompetitionRoot.SetActive(false);
    }
    void Update()
    {
        if (GlobalManager.Instance.IsCompetition)
        {
            remainingTime -= Time.deltaTime;

            if (remainingTime <= 0)
            {
                remainingTime = 0;
            }
            else
            {
                //更新倒计时文本，以分钟为单位
                m_CountDownText.text = string.Format("{0:D2}:{1:D2}", (int)remainingTime / 60, (int)remainingTime % 60);
            }
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
        DOVirtual.DelayedCall(2f, () =>
       {
           m_RewardRoot.SetActive(false);
       });
    }

}
