using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    //汽车物体
    public GameObject m_Car;
    public List<GameObject> m_LevelList;
    //设置界面
    public GameObject m_SettingPanel;
    //相机
    public Camera m_Camera;
    //转盘界面
    public GameObject m_WheelPanel;
    //开关按钮切换图片
    public List<Sprite> switchSprites;
    //路牌图标图片
    public List<Sprite> roadSignSprites;
    //音乐开关按钮
    public Image m_SoundSwitch;
    //震动开关按钮
    public Image m_VibrateSwitch;
    public UIReward m_UIReward;
    //首页logo
    public GameObject m_Logo;
    //////////////////////////////////////////////////多语言设置，文本物体
    //设置标题
    public TMP_Text m_SettingTitle;
    //声音文字
    public TMP_Text m_SoundText;
    //震动文字
    public TMP_Text m_VibrateText;
    //隐私政策
    public TMP_Text m_PrivacyText;
    //用户协议
    public TMP_Text m_UserAgreementText;
    //开始按钮文字
    public TMP_Text m_StartText;
    //打开按钮文本
    public TMP_Text m_OpenText;
    //打开按钮文本
    public TMP_Text m_OpenText1;
    //礼盒奖励文本
    public TMP_Text m_RewardText;
    //更多奖励文本
    public TMP_Text m_MoreRewardText;
    //放弃按钮文本
    public TMP_Text m_GiveUpText;

    void Start()
    {
        SetLevelList();
        MoveCarEnterAnima();
        SetLanguage();
        m_SoundSwitch.sprite = GlobalManager.Instance.IsSound ? switchSprites[0] : switchSprites[1];
        m_VibrateSwitch.sprite = GlobalManager.Instance.IsVibrate ? switchSprites[0] : switchSprites[1];
        CreateCarAndTrail();
        m_Logo.SetActive(true);
        DOVirtual.DelayedCall(2f, () =>
      {
          m_Logo.SetActive(false);
      });
    }
    //设置多语言
    public void SetLanguage()
    {
        m_SettingTitle.text = GlobalManager.Instance.GetLanguageValue("Settings");
        m_SoundText.text = GlobalManager.Instance.GetLanguageValue("Audio");
        m_VibrateText.text = GlobalManager.Instance.GetLanguageValue("Vibrate");
        m_StartText.text = GlobalManager.Instance.GetLanguageValue("Start");
        m_PrivacyText.text = GlobalManager.Instance.mLanguageDict["PrivacyPolicy"][GlobalManager.Instance.CurrentLanguage];
        m_UserAgreementText.text = GlobalManager.Instance.mLanguageDict["UserAgreement"][GlobalManager.Instance.CurrentLanguage];
        m_OpenText.text = GlobalManager.Instance.GetLanguageValue("Open");
        m_OpenText1.text = GlobalManager.Instance.GetLanguageValue("Open");
        m_RewardText.text = GlobalManager.Instance.GetLanguageValue("GiftBoxReward");
        m_MoreRewardText.text = GlobalManager.Instance.GetLanguageValue("MoreRewards");
        m_GiveUpText.text = GlobalManager.Instance.GetLanguageValue("GiveUp");
    }
    // Update is called once per frame
    void Update()
    {

    }
    void OnDestroy()
    {
        DOTween.KillAll();
    }
    public void OnClickStart()
    {
        //Application.OpenURL("https://www.baidu.com");
        AudioManager.Instance.PlayButtonClick();
        GlobalManager.Instance.GameType = GameType.Main;
        MoveCarAppearanceAnima();
    }
    //创建汽车和拖尾
    public void CreateCarAndTrail()
    {
        if (GlobalManager.Instance.PlayerCarSkinName != "")
        {
            GameObject car = Instantiate(Resources.Load<GameObject>("Prefabs/" + GlobalManager.Instance.PlayerCarSkinName), m_Car.transform);
            car.transform.localPosition = Vector3.zero;
            car.transform.localScale = Vector3.one;
        }
        if (GlobalManager.Instance.PlayerCarTrailName != "")
        {
            GameObject trail = Instantiate(Resources.Load<GameObject>("Prefabs/" + GlobalManager.Instance.PlayerCarTrailName), m_Car.transform);
            trail.transform.localPosition = Vector3.zero;
            trail.transform.localScale = Vector3.one;
        }
    }
    //汽车出场动画
    public void MoveCarAppearanceAnima()
    {
        if (m_Camera == null || m_Camera.transform == null || m_Car == null || m_Car.transform == null)
        {
            return;
        }
        AudioManager.Instance.PlayCarSmallMove();
        m_Car.transform.DOMoveZ(m_Car.transform.position.z + 1f, 0.5f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
        });
        //相机跟随
        m_Camera.transform.DOMoveZ(m_Camera.transform.position.z + 1f, 0.5f).SetEase(Ease.OutQuad);
    }
    //汽车入场动画
    public void MoveCarEnterAnima()
    {
        if (GlobalManager.Instance.IsFirstGame)//如果第一次游戏
        {
            if (m_Car == null && m_Car.transform == null)
            {
                return;
            }
            //设置汽车位置
            m_Car.transform.position = new Vector3(m_Car.transform.position.x, m_Car.transform.position.y, m_Car.transform.position.z - 1.2f);
            AudioManager.Instance.PlayCarSmallMove();
            m_Car.transform.DOMoveZ(m_Car.transform.position.z + 1.2f, 0.5f).SetEase(Ease.OutQuad).onComplete = () =>
            {
                StartCarScaleAnimation();
                if (GlobalManager.Instance.IsReward)
                {
                    m_UIReward.ShowReward();
                }
            };
        }
        else
        {
            StartCarScaleAnimation();
        }

    }
    // 开始汽车震动动画
    public void StartCarScaleAnimation()
    {
        return;
        float scaleDuration = 0.1f; // 缩放动画的持续时间
        Vector3 minScale = m_Car.transform.localScale * 0.99f; // 缩小到汽车本身的90%

        m_Car.transform.DOScale(minScale, scaleDuration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo); // 无限循环，来回缩放
    }
    //设置关卡列表
    public void SetLevelList()
    {
        for (int i = 0; i < m_LevelList.Count; i++)
        {
            if (GlobalManager.Instance.CurrentLevel + i + 1 >= 14 && (GlobalManager.Instance.CurrentLevel + i + 1 - 14) % 10 == 0)
            {
                //奖励关卡
                m_LevelList[i].transform.GetChild(0).gameObject.SetActive(false);
                m_LevelList[i].transform.GetChild(2).gameObject.SetActive(true);
                m_LevelList[i].transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = roadSignSprites[0];
            }
            else if (GlobalManager.Instance.CurrentLevel + i + 1 >= 20 && (GlobalManager.Instance.CurrentLevel + i + 1 - 20) % 10 == 0)
            {
                //boss关卡
                m_LevelList[i].transform.GetChild(0).gameObject.SetActive(false);
                m_LevelList[i].transform.GetChild(2).gameObject.SetActive(true);
                m_LevelList[i].transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = roadSignSprites[1];
            }
            else
            {
                //普通关卡
                m_LevelList[i].transform.GetChild(0).GetComponent<TextMesh>().text = (GlobalManager.Instance.CurrentLevel + i + 1).ToString();
                m_LevelList[i].transform.GetChild(0).gameObject.SetActive(true);
                m_LevelList[i].transform.GetChild(2).gameObject.SetActive(false);
            }
        }
    }
    //设置按钮
    public void OnClickSetting()
    {
        m_SettingPanel.SetActive(true);
        AudioManager.Instance.PlayButtonClick();
    }
    //关闭设置
    public void OnClickCloseSetting()
    {
        m_SettingPanel.SetActive(false);
        AudioManager.Instance.PlayButtonClick();
    }
    //震动开启关闭按钮
    public void OnClickVibrate()
    {
        GlobalManager.Instance.IsVibrate = !GlobalManager.Instance.IsVibrate;
        if (GlobalManager.Instance.IsVibrate)
        {
            m_VibrateSwitch.sprite = switchSprites[0];
        }
        else
        {
            m_VibrateSwitch.sprite = switchSprites[1];
        }
    }
    //音效开启关闭按钮
    public void OnClickSound()
    {
        GlobalManager.Instance.IsSound = !GlobalManager.Instance.IsSound;
        if (GlobalManager.Instance.IsSound)
        {
            AudioManager.Instance.ResumeBackgroundMusic();
            m_SoundSwitch.sprite = switchSprites[0];
        }
        else
        {
            AudioManager.Instance.StopBackgroundMusic();
            m_SoundSwitch.sprite = switchSprites[1];
        }
    }
    //转盘按钮
    public void OnClickWheel()
    {
        m_WheelPanel.SetActive(true);
        AudioManager.Instance.PlayButtonClick();
    }
    //隐私政策按钮
    public void OnClickPrivacy()
    {
        Application.OpenURL("https://www.baidu.com");
    }
}
