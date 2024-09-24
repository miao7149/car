using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WheelOfFortune : MonoBehaviour
{
    public Image wheelImage; // 转盘的 Image 组件
    public float spinDuration = 5f; // 旋转持续时间
    public int numberOfSectors = 8; // 扇区数量

    private float anglePerSector; // 每个扇区的角度
    private bool isSpinning = false;
    public PropData[] propDatas = new PropData[]
    {
        new() { Type = Prop.Coin, Count = 1000 },
        new() { Type = Prop.Ballon, Count = 5 },
        new() { Type = Prop.Coin, Count = 150 },
        new() { Type = Prop.Ballon, Count = 1 },
        new() { Type = Prop.Coin, Count = 50 },
        new() { Type = Prop.Coin, Count = 400 },
    };//奖品数据
    public GameObject[] propItems;//奖品物体
    float frameDuration = 0.05f; // 每帧的持续时间
    public GameObject m_CoinPrefab; // 金币预制体
    public GameObject m_CoinContainer; // 金币容器
    public GameObject m_TargetUI; // 目标UI
    public GameObject m_RewardRoot; // 奖励根节点
    public GameObject m_Halo; // 光晕
    public Image m_RewardImage; // 奖励图片
    public Sprite[] m_RewardSprites; // 奖励图片
    //是否首次抽奖
    private bool IsFirstSpinning = true;
    //今日可抽奖次数
    private int m_TodaySpinCount = 5;
    //抽奖完成时间
    private string m_SpinFinishTime;
    //第一次抽奖节点
    public GameObject m_FirstSpinRoot;
    //广告抽奖节点
    public GameObject m_AdSpinRoot;
    //次数用尽节点
    public GameObject m_NoSpinRoot;
    //抽奖按钮图片
    public Sprite[] m_SpinButtonSprites;
    //抽奖按钮
    public Button m_SpinButton;
    //转盘根节点
    public GameObject m_WheelRoot;
    //每日上线后提示玩家抽奖的委托事件
    public delegate void OnDailySpin(string message);
    public OnDailySpin OnDailySpinEvent;
    //关闭红点委托
    public delegate void CloseRedPoint(string message);
    public CloseRedPoint CloseRedPointEvent;

    void Start()
    {
        anglePerSector = 360f / numberOfSectors;
        //初始化奖品
        for (int i = 0; i < propItems.Length; i++)
        {
            propItems[i].transform.GetChild(0).GetComponent<TMP_Text>().text = "X" + propDatas[i].Count.ToString();
        }
        //读取是否首次抽奖
        IsFirstSpinning = PlayerPrefs.GetInt("IsFirstSpinning", 1) == 1;
        //读取今日抽奖次数
        m_TodaySpinCount = PlayerPrefs.GetInt("TodaySpinCount", 5);
        //读取抽奖完成时间
        m_SpinFinishTime = PlayerPrefs.GetString("SpinFinishTime", "");
        if (m_SpinFinishTime != "")
        {
            System.DateTime finishTime = System.DateTime.Parse(m_SpinFinishTime);
            System.DateTime nowTime = System.DateTime.Now;
            if (nowTime.DayOfYear != finishTime.DayOfYear)//如果不是同一天
            {
                OnDailySpinEvent?.Invoke("Spin");
                m_TodaySpinCount = 5;
            }
        }
        else
        {
            OnDailySpinEvent?.Invoke("Spin");
        }
        SetSpinButtonState();//设置抽奖按钮状态
    }
    void Update()
    {
        //如果没有抽奖次数，获取m_NoSpinRoot子节点下的文字，显示当前时间距离0点的倒计时
        if (m_NoSpinRoot.activeSelf)
        {
            System.DateTime nowTime = System.DateTime.Now;
            System.DateTime nextDay = nowTime.AddDays(1).Date;
            System.TimeSpan timeSpan = nextDay - nowTime;
            m_NoSpinRoot.transform.GetChild(0).GetComponent<TMP_Text>().text = timeSpan.Hours.ToString("00") + "h" + timeSpan.Minutes.ToString("00") + "m";
        }

    }
    public void StartSpin()
    {
        if (!isSpinning)
        {
            StartCoroutine(SpinWheel());
        }
    }

    private IEnumerator SpinWheel()
    {
        isSpinning = true;
        float targetAngle = 0;
        // 随机选择一个目标角度，增加某些区域的权重
        if (IsFirstSpinning)
            targetAngle = GetWeightedRandomAngle(0);//首次抽奖固定第一个扇区
        else
            targetAngle = GetWeightedRandomAngle();
        float totalAngle = 360f - targetAngle + 360 * 3; // 旋转多圈
        int selectedSector = 0;
        wheelImage.rectTransform.DORotate(new Vector3(0, 0, -totalAngle), spinDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.OutCubic)
            .OnComplete(() =>
            {
                // 确保最终停在目标角度
                wheelImage.rectTransform.rotation = Quaternion.Euler(0, 0, targetAngle);

                // 计算中奖扇区
                selectedSector = Mathf.FloorToInt((targetAngle + 30) / anglePerSector);
                Debug.Log("Selected Sector: " + (selectedSector + 1));
                isSpinning = false;
            });

        yield return new WaitForSeconds(spinDuration);
        m_RewardRoot.SetActive(true);
        if (propDatas[selectedSector].Type == Prop.Coin)
        {
            m_RewardImage.sprite = m_RewardSprites[0];
            GlobalManager.Instance.PlayerCoin += propDatas[selectedSector].Count;
        }
        else
        {
            m_RewardImage.sprite = m_RewardSprites[1];
            GlobalManager.Instance.ItemCount += propDatas[selectedSector].Count;
        }
        m_RewardImage.transform.localScale = Vector3.one;
        m_RewardImage.SetNativeSize();
        m_RewardImage.transform.GetChild(0).GetComponent<TMP_Text>().text = "X" + propDatas[selectedSector].Count.ToString();
        //光环顺时针一直旋转
        m_Halo.transform.DORotate(new Vector3(0, 0, -360), 5f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
        yield return new WaitForSeconds(1f);
        if (propDatas[selectedSector].Type == Prop.Coin)
            CreateAndAnimateCoins(propDatas[selectedSector].Count);
        else
        {
            m_RewardImage.transform.DOScale(1.2f, 0.3f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                m_RewardImage.transform.DOScale(0.3f, 0.6f).SetEase(Ease.OutQuad).OnComplete(() =>
                {
                    m_RewardRoot.SetActive(false);
                });
            });
        }

    }

    // 获取带权重的随机角度
    private float GetWeightedRandomAngle()
    {
        // 定义每个扇区的权重
        float[] sectorWeights = new float[numberOfSectors];
        for (int i = 0; i < numberOfSectors; i++)
        {
            sectorWeights[i] = 1f; // 默认权重为1
        }

        // 增加特定扇区的权重，例如第一个扇区
        sectorWeights[0] = 1; // 增加第一个扇区的权重

        // 计算总权重
        float totalWeight = 0f;
        foreach (float weight in sectorWeights)
        {
            totalWeight += weight;
        }

        // 随机选择一个加权扇区
        float randomValue = UnityEngine.Random.Range(0, totalWeight);
        float cumulativeWeight = 0f;
        int selectedSector = 0;
        for (int i = 0; i < numberOfSectors; i++)
        {
            cumulativeWeight += sectorWeights[i];
            if (randomValue < cumulativeWeight)
            {
                selectedSector = i;
                break;
            }
        }

        // 返回对应扇区的随机角度
        float sectorStartAngle = selectedSector * anglePerSector - 30f;
        float sectorEndAngle = sectorStartAngle + anglePerSector;
        return sectorEndAngle - (sectorEndAngle - sectorStartAngle) / 2;
        //return UnityEngine.Random.Range(sectorStartAngle, sectorEndAngle);
    }
    //固定扇区的角度
    private float GetWeightedRandomAngle(int index)
    {
        int selectedSector = index;
        float sectorStartAngle = selectedSector * anglePerSector - 30f;
        float sectorEndAngle = sectorStartAngle + anglePerSector;
        return sectorEndAngle - (sectorEndAngle - sectorStartAngle) / 2;
    }
    // 点击按钮
    public void OnClickSpin()
    {
        if (isSpinning)
        {
            return;
        }
        if (IsFirstSpinning)//如果是第一次抽奖
        {
            StartSpin();
            IsFirstSpinning = false;
            PlayerPrefs.SetInt("IsFirstSpinning", 0);
        }
        else if (m_TodaySpinCount > 0)//如果还有抽奖次数
        {
            m_TodaySpinCount--;
            PlayerPrefs.SetInt("TodaySpinCount", m_TodaySpinCount);
            StartSpin();
        }
        else
        {
            System.DateTime finishTime = System.DateTime.Now;
            PlayerPrefs.SetString("SpinFinishTime", finishTime.ToString());
        }
        SetSpinButtonState();
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
    public void OnClose()
    {
        m_WheelRoot.SetActive(false);
    }
    //设置抽奖按钮状态
    public void SetSpinButtonState()
    {
        if (IsFirstSpinning)//如果是第一次抽奖
        {
            m_FirstSpinRoot.SetActive(true);
            m_AdSpinRoot.SetActive(false);
            m_NoSpinRoot.SetActive(false);
            m_SpinButton.image.sprite = m_SpinButtonSprites[0];
        }
        else if (m_TodaySpinCount > 0)//如果还有抽奖次数
        {
            m_FirstSpinRoot.SetActive(false);
            m_AdSpinRoot.SetActive(true);
            m_AdSpinRoot.transform.GetChild(2).GetComponent<TMP_Text>().text = m_TodaySpinCount.ToString() + "/5";
            m_NoSpinRoot.SetActive(false);
            m_SpinButton.image.sprite = m_SpinButtonSprites[0];
        }
        else
        {
            m_FirstSpinRoot.SetActive(false);
            m_AdSpinRoot.SetActive(false);
            m_NoSpinRoot.SetActive(true);
            m_SpinButton.image.sprite = m_SpinButtonSprites[1];
            CloseRedPointEvent?.Invoke("Spin");
        }
    }
}