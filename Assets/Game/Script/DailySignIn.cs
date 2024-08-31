using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using DG.Tweening;

public class DailySignIn : MonoBehaviour
{
    public Image[] dayImages; // 七天的Image数组
    public int[] rewardCoins; // 七天的奖励金币数组
    public Sprite signedInSprite; // 已领取的图片
    public Sprite notSignedInSprite; // 未领取的图片

    private bool[] signInStatus = new bool[7]; // 签到状态数组
    private DateTime lastSignInDate;
    public GameObject m_DailyRoot;
    public GameObject m_CoinContainer;
    public GameObject m_CoinPrefab;
    public GameObject m_TargetUI;
    float frameDuration = 0.05f;
    public GameObject m_RewradRoot;//奖励界面

    void Start()
    {
        LoadSignInData();
        UpdateSignInImages();
    }

    void LoadSignInData()
    {
        // 从PlayerPrefs加载签到数据
        for (int i = 0; i < 7; i++)
        {
            signInStatus[i] = PlayerPrefs.GetInt("SignInStatus" + i, 0) == 1;
        }

        // 加载上次签到日期
        string lastSignInDateString = PlayerPrefs.GetString("LastSignInDate", DateTime.MinValue.ToString());
        lastSignInDate = DateTime.Parse(lastSignInDateString);

        // 如果上次签到日期不是昨天，重置签到状态
        if ((DateTime.Now - lastSignInDate).Days > 1)
        {
            ResetSignInStatus();
        }
        if (GlobalManager.Instance.CurrentLevel > 7 && (DateTime.Now - lastSignInDate).Days >= 1)
        {
            m_DailyRoot.SetActive(true);
        }
    }

    void SaveSignInData()
    {
        // 保存签到数据到PlayerPrefs
        for (int i = 0; i < 7; i++)
        {
            PlayerPrefs.SetInt("SignInStatus" + i, signInStatus[i] ? 1 : 0);
        }

        // 保存上次签到日期
        PlayerPrefs.SetString("LastSignInDate", lastSignInDate.ToString());
        PlayerPrefs.Save();
    }

    void ResetSignInStatus()
    {
        for (int i = 0; i < 7; i++)
        {
            signInStatus[i] = false;
        }
        SaveSignInData();
    }

    public void SignIn()
    {
        int dayIndex = (int)(DateTime.Now - lastSignInDate).TotalDays % 7;
        if (lastSignInDate == DateTime.MinValue)
        {
            dayIndex = 0;
        }
        if (!signInStatus[dayIndex])
        {
            signInStatus[dayIndex] = true;
            lastSignInDate = DateTime.Now;
            SaveSignInData();
            UpdateSignInImages();
        }
        DOVirtual.DelayedCall(1f, () =>
        {
            m_DailyRoot.SetActive(false);
            m_RewradRoot.SetActive(true);
            CreateAndAnimateCoins(rewardCoins[dayIndex]);
        });
    }

    void UpdateSignInImages()
    {
        for (int i = 0; i < 7; i++)
        {
            dayImages[i].sprite = signInStatus[i] ? signedInSprite : notSignedInSprite;
            dayImages[i].transform.GetChild(3).gameObject.SetActive(signInStatus[i]);
            dayImages[i].transform.GetChild(2).GetComponent<TMP_Text>().text = "X" + rewardCoins[i].ToString();

        }
        int dayIndex = (int)(DateTime.Now - lastSignInDate).TotalDays % 7;
        if (lastSignInDate == DateTime.MinValue)
        {
            dayIndex = 0;
        }
        dayImages[dayIndex].sprite = signedInSprite;
    }
    public void CloseDailyRoot()
    {
        m_DailyRoot.SetActive(false);
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
            DOVirtual.DelayedCall(2f, () =>
               {
                   m_RewradRoot.SetActive(false);
               });
        }
    }
}