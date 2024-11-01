using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using MM;

public class UI : MonoBehaviour {
    // Start is called before the first frame update
    public Text ItemCountText; //道具数量

    //失败界面根节点
    public GameObject FailedRoot;

    //完成界面根节点
    public GameObject FinishRoot;

    //设置界面根节点
    public GameObject SettingRoot;

    //车辆图片和人物图片
    public List<Sprite> sprites;

    //车辆图片
    public Image m_FailedImage;

    //开关按钮切换图片
    public List<Sprite> switchSprites;

    //音乐开关按钮
    public Image m_SoundSwitch;

    //震动开关按钮
    public Image m_VibrateSwitch;

    //主关卡Root
    public GameObject MainLevelRoot;

    //困难关卡Root
    public GameObject HardLevelRoot;

    //开始关卡Root
    public GameObject StartLevelRoot;

    //游戏介绍Root
    public GameObject GameIntroduceRoot;

    //道具介绍Root
    public GameObject ItemIntroduceRoot;

    //引导手指图片序列
    public List<Sprite> m_GuideFingerSprites;

    //引导提示
    public GameObject m_GuideTip;

    //引导手指图片
    public Image m_GuideFinger;

    //道具按钮
    public Button m_ItemBtn;

    //右上角金币
    public GameObject m_TargetUI;

    // 金币图片的预制体
    public GameObject m_CoinPrefab;

    float frameDuration = 0.05f; // 每帧的持续时间

    //金币数量文本
    //汽车出界动画根节点
    public GameObject m_CarOutOfBoundsRoot;

    //顶部UI根节点
    public GameObject m_TopUIRoot;

    /////////////////////////////////////////////多语言设置，文本物体
    //步数
    public Text m_StepText;

    //游戏结束
    public Text m_GameOverText;

    //再来一次


    //花费金币
    public Text m_CostCoinText;


    //金币数量
    public Text m_CoinText;

    //设置标题


    //观看广告
    public Text m_WatchAdText;


    void Start() {
        GameManager.Instance.OnStepCountChanged += ChangeStepCount;
        ShowFailedRoot(false);
        m_SoundSwitch.sprite = GlobalManager.Instance.IsSound ? switchSprites[0] : switchSprites[1];
        m_VibrateSwitch.sprite = GlobalManager.Instance.IsVibrate ? switchSprites[0] : switchSprites[1];
        SetLanguage();
        CheckTopSafeArea();
        ChangeItemCount(GlobalManager.Instance.ItemCount);
        GlobalManager.Instance.OnItemCountChanged += ChangeItemCount;
    }

    public void OnDisable() {
        GlobalManager.Instance.OnItemCountChanged -= ChangeItemCount;
    }

    public void SetLanguage() {
        m_GameOverText.text = GlobalManager.Instance.GetLanguageValue("GameOver");
    }

    void OnDestroy() {
        // 取消注册方法
        GameManager.Instance.OnStepCountChanged -= ChangeStepCount;
    }

    // Update is called once per frame
    void Update() {
    }

    //点击重新开始按钮
    public void OnTouchReplay() {
        if (GlobalManager.Instance.CurrentLevel > 9)
            ApplovinSDKManager.Instance().interstitialAdsManager.ShowInterstitialAd(GlobalManager.Instance.GameType == GameType.Main ? "A0" : "C5", () => {
                GameManager.Instance.InitGame();
                ShowFailedRoot(false);
            });
        else {
            GameManager.Instance.InitGame();
            ShowFailedRoot(false);
        }
    }

    //道具使用按钮
    public void OnUseItemBtn() {
        if (GlobalManager.Instance.ItemCount > 0) {
            GameManager.Instance.IsUseItem = true;
            ShowItemIntroduce();
            HideGuideFinger();
            Debug.Log("使用道具");
        }
        else {
            if (GlobalManager.Instance.PlayerCoin >= 500) //使用金币购买
            {
                GameManager.Instance.IsUseItem = true;
                ShowItemIntroduce();
                HideGuideFinger();
                // GlobalManager.Instance.PlayerCoin -= 500;
            }
            else //看广告
            {
                ApplovinSDKManager.Instance().rewardAdsManager.ShowRewardedAd(GlobalManager.Instance.GameType == GameType.Main ? "A0" : "C5", () => {
                        GameManager.Instance.IsUseItem = true;
                        ShowItemIntroduce();
                        HideGuideFinger();
                    },
                    () => {
                        TipsManager.Instance.ShowTips(GlobalManager.Instance.GetLanguageValue("WatchAdFailed"));
                    });
            }
        }

        AudioManager.Instance.PlayButtonClick();
    }

