using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class UIReward : MonoBehaviour {
    // Start is called before the first frame update
    public GameObject m_RewardRoot;
    public GameObject m_RewardRootRV;
    public SkeletonGraphic skeletonGraphic; //礼盒
    public SkeletonGraphic skeletonGraphicRV; //礼盒
    public GameObject coinContainer; // 存放金币图片的容器
    public GameObject coinContainerRV; // 存放金币图片的容器


    public Text t;
    public Text tRv;
    public GameObject m_ItemRoot; //道具根节点
    public float frameDuration = 0.05f; // 每帧的持续时间
    public GameObject coinPrefab; // 金币图片的预制体
    const int mGoldMax = 50; //金币数量上限
    public Transform targetUI; // 目标金币UI的位置
    private Queue<GameObject> coinPool = new Queue<GameObject>();
    public int initialPoolSize = 20;
    public PropData[] Rewards = new PropData[] { new(Prop.Coin, 50) }; //第一个奖励

    public PropData[] RewardsRv = new PropData[] { new(Prop.Coin, 355), new(Prop.Ballon, 1) }; //第二个奖励

    //是否首次领取第二次奖励
    public bool mIsFirstGetRewardRV = true;

    public GameObject adFlag;

    void Start() {
        // if (skeletonGraphic != null) {
        //     // 订阅动画完成事件
        //     skeletonGraphic.AnimationState.Complete += OnAnimationComplete;
        //     skeletonGraphic.AnimationState.Event += OnAnimationEvent;
        //     skeletonGraphicRV.AnimationState.Complete += OnAnimationComplete;
        //     skeletonGraphicRV.AnimationState.Event += OnAnimationEvent;
        // }
        //
        // skeletonGraphic.AnimationState.SetAnimation(0, "daiji", true);
        // skeletonGraphicRV.AnimationState.SetAnimation(0, "daiji", true);
        mIsFirstGetRewardRV = PlayerPrefs.GetInt("IsFirstGetRewardRV", 0) == 0;
        adFlag.SetActive(!mIsFirstGetRewardRV);
    }

    private void OnDestroy() {
        // // 取消订阅动画完成事件
        // if (skeletonGraphic != null) {
        //     skeletonGraphic.AnimationState.Complete -= OnAnimationComplete;
        //     skeletonGraphic.AnimationState.Event -= OnAnimationEvent;
        //     skeletonGraphicRV.AnimationState.Complete -= OnAnimationComplete;
        //     skeletonGraphicRV.AnimationState.Event -= OnAnimationEvent;
        // }
    }

    private GameObject GetCoin(Transform prant) {
        if (coinPool.Count > 0) {
            GameObject coin = coinPool.Dequeue();
            coin.SetActive(true);
            coin.transform.SetParent(prant);
            return coin;
        }
        else {
            GameObject coin = Instantiate(coinPrefab, prant);
            return coin;
        }
    }

    private void ReturnCoin(GameObject coin) {
        // 重置金币位置
        coin.transform.localPosition = Vector3.zero;
        // 重置金币图片
        coin.GetComponent<Image>().sprite = GlobalManager.Instance.m_CoinSprites[0];
        //金币dotween动画重置
        coin.transform.DOKill();
        coin.SetActive(false);
        coinPool.Enqueue(coin);
    }

    private void OnAnimationComplete(Spine.TrackEntry trackEntry) {
    }

    private void OnAnimationEvent(Spine.TrackEntry trackEntry, Spine.Event e) {
        Debug.Log("动画事件：" + e.Data.Name);
        // 检查动画进度
        float animationProgress = trackEntry.AnimationTime / trackEntry.AnimationEnd;
        if (animationProgress >= 0.4f) {
            if (m_RewardRoot.activeSelf) {
                skeletonGraphic.DOFade(0, 0.5f).OnComplete(() => {
                    //skeletonGraphic.gameObject.SetActive(false);
                    coinContainer.transform.DOLocalMove(new Vector3(0, coinContainer.transform.localPosition.y + 120, 0), 0.3f).SetEase(Ease.OutQuad);
                    t.DOFade(1, 0.5f);
                    coinContainer.GetComponent<Image>().DOFade(1, 0.5f).OnComplete(() => {
                        t.DOFade(0, 0.3f);
                        coinContainer.GetComponent<Image>().DOFade(0, 0.3f).OnComplete(() => {
                            GlobalManager.Instance.PlayerCoin += Rewards[0].Count;
                            CreateAndAnimateCoins(Rewards[0].Count); // 假设金币数量为50
                        });
                    });
                });
            }
            else {
                skeletonGraphicRV.DOFade(0, 0.5f).OnComplete(() => {
                    //skeletonGraphicRV.gameObject.SetActive(false);
                    coinContainerRV.transform.DOLocalMove(new Vector3(coinContainerRV.transform.localPosition.x - 120, coinContainerRV.transform.localPosition.y + 120, 0), 0.3f).SetEase(Ease.OutQuad);
                    m_ItemRoot.transform.DOLocalMove(new Vector3(coinContainerRV.transform.localPosition.x + 120, coinContainerRV.transform.localPosition.y + 120, 0), 0.3f).SetEase(Ease.OutQuad);
                    tRv.DOFade(1, 0.5f);
                    coinContainerRV.GetComponent<Image>().DOFade(1, 0.5f).OnComplete(() => {
                        tRv.DOFade(0, 0.3f);
                        coinContainerRV.GetComponent<Image>().DOFade(0, 0.3f).OnComplete(() => {
                            GlobalManager.Instance.PlayerCoin += RewardsRv[0].Count;
                            GlobalManager.Instance.ItemCount += RewardsRv[1].Count;
                            CreateAndAnimateCoins(RewardsRv[0].Count); // 假设金币数量为50
                        });
                    });
                    m_ItemRoot.GetComponent<Image>().DOFade(1, 0.5f).OnComplete(() => {
                        m_ItemRoot.transform.DOScale(1.2f, 0.3f).SetEase(Ease.OutQuad).OnComplete(() => {
                            m_ItemRoot.transform.DOScale(0.3f, 0.6f).SetEase(Ease.OutQuad);
                            m_ItemRoot.GetComponent<Image>().DOFade(0, 0.6f);
                        });
                    });
                });
            }
        }
    }

    private void CreateAndAnimateCoins(int goldAmount) {
        AudioManager.Instance.PlayCoinSettle();
        int coinCount = Mathf.Min(goldAmount / 2, mGoldMax);
        for (int i = 0; i < coinCount; i++) {
            GameObject coin = null;
            if (m_RewardRoot.activeSelf) {
                coin = GetCoin(coinContainer.transform);
            }
            else {
                coin = GetCoin(coinContainerRV.transform);
            }

            if (coin == null) {
                return;
            }

            coin.transform.localPosition = Vector3.zero; // 初始位置
            Sequence sequence = DOTween.Sequence();
            // 随机散开
            Vector3 randomPosition = new Vector3(Random.Range(-200, 200), Random.Range(-200, 200), 0);
            coin.transform.DOLocalMove(randomPosition, 0.5f).SetEase(Ease.OutQuad).OnComplete(() => {
                // 等待0.1秒
                DOVirtual.DelayedCall(0.3f, () => {
                    // 飞向目标UI
                    float randomDuration = Random.Range(0.3f, 0.8f); // 随机飞行时间
                    coin.transform.DOMove(targetUI.position, randomDuration).SetEase(Ease.InQuad).OnComplete(() => {
                        targetUI.GetComponent<UICoin>().UpdateCoin();
                        // 将金币返回对象池
                        ReturnCoin(coin);
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
            if (m_RewardRoot.activeSelf) {
                HideReward();
                ShowRewardRV();
            }
            else {
                HideRewardRV();
                GlobalManager.Instance.IsReward = false;
            }
        });
    }

    // Update is called once per frame
    void Update() {
    }

    public void ShowReward() {
        cantouch = true;
        m_RewardRoot.SetActive(true);
        skeletonGraphic.AnimationState.SetAnimation(0, "idle1", false);
        DOVirtual.DelayedCall(1.34f, () => {
            skeletonGraphic.AnimationState.SetAnimation(0, "idle2", true);
        });
    }

    public void HideReward() {
        m_RewardRoot.SetActive(false);
    }

    public void ShowRewardRV() {
        cantouch = true;
        m_RewardRootRV.SetActive(true);
        skeletonGraphicRV.AnimationState.SetAnimation(0, "idle1", false);
        DOVirtual.DelayedCall(1.34f, () => {
            skeletonGraphicRV.AnimationState.SetAnimation(0, "idle2", true);
        });
    }

    public void HideRewardRV() {
        m_RewardRootRV.SetActive(false);
    }

    private bool cantouch = true;

    //点击领取奖励
    public void OnOpenGiftClick() {
        if (!cantouch) return;
        cantouch = false;
        if (skeletonGraphic == null)
            return;
        skeletonGraphic.AnimationState.SetAnimation(0, "open", false);

        DOVirtual.DelayedCall(1.34f, () => {
            coinContainer.transform.DOLocalMove(new Vector3(0, coinContainer.transform.localPosition.y + 120, 0), 0.3f).SetEase(Ease.OutQuad);
            t.DOFade(1, 0.5f);
            coinContainer.GetComponent<Image>().DOFade(1, 0.5f).OnComplete(() => {
                t.DOFade(0, 0.3f);
                coinContainer.GetComponent<Image>().DOFade(0, 0.3f).OnComplete(() => {
                    GlobalManager.Instance.PlayerCoin += Rewards[0].Count;
                    CreateAndAnimateCoins(Rewards[0].Count); // 假设金币数量为50
                });
            });
        });
    }

    public void OnOpenGiftClickRV() {
        if (!cantouch) return;
        cantouch = false;
        if (skeletonGraphicRV == null)
            return;
        if (mIsFirstGetRewardRV) //首次领取第二次奖励
        {
            skeletonGraphicRV.AnimationState.SetAnimation(0, "open", false);
            DOVirtual.DelayedCall(1.34f, () => {
                coinContainerRV.transform.DOLocalMove(new Vector3(coinContainerRV.transform.localPosition.x - 120, coinContainerRV.transform.localPosition.y + 120, 0), 0.3f).SetEase(Ease.OutQuad);
                m_ItemRoot.transform.DOLocalMove(new Vector3(coinContainerRV.transform.localPosition.x + 120, coinContainerRV.transform.localPosition.y + 120, 0), 0.3f).SetEase(Ease.OutQuad);
                tRv.DOFade(1, 0.5f);
                coinContainerRV.GetComponent<Image>().DOFade(1, 0.5f).OnComplete(() => {
                    tRv.DOFade(0, 0.3f);
                    coinContainerRV.GetComponent<Image>().DOFade(0, 0.3f).OnComplete(() => {
                        GlobalManager.Instance.PlayerCoin += RewardsRv[0].Count;
                        GlobalManager.Instance.ItemCount += RewardsRv[1].Count;
                        CreateAndAnimateCoins(RewardsRv[0].Count); // 假设金币数量为50
                    });
                });
                m_ItemRoot.GetComponent<Image>().DOFade(1, 0.5f).OnComplete(() => {
                    m_ItemRoot.transform.DOScale(1.2f, 0.3f).SetEase(Ease.OutQuad).OnComplete(() => {
                        m_ItemRoot.transform.DOScale(0.3f, 0.6f).SetEase(Ease.OutQuad);
                        m_ItemRoot.GetComponent<Image>().DOFade(0, 0.6f);
                    });
                });
            });
            PlayerPrefs.SetInt("IsFirstGetRewardRV", 1);
            mIsFirstGetRewardRV = false;
        }
        else {
            ApplovinSDKManager.Instance().rewardAdsManager.ShowRewardedAd(() => {
                skeletonGraphicRV.AnimationState.SetAnimation(0, "open", false);
                DOVirtual.DelayedCall(1.34f, () => {
                    coinContainerRV.transform.DOLocalMove(new Vector3(coinContainerRV.transform.localPosition.x - 120, coinContainerRV.transform.localPosition.y + 120, 0), 0.3f).SetEase(Ease.OutQuad);
                    m_ItemRoot.transform.DOLocalMove(new Vector3(coinContainerRV.transform.localPosition.x + 120, coinContainerRV.transform.localPosition.y + 120, 0), 0.3f).SetEase(Ease.OutQuad);

                    tRv.DOFade(1, 0.5f);
                    coinContainerRV.GetComponent<Image>().DOFade(1, 0.5f).OnComplete(() => {
                        tRv.DOFade(0, 0.3f);
                        coinContainerRV.GetComponent<Image>().DOFade(0, 0.3f).OnComplete(() => {
                            GlobalManager.Instance.PlayerCoin += RewardsRv[0].Count;
                            GlobalManager.Instance.ItemCount += RewardsRv[1].Count;
                            CreateAndAnimateCoins(RewardsRv[0].Count); // 假设金币数量为50
                        });
                    });

                    m_ItemRoot.GetComponent<Image>().DOFade(1, 0.5f).OnComplete(() => {
                        m_ItemRoot.transform.DOScale(1.2f, 0.3f).SetEase(Ease.OutQuad).OnComplete(() => {
                            m_ItemRoot.transform.DOScale(0.3f, 0.6f).SetEase(Ease.OutQuad);
                            m_ItemRoot.GetComponent<Image>().DOFade(0, 0.6f);
                        });
                    });
                });
            }, () => {
                TipsManager.Instance.ShowTips(GlobalManager.Instance.GetLanguageValue("AdNotReady"));
            });
        }
    }

    //放弃按钮
    public void OnGiveUpClick() {
        HideRewardRV();
    }
}
