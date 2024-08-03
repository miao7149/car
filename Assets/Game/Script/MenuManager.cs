using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
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
    //////////////////////////////////////////////////多语言设置，文本物体
    //设置标题
    public Text m_SettingTitle;
    //声音文字
    public Text m_SoundText;
    //震动文字
    public Text m_VibrateText;
    //隐私政策
    public Text m_PrivacyText;
    //开始按钮文字
    public Text m_StartText;

    void Start()
    {
        SetLevelList();
        MoveCarEnterAnima();
        SetLanguage();
    }
    //设置多语言
    public void SetLanguage()
    {
        m_SettingTitle.text = GlobalManager.Instance.GetLanguageValue("Settings");
        m_SoundText.text = GlobalManager.Instance.GetLanguageValue("Audio");
        m_VibrateText.text = GlobalManager.Instance.GetLanguageValue("Vibrate");
        m_StartText.text = GlobalManager.Instance.GetLanguageValue("Start") + "Lv" + GlobalManager.Instance.CurrentLevel;
        // m_PrivacyText.text = GlobalManager.Instance.mLanguageDict["PrivacyText"][GlobalManager.Instance.CurrentLanguage];
    }
    // Update is called once per frame
    void Update()
    {

    }
    void OnDestroy()
    {
        if (m_Car != null)
            m_Car.transform.DOKill();
    }
    public void OnClickStart()
    {
        //Application.OpenURL("https://www.baidu.com");
        AudioManager.Instance.PlayButtonClick();
        MoveCarAppearanceAnima();
    }
    //汽车出场动画
    public void MoveCarAppearanceAnima()
    {
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
            //设置汽车位置
            m_Car.transform.position = new Vector3(m_Car.transform.position.x, m_Car.transform.position.y, m_Car.transform.position.z - 1f);
            AudioManager.Instance.PlayCarSmallMove();
            m_Car.transform.DOMoveZ(m_Car.transform.position.z + 1f, 0.5f).SetEase(Ease.OutQuad).onComplete = () =>
            {
                StartCarScaleAnimation();
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
        float scaleDuration = 0.1f; // 缩放动画的持续时间
        Vector3 minScale = m_Car.transform.localScale * 0.99f; // 缩小到汽车本身的90%

        m_Car.transform.DOScale(minScale, scaleDuration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo); // 无限循环，来回缩放
    }
    //设置关卡列表
    public void SetLevelList()
    {
        for (int i = 0; i < m_LevelList.Count; i++)
        {
            m_LevelList[i].transform.GetChild(1).GetComponent<TextMesh>().text = (GlobalManager.Instance.CurrentLevel + i).ToString();
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
    }
    //音效开启关闭按钮
    public void OnClickSound()
    {
        GlobalManager.Instance.IsSound = !GlobalManager.Instance.IsSound;
        if (GlobalManager.Instance.IsSound)
        {
            AudioManager.Instance.ResumeBackgroundMusic();
        }
        else
        {
            AudioManager.Instance.StopBackgroundMusic();
        }
    }
    //转盘按钮
    public void OnClickWheel()
    {
        m_WheelPanel.SetActive(true);
        AudioManager.Instance.PlayButtonClick();
    }
}