    //更改道具数量
    public void ChangeItemCount(int count) {
        ItemCountText.text = count.ToString();

        if (count > 0) {
            m_ItemBtn.transform.GetChild(1).gameObject.SetActive(false);
            m_ItemBtn.transform.GetChild(2).gameObject.SetActive(false);
        }
        else {
            if (GlobalManager.Instance.PlayerCoin >= 500) {
                m_ItemBtn.transform.GetChild(1).gameObject.SetActive(true);
                m_ItemBtn.transform.GetChild(2).gameObject.SetActive(false);
            }
            else {
                m_ItemBtn.transform.GetChild(1).gameObject.SetActive(false);
                m_ItemBtn.transform.GetChild(2).gameObject.SetActive(true);
            }
        }

        // if (count <= 0 && GlobalManager.Instance.PlayerCoin >= 500) {
        //     m_ItemBtn.transform.GetChild(1).gameObject.SetActive(true);
        //     m_ItemBtn.transform.GetChild(2).gameObject.SetActive(false);
        // }
        // else if (count <= 0 && GlobalManager.Instance.PlayerCoin < 500) {
        //     m_ItemBtn.transform.GetChild(1).gameObject.SetActive(false);
        //     m_ItemBtn.transform.GetChild(2).gameObject.SetActive(true);
        // }
        // else {
        //     m_ItemBtn.transform.GetChild(1).gameObject.SetActive(false);
        //     m_ItemBtn.transform.GetChild(2).gameObject.SetActive(false);
        // }
    }

    //更新行动次数
    public void ChangeStepCount(int count) {
        if (GlobalManager.Instance.CurrentLevel < 10) {
            m_StepText.gameObject.SetActive(false);
            m_StepText.transform.parent.gameObject.SetActive(false);
            return;
        }
        else {
            m_StepText.gameObject.SetActive(true);
            m_StepText.transform.parent.gameObject.SetActive(true);
        }

        m_StepText.text = GlobalManager.Instance.GetLanguageValue("StepNumber") + ":" + count.ToString();
    }

    //更新关卡数
    public void ChangeLevelCount(GameType gameType) {
        if (gameType == GameType.Main) {
            if (GlobalManager.Instance.CurrentLevel < 10) {
                MainLevelRoot.SetActive(false);
                StartLevelRoot.SetActive(true);
                StartLevelRoot.transform.GetChild(0).GetComponent<Text>().text = "Lv " + (GlobalManager.Instance.CurrentLevel + 1).ToString();
            }
            else {
                MainLevelRoot.SetActive(true);
                StartLevelRoot.SetActive(false);
                HardLevelRoot.SetActive(false);
                // MainLevelRoot.transform.GetChild(0).GetComponent<Text>().text = "Lv " + (GlobalManager.Instance.CurrentLevel + 1).ToString();
                MainLevelRoot.transform.GetChild(0).GetComponent<Text>().text = LanguageManager.Instance.GetStringByCode("Level", "" + (GlobalManager.Instance.CurrentLevel + 1));
            }
        }
        else {
            MainLevelRoot.SetActive(false);
            HardLevelRoot.SetActive(true);
            HardLevelRoot.transform.GetChild(0).GetComponent<Text>().text = "Lv " + (GlobalManager.Instance.CurrentHardLevel + 1).ToString();
        }
    }

