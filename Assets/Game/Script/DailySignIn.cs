using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using DG.Tweening;
using MM;

public class DailySignIn : MonoBehaviour {
    public Image[] dayImages; // 七天的Image数组
    public int[] rewardCoins; // 七天的奖励金币数组
    public Sprite signedInSprite; // 已领取的图片
    public Sprite notSignedInSprite; // 未领取的图片

    //private bool[] signInStatus = new bool[7]; // 签到状态数组

    private int signCount = 0; // 已签到天数
    private DateTime lastSignInDate;
    public GameObject m_DailyRoot;
    public GameObject m_CoinContainer;
    public GameObject m_CoinPrefab;
    public GameObject m_TargetUI;
    float frameDuration = 0.05f;

    public GameObject m_RewradRoot; //奖励界面

    //初始签到时间
    // public DateTime m_StartDailySignTime;
    /////////////////////////////////////////////多语言设置，文本物体


    void Start() {
        LoadSignInData();
        UpdateSignInImages();
        SetLanguage();
    }

    public void SetLanguage() {
    }

    void LoadSignInData() {
        // 从PlayerPrefs加载签到数据
        // for (int i = 0; i < 7; i++)
        // {
        //     signInStatus[i] = PlayerPrefs.GetInt("SignInStatus" + i, 0) == 1;
        // }

        signCount = PlayerPrefs.GetInt("SignCount", 0);

        // 加载上次签到日期
        string lastSignInDateString = PlayerPrefs.GetString("LastSignInDate", DateTime.MinValue.ToString());
        lastSignInDate = DateTime.Parse(lastSignInDateString);
        //m_StartDailySignTime = DateTime.Parse(PlayerPrefs.GetString("StartDailySignTime", DateTime.MinValue.ToString()));
        // 如果上次签到日期不是昨天，重置签到状态
        if (signCount >= 7) {
            ResetSignInStatus();
        }

        if (GlobalManager.Instance.CurrentLevel > 7 && (DateTime.Now - lastSignInDate).Days >= 1) {
            m_DailyRoot.SetActive(true);
        }
    }

    void SaveSignInData() {
        // 保存签到数据到PlayerPrefs
        // for (int i = 0; i < 7; i++)
        // {
        //     PlayerPrefs.SetInt("SignInStatus" + i, signInStatus[i] ? 1 : 0);
        // }

        PlayerPrefs.SetInt("SignCount", signCount);

        // 保存上次签到日期
        PlayerPrefs.SetString("LastSignInDate", lastSignInDate.ToString());
        PlayerPrefs.Save();
    }

    void ResetSignInStatus() {
        // for (int i = 0; i < 7; i++)
        // {
        //     signInStatus[i] = false;
        // }
        signCount = 0;
        SaveSignInData();
        //m_StartDailySignTime = DateTime.Now;
        // PlayerPrefs.SetString("StartDailySignTime", m_StartDailySignTime.ToString());
    }


    public void SignIn() {
        int dayIndex = signCount;
        if (lastSignInDate == DateTime.MinValue) {
            dayIndex = 0;
        }


        if ((DateTime.Now - lastSignInDate).Days > 0) {
            // signInStatus[dayIndex] = true;
            signCount++;
            lastSignInDate = DateTime.Now;
            SaveSignInData();
            UpdateSignInImages();
        }

        DOVirtual.DelayedCall(1f, () => {
            m_DailyRoot.SetActive(false);
            m_RewradRoot.SetActive(true);
            GlobalManager.Instance.PlayerCoin += rewardCoins[dayIndex];
            CreateAndAnimateCoins(rewardCoins[dayIndex]);
        });
    }

    void UpdateSignInImages() {
        for (int i = 0; i < 7; i++) {
            dayImages[i].sprite = i > signCount ? signedInSprite : notSignedInSprite;
            dayImages[i].transform.GetChild(3).gameObject.SetActive(i < signCount);
            dayImages[i].transform.GetChild(2).GetComponent<Text>().text = "X" + rewardCoins[i].ToString();
            //dayImages[i].transform.GetChild(0).GetComponent<Text>().text = GlobalManager.Instance.GetLanguageValue("Day") + (i + 1).ToString();
            if (i < 5)
                dayImages[i].transform.GetChild(0).GetComponent<Text>().text = LanguageManager.Instance.GetStringByCode("Day" + (i + 1));
            else
                dayImages[i].transform.GetChild(0).GetComponent<Text>().text = LanguageManager.Instance.GetStringByCode("Seventh day");
        }

        int dayIndex = signCount;
        if (lastSignInDate == DateTime.MinValue) {
            dayIndex = 0;
        }

        dayImages[dayIndex].sprite = signedInSprite;
    }

    public void CloseDailyRoot() {
        m_DailyRoot.SetActive(false);
    }

    private void CreateAndAnimateCoins(int goldAmount) {
        AudioManager.Instance.PlayCoinSettle();
        int coinCount = Mathf.Min(goldAmount, 30);
        for (int i = 0; i < coinCount; i++) {
            GameObject coin = Instantiate(m_CoinPrefab, m_CoinContainer.transform);
            if (coin == null) {
                return;
            }

            coin.transform.localPosition = Vector3.zero; // 初始位置
            Sequence sequence = DOTween.Sequence();
            // 随机散开
            Vector3 randomPosition = new Vector3(UnityEngine.Random.Range(-200, 200), UnityEngine.Random.Range(-200, 200), 0);
            coin.transform.DOLocalMove(randomPosition, 0.5f).SetEase(Ease.OutQuad).OnComplete(() => {
                // 等待0.1秒
                DOVirtual.DelayedCall(0.3f, () => {
                    // 飞向目标UI
                    float randomDuration = UnityEngine.Random.Range(0.3f, 0.8f); // 随机飞行时间
                    coin.transform.DOMove(m_TargetUI.transform.position, randomDuration).SetEase(Ease.InQuad).OnComplete(() => {
                        m_TargetUI.GetComponent<UICoin>().UpdateCoin();
                        coin.SetActive(false);
                        sequence.Kill();
                    });
                });
            });
            // 序列帧动画
            foreach (var sprite in GlobalManager.Instance.m_CoinSprites) {
                sequence.AppendCallback(() => coin.GetComponent<Image>().sprite = sprite);
                sequence.AppendInterval(frameDuration);
            }

            sequence.SetLoops(-1, LoopType.Restart);
            DOVirtual.DelayedCall(2f, () => {
                m_RewradRoot.SetActive(false);
            });
        }
    }
}
