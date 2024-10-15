using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MM;
using SuperScrollView;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class RankingMatch : MonoBehaviour {
    // Start is called before the first frame update
    public ScrollRect m_ScrollRect;
    bool IsCreatList = false;

    public GameObject m_RankingMatchRoot;

    //奖杯图片
    public GameObject m_RankImage;

    //倒计时文本
    public Text m_CountDownText;

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
    public GameObject m_CoinPrefab; // 金币预制体
    public GameObject m_TargetUI; // 目标UI
    float frameDuration = 0.05f; // 每帧的持续时间

    public GameObject m_CoinContainer; // 金币容器

    //主界面
    public GameObject m_MainRoot;

    //段位界面
    public GameObject m_RankRoot;

    //描述界面
    public GameObject m_DescRoot;

    //汽车
    public GameObject m_Car;

    //段位物体列表
    public GameObject[] m_RankItems;

    //预制体1
    public GameObject m_Item1;

    //预制体2
    public GameObject m_Item2;

    //预制体3
    public GameObject m_Item3;
    float mItemHeight1;
    float mItemHeight2;

    float mItemHeight3;

    /////////////////////////////////////////////多语言设置，文本物体
    //标题文本


    //青铜段位文本
    public Text m_BronzeText;

    //白银段位文本
    public Text m_SilverText;

    //黄金段位文本
    public Text m_GoldText;

    //铂金段位文本
    public Text m_PlatinumText;

    //钻石段位文本
    public Text m_DiamondText;

    //王者段位文本
    public Text m_KingText;


    void Start() {
        mItemHeight1 = m_Item1.GetComponent<RectTransform>().sizeDelta.y;
        mItemHeight2 = m_Item2.GetComponent<RectTransform>().sizeDelta.y;
        mItemHeight3 = m_Item3.GetComponent<RectTransform>().sizeDelta.y;
        if (GlobalManager.Instance.CurrentLevel >= 14 && GlobalManager.Instance.mIsStartRankingMatch == false) {
            GlobalManager.Instance.mIsStartRankingMatch = true;
            GlobalManager.Instance.StartDate = DateTime.Now;
            //保存开始时间
            PlayerPrefs.SetString("RankingStartDate", GlobalManager.Instance.StartDate.ToString());
            PlayerPrefs.SetInt("IsStartRankingMatch", 1);
            RefreshRankingMatch();
        }
        else {
            RefreshRankingMatch();
        }

        SetLanguage();
    }

    public void SetLanguage() {
    }

    public void RefreshRankingMatch() {
        //GlobalManager.Instance.RefreshTrophyRankingList();
        //获取开始时间
        string startDate = PlayerPrefs.GetString("RankingStartDate");
        if (startDate != "") {
            GlobalManager.Instance.StartDate = Convert.ToDateTime(startDate);
            //计算剩余时间
            remainingTime = mCountDownTime - (DateTime.Now - GlobalManager.Instance.StartDate).TotalSeconds;
            if (remainingTime <= 0) {
                //如果排位赛时间到，主动弹出排位赛界面
                remainingTime = 0;
                m_RankingMatchRoot.SetActive(true);
                m_ContinueBtn.SetActive(false);
                m_Title.SetActive(false);
                m_GetBtn.SetActive(true);
                m_CountDownText.text = "已结束";
            }
            else {
                m_ContinueBtn.SetActive(true);
                m_GetBtn.SetActive(false);
                m_MainRoot.SetActive(true);
                m_RankRoot.SetActive(false);
                //更新倒计时文本，只显示天数和小时数
                int days = (int)(remainingTime / (24 * 60 * 60));
                int hours = (int)((remainingTime % (24 * 60 * 60)) / (60 * 60));
                m_CountDownText.text = days + GlobalManager.Instance.GetLanguageValue("Day") + hours + GlobalManager.Instance.GetLanguageValue("Hour");
            }

            m_RankImage.GetComponent<Image>().sprite = m_RankSprites[GlobalManager.Instance.CurrentRank - 1]; //设置段位图片
            m_RankImage.GetComponent<Image>().SetNativeSize();
            m_RankImage.transform.GetChild(0).GetComponent<Text>().text = GlobalManager.Instance.GetLanguageValue("Rank" + GlobalManager.Instance.CurrentRank.ToString());
            switch (GlobalManager.Instance.CurrentRank) {
                case 1:
                    m_RankImage.transform.GetChild(0).GetComponent<Text>().color = m_BronzeText.color;
                    break;
                case 2:
                    m_RankImage.transform.GetChild(0).GetComponent<Text>().color = m_SilverText.color;
                    break;
                case 3:
                    m_RankImage.transform.GetChild(0).GetComponent<Text>().color = m_GoldText.color;
                    break;
                case 4:
                    m_RankImage.transform.GetChild(0).GetComponent<Text>().color = m_PlatinumText.color;
                    break;
                case 5:
                    m_RankImage.transform.GetChild(0).GetComponent<Text>().color = m_DiamondText.color;
                    break;
                case 6:
                    m_RankImage.transform.GetChild(0).GetComponent<Text>().color = m_KingText.color;
                    break;
                default:
                    break;
            }

            GlobalManager.Instance.RefreshTrophyRankingList();
            //设置content的大小
            var y = (GlobalManager.Instance._trophyRankingList.Count - 2) * (mItemHeight1 + 15) + mItemHeight2 + mItemHeight3 + 30;
            m_ScrollRect.content.sizeDelta = new Vector2(0, y);
            //创建列表
            CreatItemList();
            var PlayerListIndex = GlobalManager.Instance.GetRankIndex();
            int currentPromotionNum = 0;
            if (PlayerListIndex > 29) {
                currentPromotionNum = PlayerListIndex - 29;
                if (remainingTime <= 0)
                    GlobalManager.Instance.CurrentRank--;
            }
            else {
                currentPromotionNum = 0;
                if (remainingTime <= 0)
                    GlobalManager.Instance.CurrentRank++;
            }

            //var str = GlobalManager.Instance.GetLanguageValue("PromotionDes");
            var str = LanguageManager.Instance.GetStringByCode("PromotionDes", currentPromotionNum + "");
            if (currentPromotionNum == 0) {
                str = LanguageManager.Instance.GetStringByCode("KeepRank");
            }

            //查找字符串xx 替换为当前晋升人数
            //str = str.Replace("                 ", "<size=150%><color=#7F68F0>" + currentPromotionNum.ToString() + "</color></size>");
            m_CurrentPromotionDesc.GetComponent<Text>().text = str;
        }
        else {
            GlobalManager.Instance.mIsStartRankingMatch = false;
            PlayerPrefs.SetInt("IsStartRankingMatch", 0);
        }
    }

    // 获取列表项
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
        //
        // IsCreatList = true;
        if (!IsCreatList) {
            InitItemList();
            IsCreatList = true;
        }


        var y = (GlobalManager.Instance._trophyRankingList.Count) * (mItemHeight1 + 15);


        for (int i = 0; i < GlobalManager.Instance._trophyRankingList.Count; i++) {
            Debug.Log(i);
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
    }

    private GameObject itemUP;
    private GameObject itemDown;

    private List<GameObject> itemArr = new();

    void InitItemList() {
        itemUP = Instantiate(m_Item2, m_ScrollRect.content);
        itemDown = Instantiate(m_Item3, m_ScrollRect.content);

        itemUP.SetActive(false);
        itemDown.SetActive(false);
        for (int i = 0; i < 99; i++) {
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

    // Update is called once per frame
    void Update() {
    }

    public void OnCloseBtn() {
        if (remainingTime <= 0) {
            GlobalManager.Instance.StartDate = DateTime.Now;
            //保存开始时间
            PlayerPrefs.SetString("RankingStartDate", GlobalManager.Instance.StartDate.ToString());
        }

        m_RankingMatchRoot.SetActive(false);
    }

    public void OnRankingMatchBtn() {
        m_RankingMatchRoot.SetActive(true);
        m_ScrollRect.content.anchoredPosition = new Vector2(0, 0);
        RefreshRankingMatch();
        SmoothScrollToIndex(GlobalManager.Instance.GetRankIndex() - 3, 1.5f);
    }

    private void SmoothScrollToIndex(int targetIndex, float duration) {
        if (targetIndex < 0 || targetIndex >= GlobalManager.Instance._trophyRankingList.Count) {
            return;
        }

        float targetY = 0;
        //计算目标位置
        if (targetIndex < 29) {
            targetY = targetIndex * (mItemHeight1 + 15);
        }
        else if (targetIndex > 29 && targetIndex < 59) {
            targetY = (targetIndex - 1) * (mItemHeight1 + 15) + mItemHeight2 + 15;
        }
        else {
            targetY = (targetIndex - 2) * (mItemHeight1 + 15) + mItemHeight2 + mItemHeight3 + 30;
        }

        m_ScrollRect.content.DOLocalMoveY(targetY, duration).SetEase(Ease.OutQuad);
    }

    //领取按钮
    public void OnGetBtn() {
        var propArray = GlobalManager.Instance._selfPlayerInfo.TrophyProp.PropArray;
        m_GoldReward.GetComponent<Image>().DOFade(0, 0.3f).OnComplete(() => {
            if (propArray.Length == 1) {
                GlobalManager.Instance.PlayerCoin += propArray[0].Count;
            }
            else if (propArray.Length == 2) {
                GlobalManager.Instance.PlayerCoin += propArray[0].Count;
                GlobalManager.Instance.ItemCount += propArray[1].Count;
            }

            CreateAndAnimateCoins(propArray[0].Count); // 假设金币数量为50
        });
        m_ItemReward.transform.DOScale(1.2f, 0.3f).SetEase(Ease.OutQuad).OnComplete(() => {
            m_ItemReward.transform.DOScale(0.3f, 0.6f).SetEase(Ease.OutQuad);
            m_ItemReward.GetComponent<Image>().DOFade(0, 0.6f);
        });
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

        DOVirtual.DelayedCall(2f, () => {
            m_SettlementRoot.SetActive(false);
            m_Title.SetActive(true);
            m_RankList.SetActive(true);
            m_DetailBtn.SetActive(true);
            ShowRankPage();
        });
    }

    //打开领取界面
    public void OnOpenGetPageBtn() {
        m_SettlementRoot.SetActive(true);
        m_RankList.SetActive(false);
        m_DetailBtn.SetActive(false);
        m_RankBtn.SetActive(false);
        m_GetBtn.SetActive(false);
        ShowRewardItem();
    }

    //获取奖励
    public void ShowRewardItem() {
        var _selfPlayerInfo = GlobalManager.Instance._selfPlayerInfo;
        var propArray = _selfPlayerInfo.TrophyProp.PropArray;
        if (propArray.Length == 1) //只有金币
        {
            m_GoldReward.SetActive(true);
            m_ItemReward.SetActive(false);
            m_GoldReward.transform.localPosition = new Vector3(0, m_GoldReward.transform.localPosition.y, 0);
            m_GoldReward.transform.GetChild(0).GetComponent<Text>().text = "X" + propArray[0].Count.ToString();
        }
        else if (propArray.Length == 2) //金币和道具
        {
            if (propArray[2].Type == Prop.Ballon) {
                m_GoldReward.SetActive(true);
                m_ItemReward.SetActive(true);
                m_GoldReward.transform.GetChild(0).GetComponent<Text>().text = "X" + propArray[0].Count.ToString();
                m_ItemReward.transform.GetChild(0).GetComponent<Text>().text = "X" + propArray[1].Count.ToString();
            }
            else {
                m_GoldReward.SetActive(true);
                m_ItemReward.SetActive(false);
                m_GoldReward.transform.localPosition = new Vector3(0, m_GoldReward.transform.localPosition.y, 0);
                m_GoldReward.transform.GetChild(0).GetComponent<Text>().text = "X" + propArray[0].Count.ToString();
            }
        }

        var PlayerListIndex = GlobalManager.Instance.GetRankIndex();
        var str1 = GlobalManager.Instance.GetLanguageValue("RankingSettlement");
        //查找字符串xx 替换为当前排名
        str1 = str1.Replace("xx", PlayerListIndex.ToString());
        m_CurrentRankDesc.GetComponent<Text>().text = str1; //当前排名
        var str2 = GlobalManager.Instance.GetLanguageValue("RankSettlement");
        //查找字符串xx 替换为当前段位
        str2 = str2.Replace("xx", GlobalManager.Instance.GetLanguageValue("Rank" + GlobalManager.Instance.CurrentRank.ToString()));
        m_CurrentRankDesc2.GetComponent<Text>().text = str2;
    }

    //显示排位赛详情
    public void ShowRankPage() {
        m_MainRoot.SetActive(false);
        m_RankRoot.SetActive(true);
        m_Car.transform.DOMoveY(m_RankItems[GlobalManager.Instance.CurrentRank - 1].transform.GetChild(0).transform.position.y, 0.5f).SetEase(Ease.OutQuad);
    }

    //显示描述界面
    public void ShowDescPage() {
        m_DescRoot.SetActive(true);
    }

    //关闭描述界面
    public void CloseDescPage() {
        m_DescRoot.SetActive(false);
    }

    //第一次打开处理逻辑
    public void FirstOpenRankingMatch() {
        m_RankingMatchRoot.SetActive(true);
        m_ScrollRect.content.anchoredPosition = new Vector2(0, 0);
        RefreshRankingMatch();
        SmoothScrollToIndex(GlobalManager.Instance.GetRankIndex() - 3, 1.5f);
        //继续、段位、详情按钮不可点击
        m_ContinueBtn.GetComponent<Button>().interactable = false;
        m_DetailBtn.GetComponent<Button>().interactable = false;
        m_RankBtn.GetComponent<Button>().interactable = false;
        //使用DOTween等待1秒
        DOVirtual.DelayedCall(1f, () => {
            //继续、段位、详情按钮可点击
            m_ContinueBtn.GetComponent<Button>().interactable = true;
            m_DetailBtn.GetComponent<Button>().interactable = true;
            m_RankBtn.GetComponent<Button>().interactable = true;
            //显示详情界面
            ShowDescPage();
        });
    }
}
