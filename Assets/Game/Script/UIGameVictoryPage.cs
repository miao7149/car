using System;
using System.Collections;
using System.Collections.Generic;
using AssetKits.ParticleImage;
using DG.Tweening;
using MM;
using SuperScrollView;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGameVictoryPage : MonoBehaviour {
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
    public Text m_FinishCoinText;

    //光环
    public GameObject m_Halo;

    /************************************************************************/
    //进阶完成界面子节点UI
    //金币
    public GameObject m_AdvanceCoin;

    //奖杯
    public GameObject m_AdvanceCup;

    public ScrollRect m_ScrollRect;

    //当前晋升描述
    public GameObject m_CurrentPromotionDesc;

    //当前晋升人数
    // public GameObject m_CurrentPromotionNum;

    //奖杯图片
    public GameObject m_RankImage;

    //倒计时文本
    public Text m_CountDownText;

    //七天倒计时（秒）
    private const float mCountDownTime = 7 * 24 * 60 * 60;

    private double remainingTime; // 剩余时间

    //段位图片
    public Sprite[] m_RankSprites;

    //预制体1
    public GameObject m_Item1;

    //预制体2
    public GameObject m_Item2;

    //预制体3
    public GameObject m_Item3;
    float mItemHeight1;
    float mItemHeight2;
    float mItemHeight3;

    /************************************************************************/
    //竞速赛完成界面子节点UI
    //玩家列表
    public List<GameObject> m_PlayerList;

    //玩家车辆图片
    public Sprite m_PlayerIcon;

    //其他玩家车辆图片
    public Sprite m_OtherPlayerIcon;

    void Start() {
        InitItemList();
        //初始化界面
        FinishRoot.SetActive(false);
        m_Beginner.SetActive(false);
        m_Advance.SetActive(false);
        m_Racing.SetActive(false);
        mItemHeight1 = m_Item1.GetComponent<RectTransform>().sizeDelta.y;
        mItemHeight2 = m_Item2.GetComponent<RectTransform>().sizeDelta.y;
        mItemHeight3 = m_Item3.GetComponent<RectTransform>().sizeDelta.y;
    }

    // Update is called once per frame
    void Update() {
    }

    //显示竞速赛完成界面
    public void ShowRacingFinishRoot() {
        FinishRoot.SetActive(true);
        m_Racing.SetActive(true);
        m_TargetUI.SetActive(true);
        GlobalManager.Instance.RefreshPlayerInfoList();
        var playerInfoList = GlobalManager.Instance._playerInfoList;
        for (int i = 0; i < m_PlayerList.Count; i++) {
            if (m_PlayerList[i] != null && playerInfoList[i] != null) {
                m_PlayerList[i].transform.GetChild(1).GetComponent<Text>().text = playerInfoList[i].Name;
                m_PlayerList[i].transform.GetChild(0).GetChild(1).GetComponent<Text>().text = playerInfoList[i].CompleteLevel.ToString() + "/30";
                if (playerInfoList[i].Id >= 0) {
                    m_PlayerList[i].GetComponent<Image>().sprite = m_OtherPlayerIcon;
                }
                else {
                    m_PlayerList[i].GetComponent<Image>().sprite = m_PlayerIcon;
                }
            }
        }

        m_Racing.transform.DOLocalMoveY(m_Racing.transform.localPosition.y + m_Racing.GetComponent<RectTransform>().sizeDelta.y, 0.5f);
    }

    public ParticleImage particleRank;

    //显示进阶完成界面
    public void ShowAdvanceFinishRoot(int coinCount = 0, int trophyCount = 0) {
        bool doubleScore = GlobalManager.Instance.IsDoubleReward;
        if (doubleScore) {
            trophyCount *= 2;
        }

        GlobalManager.Instance.RefreshTrophyRankingList();
        int oldRank = GlobalManager.Instance.GetRankIndex();
        CreatItemList();

        GlobalManager.Instance.PlayerCoin += coinCount;
        GlobalManager.Instance._selfPlayerInfo.TrophyProp.Trophy.Count += trophyCount;
        GlobalManager.Instance._selfTrophyCount += trophyCount;
        FinishRoot.SetActive(true);
        m_Advance.SetActive(true);
        m_TargetUI.SetActive(true);
        GlobalManager.Instance.RefreshTrophyRankingList();
        //设置content的大小
        // var y = (GlobalManager.Instance._trophyRankingList.Count) * (mItemHeight1 + 15) + mItemHeight2 + mItemHeight3 + 30;
        // m_ScrollRect.content.sizeDelta = new Vector2(0, y);
        //创建列表
        // CreatItemList();
        //string s = "There are                 people left to reach the promotion rank";
        string s;

        var PlayerListIndex = GlobalManager.Instance.GetRankIndex();
        if (PlayerListIndex > 29) {
            //m_CurrentPromotionNum.GetComponent<Text>().text = (PlayerListIndex - 29).ToString();
            s = LanguageManager.Instance.GetStringByCode("PromotionDes", "" + (PlayerListIndex - 29));
        }
        else {
            //m_CurrentPromotionNum.GetComponent<Text>().text = "0";
            s = LanguageManager.Instance.GetStringByCode("KeepRank");
        }

        m_RankImage.GetComponent<Image>().sprite = m_RankSprites[GlobalManager.Instance.CurrentRank - 1]; //设置段位图片
        m_RankImage.GetComponent<Image>().SetNativeSize();
        m_CurrentPromotionDesc.GetComponent<Text>().text = s;
        m_ScrollRect.enabled = false;

        SmoothScrollToIndex(oldRank - 3, 0);
        //获取开始时间
        string startDate = PlayerPrefs.GetString("RankingStartDate");
        if (startDate != "") {
            GlobalManager.Instance.StartDate = Convert.ToDateTime(startDate);
            //计算剩余时间
            remainingTime = mCountDownTime - (DateTime.Now - GlobalManager.Instance.StartDate).TotalSeconds;
            if (remainingTime <= 0) {
                m_CountDownText.text = "已结束";
            }
            else {
                //更新倒计时文本，只显示天数和小时数
                int days = (int)(remainingTime / (24 * 60 * 60));
                int hours = (int)((remainingTime % (24 * 60 * 60)) / (60 * 60));
                m_CountDownText.text = $"{days}天 {hours}小时";
            }

            if (coinCount > 0 && trophyCount > 0) {
                m_AdvanceCoin.transform.GetChild(0).GetComponent<Text>().text = "X" + coinCount.ToString();
                m_AdvanceCup.transform.GetChild(0).GetComponent<Text>().text = "X" + trophyCount;
                if (doubleScore) {
                    m_AdvanceCup.transform.GetChild(0).GetComponent<Text>().text = "X" + trophyCount / 2;
                    StartCoroutine(PlayDoubleAni(trophyCount, () => {
                        StartCoroutine(PlayRankAni(oldRank, PlayerListIndex));
                    }));
                }
                else {
                    StartCoroutine(PlayRankAni(oldRank, PlayerListIndex));
                }
            }
        }
    }

    IEnumerator PlayRankAni(int old, int current) {
        // Debug.Log("old" + old + "current" + current);

        itemArr[old].transform.DOScale(Vector3.one * 1.2f, 0.3f).Play();
        yield return new WaitForSeconds(0.3f);

        particleRank.attractorTarget = itemArr[old].GetComponent<RankingMatchItem>().m_RankIcon.transform;
        particleRank.Play();

        while (particleRank.isPlaying) {
            yield return null;
        }

        itemArr[old].transform.SetAsLastSibling();


        if (old == current) {
            itemArr[old].transform.DOScale(Vector3.one, 0.3f).Play();
            m_ScrollRect.enabled = true;
        }
        else {
            SmoothScrollToIndex(current - 3, 0.7f);
            itemArr[old].GetComponent<RectTransform>().DOAnchorPosY(itemArr[current].GetComponent<RectTransform>().anchoredPosition.y, 0.5f).Play();
            yield return new WaitForSeconds(0.7f);


            itemArr[old].GetComponent<RankingMatchItem>().Init(GlobalManager.Instance._trophyRankingList[current], current + 1);
            itemArr[old].transform.DOScale(Vector3.one, 0.3f).Play();

            if (old > current) {
                for (int i = current; i < old; i++) {
                    itemArr[i].GetComponent<RectTransform>().DOAnchorPosY(GetItemPos(i + 1).y, 0.3f).Play();
                    itemArr[i].GetComponent<RankingMatchItem>().Init(GlobalManager.Instance._trophyRankingList[i + 1], i + 2);
                }
            }

            if (old < current) {
                for (int i = current; i > old; i--) {
                    itemArr[i].GetComponent<RectTransform>().DOAnchorPosY(GetItemPos(i + 1).y, 0.3f).Play();
                    itemArr[i].GetComponent<RankingMatchItem>().Init(GlobalManager.Instance._trophyRankingList[i - 1], i);
                }
            }

            m_ScrollRect.enabled = true;
        }
    }

    public Animation doubleAni;

    IEnumerator PlayDoubleAni(int count, Action cb) {
        yield return new WaitForSeconds(0.5f);
        doubleAni.Play();
        yield return new WaitForSeconds(0.3f);

        // DOTween.To(x => x = count / 2,count/2, count, 1.5f).OnUpdate(() => {
        //     m_AdvanceCup.transform.GetChild(0).GetComponent<TMP_Text>().text = "X" +x;
        // }).Play();

        DOTween.To(() => count / 2, x => {
            //startValue = x; // 更新局部变量
            m_AdvanceCup.transform.GetChild(0).GetComponent<Text>().text = "X" + x; // 更新文本
        }, count, 0.5f).OnComplete(() => {
            cb?.Invoke();
        }).Play();
    }

    private void SmoothScrollToIndex(int targetIndex, float duration) {
        // if (targetIndex < 0 || targetIndex >= GlobalManager.Instance._trophyRankingList.Count) {
        //     return;
        // }

        float targetY = -GetItemPos(targetIndex).y;
        // //计算目标位置
        // if (targetIndex < 29) {
        //     targetY = targetIndex * (mItemHeight1 + 15);
        // }
        // else if (targetIndex > 29 && targetIndex < 59) {
        //     targetY = (targetIndex - 1) * (mItemHeight1 + 15) + mItemHeight2 + 15;
        // }
        // else {
        //     targetY = (targetIndex - 2) * (mItemHeight1 + 15) + mItemHeight2 + mItemHeight3 + 30;
        // }

        if (duration == 0) {
            m_ScrollRect.content.localPosition = new Vector3(m_ScrollRect.content.localPosition.x, targetY, 0);
        }
        else {
            m_ScrollRect.content.DOLocalMoveY(targetY, duration).Play();
        }
    }

    // 获取列表项
    // bool IsCreatList = false;

    void CreatItemList() {
        // float PosY = 0;
        // for (int i = 0; i < GlobalManager.Instance._trophyRankingList.Count; i++) {
        //     if (IsCreatList == false) {
        //         GameObject item = null;
        //         if (i == 29) {
        //             item = Instantiate(m_Item2, m_ScrollRect.content);
        //             item.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, PosY);
        //             PosY -= mItemHeight2 + 15;
        //         }
        //         else if (i == 59 && GlobalManager.Instance.CurrentRank > 1) {
        //             item = Instantiate(m_Item3, m_ScrollRect.content);
        //             item.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, PosY);
        //             PosY -= mItemHeight3 + 15;
        //         }
        //         else {
        //             item = Instantiate(m_Item1, m_ScrollRect.content);
        //             item.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, PosY);
        //             PosY -= mItemHeight1 + 15;
        //         }
        //
        //         item.GetComponent<RankingMatchItem>().Init(GlobalManager.Instance._trophyRankingList[i], i + 1);
        //     }
        //     else {
        //         m_ScrollRect.content.GetChild(i).GetComponent<RankingMatchItem>().Init(GlobalManager.Instance._trophyRankingList[i], i + 1);
        //     }
        // }

        var y = (GlobalManager.Instance._trophyRankingList.Count) * (mItemHeight1 + 15);


        for (int i = 0; i < itemArr.Count; i++) {
            itemArr[i].GetComponent<RankingMatchItem>().Init(GlobalManager.Instance._trophyRankingList[i], i + 1);
            SetItemPostion(itemArr[i], GetItemPos(i));
        }

        if (GlobalManager.Instance.CurrentRank < 6) {
            itemDown.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -mItemHeight2 - (60 * (mItemHeight1 + 15)));
        }
        else {
            itemDown.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -(60 * (mItemHeight1 + 15)));
        }

        itemUP.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -(30 * (mItemHeight1 + 15)));

        if (GlobalManager.Instance.CurrentRank < 6) {
            itemUP.SetActive(true);
            y += (mItemHeight2 + 15);
        }
        else {
            itemUP.SetActive(false);
        }

        if (GlobalManager.Instance.CurrentRank > 1) {
            itemDown.SetActive(true);
            y += (mItemHeight3 + 15);
        }
        else {
            itemDown.SetActive(false);
        }

        m_ScrollRect.content.sizeDelta = new Vector2(0, y);

        // IsCreatList = true;
    }

    private GameObject itemUP;
    private GameObject itemDown;

    private List<GameObject> itemArr = new();

    void InitItemList() {
        itemUP = Instantiate(m_Item2, m_ScrollRect.content);
        itemDown = Instantiate(m_Item3, m_ScrollRect.content);

        itemUP.SetActive(false);
        itemDown.SetActive(false);
        for (int i = 0; i < GlobalManager.Instance._trophyRankingList.Count; i++) {
            itemArr.Add(Instantiate(m_Item1, m_ScrollRect.content));
        }
    }


    void SetItemPostion(GameObject item, Vector2 p, float during = 0) {
        if (during == 0) {
            item.GetComponent<RectTransform>().anchoredPosition = p;
            return;
        }
        else {
            item.GetComponent<RectTransform>().DOAnchorPosY(p.y, during).Play();
        }
    }

    Vector2 GetItemPos(int index) {
        Vector2 result = new Vector2(0, -index * (mItemHeight1 + 15));
        if (index <= 29) {
        }
        else if (index <= 59) {
            if (GlobalManager.Instance.CurrentRank < 6) {
                result += new Vector2(0, -mItemHeight2);
            }
        }
        else {
            if (GlobalManager.Instance.CurrentRank < 6) {
                result += new Vector2(0, -mItemHeight2);
            }

            if (GlobalManager.Instance.CurrentRank > 1) {
                result += new Vector2(0, -mItemHeight3);
            }
        }

        return result;
    }

    //显示完成界面
    public void ShowBeginnerFinishRoot(int coinCount = 0) {
        FinishRoot.SetActive(true);
        m_TargetUI.SetActive(true);
        m_Beginner.SetActive(true);
        if (coinCount > 0) {
            m_FinishCoinText.text = "X" + coinCount.ToString();
            m_Halo.SetActive(true);
            //顺时针一直旋转
            m_Halo.transform.DORotate(new Vector3(0, 0, -360), 5f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
            m_FinishCoinText.transform.parent.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).OnComplete(() => {
                GlobalManager.Instance.PlayerCoin += coinCount;
                CreateAndAnimateCoins(coinCount);
            });
        }
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
        }
    }
}