    //复活按钮
    public void OnTouchRevive() {
        //判断是否有足够的金币
        if (GameManager.Instance.failReason == FailReason.PeopleCrash) {
            if (GlobalManager.Instance.PlayerCoin >= 1000) {
                GlobalManager.Instance.PlayerCoin -= 1000;
                m_TargetUI.GetComponent<UICoin>().UpdateCoin();
            }
            else {
                TipsManager.Instance.ShowTips(GlobalManager.Instance.GetLanguageValue("CoinNotEnough"));
                return;
            }
        }
        else {
            if (GlobalManager.Instance.PlayerCoin >= 600) {
                GlobalManager.Instance.PlayerCoin -= 600;
                m_TargetUI.GetComponent<UICoin>().UpdateCoin();
            }
            else {
                TipsManager.Instance.ShowTips(GlobalManager.Instance.GetLanguageValue("CoinNotEnough"));
                return;
            }
        }

        GameManager.Instance.ContinueGame();
        AudioManager.Instance.PlayButtonClick();
    }

    //观看广告复活
    public void OnWatchAdRevive() {
        ApplovinSDKManager.Instance().rewardAdsManager.ShowRewardedAd(GlobalManager.Instance.GameType == GameType.Main ? "A0" : "C5", () => {
                GameManager.Instance.ContinueGame();
            },
            () => {
                TipsManager.Instance.ShowTips(GlobalManager.Instance.GetLanguageValue("WatchAdFailed"));
            });
        AudioManager.Instance.PlayButtonClick();
    }

    public void OnContinue() //胜利界面点击继续按钮
    {
        AudioManager.Instance.PlayButtonClick();
        m_GuideFinger.gameObject.SetActive(false);

        if (backToMenu()) {
            SceneManager.LoadScene("MenuScene");
        }
        else {
            GlobalManager.Instance.GameType = GameType.Main;
            GameManager.Instance.m_UIGameVictoryPage.HideLayer();
            GameManager.Instance.InitGame();
        }
    }

    public void OnTouchQuit() {
        SceneManager.LoadScene("MenuScene");
    }

    bool backToMenu() {
        if (GlobalManager.Instance.CurrentLevel == 6) {
            Debug.Log("比赛回home");
            return true;
        }

        if (GlobalManager.Instance.CurrentLevel == 8) {
            Debug.Log("转盘回home");
            return true;
        }

        if (GlobalManager.Instance.CurrentLevel == 17) {
            Debug.Log("皮肤回home");
            return true;
        }

        if (GlobalManager.Instance.IsReward) {
            Debug.Log("奖励关卡回home");
            return true;
        }

        if (isHardLevel()) {
            Debug.Log("困难关卡回home");
            return true;
        }

        if (GlobalManager.Instance.CurrentLevel == 15) {
            Debug.Log("联赛回home");
            return true;
        }

        if (GlobalManager.Instance.CurrentLevel == 19) {
            Debug.Log("双倍home");
            return true;
        }

        return false;
    }

    bool isHardLevel() {
        int level = GlobalManager.Instance.CurrentLevel;
        if (level >= 25) {
            return (level - 5) % 10 == 0;
        }
        else {
            return level == 16 || level == 20;
        }
    }

    //显示车撞行人失败界面
    public void ShowFailedRoot(bool isShow) {
        FailedRoot.SetActive(isShow);
        m_TargetUI.SetActive(isShow);
        var str = GlobalManager.Instance.GetLanguageValue("ConsumeCoins");
        if (GameManager.Instance.failReason == FailReason.PeopleCrash) {
            //查找str是否有xxx，如果有则替换
            if (str.Contains("xxx")) {
                str = str.Replace("xxx", "");
            }

            m_CostCoinText.text = GlobalManager.Instance.GetLanguageValue("Continue");
            m_CoinText.text = "1000";
            m_GameOverText.text = GlobalManager.Instance.GetLanguageValue("GameOver");
            m_FailedImage.sprite = sprites[1];
            m_FailedImage.SetNativeSize();
            m_FailedImage.transform.GetChild(0).gameObject.SetActive(false);
            m_WatchAdText.text = GlobalManager.Instance.GetLanguageValue("Continue");
        }
        else {
            //查找str是否有xxx，如果有则替换
            if (str.Contains("xxx")) {
                str = str.Replace("xxx", "");
            }

            m_CostCoinText.text = "+5 Steps";
            m_CoinText.text = "600";
            m_GameOverText.text = GlobalManager.Instance.GetLanguageValue("OutOfSteps");
            m_FailedImage.sprite = sprites[0];
            m_FailedImage.SetNativeSize();
            m_WatchAdText.text = "+5 Steps";
            m_FailedImage.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    //设置按钮
    public void OnSetting() {
        AudioManager.Instance.PlayButtonClick();
        SettingRoot.SetActive(SettingRoot.activeSelf ? false : true);
    }


    //震动开启关闭按钮
    public void OnClickVibrate() {
        GlobalManager.Instance.IsVibrate = !GlobalManager.Instance.IsVibrate;
        if (GlobalManager.Instance.IsVibrate) {
            m_VibrateSwitch.sprite = switchSprites[0];
        }
        else {
            m_VibrateSwitch.sprite = switchSprites[1];
        }
    }

    //音效开启关闭按钮
    public void OnClickSound() {
        GlobalManager.Instance.IsSound = !GlobalManager.Instance.IsSound;
        if (GlobalManager.Instance.IsSound) {
            AudioManager.Instance.ResumeBackgroundMusic();
            m_SoundSwitch.sprite = switchSprites[0];
        }
        else {
            AudioManager.Instance.StopBackgroundMusic();
            m_SoundSwitch.sprite = switchSprites[1];
        }
    }

    public void OnNextLevelBtn() {
        GlobalManager.Instance.CurrentLevel++;
        GameManager.Instance.InitGame();
    }

    public void OnLastLevelBtn() {
        GlobalManager.Instance.CurrentLevel--;
        GameManager.Instance.InitGame();
    }

    public Text IntroduceTitle;


    //显示游戏介绍
    public void ShowGameIntroduce(string gameIntroType) {
        GameIntroduceRoot.SetActive(true);
        if (gameIntroType == "Drone") {
            var drone = FindChildByName(GameIntroduceRoot.transform, "Drone");
            drone.gameObject.SetActive(true);
        }

        else if (gameIntroType == "Bulldozer") {
            //IntroduceTitle.text = LanguageManager.Instance.GetStringByCode("Bulldozer");

            var bulldozer = FindChildByName(GameIntroduceRoot.transform, "Bulldozer");
            bulldozer.gameObject.SetActive(true);
        }
        else if (gameIntroType == "People") {
            //IntroduceTitle.text = LanguageManager.Instance.GetStringByCode("Pedestrian");

            var people = FindChildByName(GameIntroduceRoot.transform, "People");
            people.gameObject.SetActive(true);
        }
        else {
            //IntroduceTitle.text = LanguageManager.Instance.GetStringByCode("TrafficLight");

            var trafficLight = FindChildByName(GameIntroduceRoot.transform, "TrafficLights");
            trafficLight.gameObject.SetActive(true);
        }
    }

    public void OnCloseGameIntroduceBtn() {
        var bulldozer = FindChildByName(GameIntroduceRoot.transform, "Bulldozer");
        bulldozer.gameObject.SetActive(false);
        var people = FindChildByName(GameIntroduceRoot.transform, "People");
        people.gameObject.SetActive(false);
        var trafficLight = FindChildByName(GameIntroduceRoot.transform, "TrafficLights");
        trafficLight.gameObject.SetActive(false);
        GameIntroduceRoot.SetActive(false);
    }

    public Transform FindChildByName(Transform parent, string name) {
        // 如果当前物体的名称匹配，返回当前物体
        if (parent.name == name) {
            return parent;
        }

        // 遍历所有子物体，包括未激活的物体
        foreach (Transform child in parent) {
            Transform result = FindChildByName(child, name);
            if (result != null) {
                return result;
            }
        }

        // 如果没有找到，返回 null
        return null;
    }

    //显示道具介绍
    public void ShowItemIntroduce() {
        ItemIntroduceRoot.SetActive(true);
        m_ItemBtn.gameObject.SetActive(false);
    }

    public void OnHideItemIntroduceBtn() {
        GameManager.Instance.IsUseItem = false;
        ItemIntroduceRoot.SetActive(false);
        m_ItemBtn.gameObject.SetActive(true);
    }

    //显示引导手指
    Sequence GuideSequence;

    public void ShowGuideFinger(Vector3 pos) {
        m_GuideFinger.transform.position = pos;
        m_GuideFinger.gameObject.SetActive(true);
        if (GuideSequence == null) {
            GuideSequence = DOTween.Sequence();
            // 序列帧动画
            foreach (var sprite in m_GuideFingerSprites) {
                GuideSequence.AppendCallback(() => m_GuideFinger.sprite = sprite);
                GuideSequence.AppendInterval(0.2f);
            }

            GuideSequence.SetLoops(-1, LoopType.Restart);
        }
    }

    //隐藏引导手指
    public void HideGuideFinger() {
        m_GuideFinger.gameObject.SetActive(false);
        HideGuideTip();
    }

    //显示引导提示
    public void ShowGuideTip(int level) {
        m_GuideTip.SetActive(true);
        if (level == 0)
            m_GuideTip.transform.GetChild(0).GetComponent<Text>().text = GlobalManager.Instance.GetLanguageValue("GuidanceTips1");
        else
            m_GuideTip.transform.GetChild(0).GetComponent<Text>().text = GlobalManager.Instance.GetLanguageValue("GuidanceTips2");
    }

    //隐藏引导提示
    public void HideGuideTip() {
        m_GuideTip.SetActive(false);
    }

    public void CreateCarOutOfBoundsAnim(Vector3 pos, Vector3 dir) {
        Sequence sequence = DOTween.Sequence();
        GameObject coin = Instantiate(m_CoinPrefab, m_CarOutOfBoundsRoot.transform);
        coin.transform.localScale = Vector3.one * 0.6f;
        coin.transform.position = Camera.main.WorldToScreenPoint(pos);
        foreach (var sprite in GlobalManager.Instance.m_CoinSprites) {
            sequence.AppendCallback(() => coin.GetComponent<Image>().sprite = sprite);
            sequence.AppendInterval(frameDuration);
        }

        sequence.SetLoops(-1, LoopType.Restart);
        //让金币以dir反方向飞出
        coin.transform.DOMove(Camera.main.WorldToScreenPoint(pos - dir * 5), 3f).SetEase(Ease.OutQuad).OnComplete(() => {
            coin.SetActive(false);
            Destroy(coin);
            sequence.Kill();
        });
        //渐隐
        coin.GetComponent<Image>().DOFade(0, 2f).SetEase(Ease.OutQuad);
    }

    //检测顶部安全区域
    public void CheckTopSafeArea() {
        Rect safeArea = Screen.safeArea;
        float height = Screen.height - safeArea.height;
        if (height > 0) {
            m_TopUIRoot.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, m_TopUIRoot.GetComponent<RectTransform>().anchoredPosition.y - safeArea.y);
            m_TargetUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(m_TargetUI.GetComponent<RectTransform>().anchoredPosition.x, m_TargetUI.GetComponent<RectTransform>().anchoredPosition.y - safeArea.y);
        }
    }


    public GameObject StepAniNodePrefab;

    public void PlayStepAni(Transform target) {
        var pos = target.TransformPoint(Vector3.zero);
        pos = transform.InverseTransformPoint(pos);
        var StepAniNode = Instantiate(StepAniNodePrefab, transform.parent);
        StepAniNode.transform.localPosition = pos;

        var mainCamera = Camera.main;


        // 将3D物体的世界位置转换为屏幕坐标
        Vector3 screenPoint = mainCamera.WorldToScreenPoint(target.position);
        Debug.Log(screenPoint);

        // 将屏幕坐标转换为Canvas的坐标
        Vector2 anchoredPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), screenPoint, null, out anchoredPosition);

        Debug.Log(anchoredPosition);
        // 更新UI元素位置
        StepAniNode.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;

        StepAniNode.GetComponent<Animation>().Play();

        DOVirtual.DelayedCall(2f, () => {
            Destroy(StepAniNode);
        }).Play();
    }
}
