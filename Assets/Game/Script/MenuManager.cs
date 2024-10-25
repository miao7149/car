using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour {
    public static MenuManager instance;

    public static MenuManager Instance() {
        if (instance == null) {
            instance = FindObjectOfType<MenuManager>();
        }

        return instance;
    }

    // Start is called before the first frame update
    //汽车物体
    public GameObject m_Car;

    public List<GameObject> m_LevelList;

    //设置界面
    public GameObject m_SettingPanel;

    //相机
    public Camera m_Camera;

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

    //UI金币
    public GameObject m_UICoin;

    //输入框
    public InputField m_InputField;

    //////////////////////////////////////////////////多语言设置，文本物体


    void Start() {
        SetLevelList();
        MoveCarEnterAnima();
        SetLanguage();
        m_SoundSwitch.sprite = GlobalManager.Instance.IsSound ? switchSprites[0] : switchSprites[1];
        m_VibrateSwitch.sprite = GlobalManager.Instance.IsVibrate ? switchSprites[0] : switchSprites[1];
        CreateCarAndTrail();
        m_Logo.SetActive(true);
        DOVirtual.DelayedCall(2f, () => {
            m_Logo.SetActive(false);
        });
        m_InputField.onEndEdit.AddListener(OnInputFieldEndEdit);
        m_InputField.text = GlobalManager.Instance.PlayerName;

        StartCoroutine(LogHelper.LogToServer("EnterHome", new Dictionary<string, object>() {
            { "CarType", GlobalManager.Instance.PlayerCarSkinName },
            { "UserLevel", GlobalManager.Instance.CurrentRank },
            { "AudioFlag", GlobalManager.Instance.IsSound ? "11" : "00" },
            { "VibFlag", GlobalManager.Instance.IsVibrate ? "11" : "00" }
        }));
    }

    //设置多语言
    public void SetLanguage() {
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            GlobalManager.Instance.CurrentLevel++;
            PlayerPrefs.SetInt("CurrentLevel", GlobalManager.Instance.CurrentLevel);
            SetLevelList();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            GlobalManager.Instance.CurrentLevel--;
            PlayerPrefs.SetInt("CurrentLevel", GlobalManager.Instance.CurrentLevel);
            SetLevelList();
        }
    }

    void OnDestroy() {
        instance = null;
        DOTween.KillAll();
    }

    public void OnClickStart() {
        if (DOTween.IsTweening(m_Car.transform)) return;
        //Application.OpenURL("https://www.baidu.com");
        AudioManager.Instance.PlayButtonClick();
        GlobalManager.Instance.GameType = GameType.Main;
        GlobalManager.Instance.difficuteMode = m_LevelList[0].transform.GetChild(0).gameObject.activeSelf == false;
        MoveCarAppearanceAnima();
        StartCoroutine(LogHelper.LogToServer("ClickModule", new Dictionary<string, object>() {
            { "ModuleId", "A0" }
        }));
    }

    public Material[] homeMaterials;

    //创建汽车和拖尾
    public void CreateCarAndTrail() {
        if (GlobalManager.Instance.PlayerCarSkinName != "") {
            GameObject car = Instantiate(Resources.Load<GameObject>("Prefabs/" + GlobalManager.Instance.PlayerCarSkinName), m_Car.transform);
            car.transform.localPosition = Vector3.zero;
            car.transform.localScale = Vector3.one;

            Material[] ma = new Material[2];
            ma[0] = homeMaterials[int.Parse(GlobalManager.Instance.PlayerCarSkinName.Split("_")[1]) - 1];
            ma[1] = car.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().materials[1];
            car.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().materials = ma;

            //car.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().materials[0] = homeMaterials[int.Parse(GlobalManager.Instance.PlayerCarSkinName.Split("_")[1]) - 1];
        }
    }

    private Vector3 offsetCamera;

    //汽车出场动画
    public void MoveCarAppearanceAnima() {
        if (m_Camera == null || m_Camera.transform == null || m_Car == null || m_Car.transform == null) {
            return;
        }

        offsetCamera = m_Camera.transform.position - m_Car.transform.position;

        AudioManager.Instance.PlayCarSmallMove();
        m_Car.transform.GetChild(2).GetComponent<Animation>().Stop();
        //StartCoroutine(CameraFollowCar());
        m_Car.transform.DOMoveZ(m_Car.transform.position.z + 3.6f, 0.7f).SetEase(Ease.InOutQuart).OnComplete(() => {
            //StopCoroutine(CameraFollowCar());
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
        });
        //相机跟随
        m_Camera.transform.DOMoveZ(m_Camera.transform.position.z + 3.6f, 0.7f).SetEase(Ease.InOutQuart);
        //m_Camera.transform.DOShakePosition(0.7f, 0.03f, 30, 0.1f, false, true).SetEase(Ease.InOutQuad);
    }

    //public GameObject m_CameraNode;

    // IEnumerator CameraFollowCar() {
    //     while (true) {
    //         yield return null;
    //         m_Camera.transform.position = m_Car.transform.position + offsetCamera;
    //     }
    // }

    //汽车入场动画
    public void MoveCarEnterAnima() {
        if (GlobalManager.Instance.IsFirstGame) //如果第一次游戏
        {
            if (m_Car == null && m_Car.transform == null) {
                return;
            }

            //设置汽车位置
            m_Car.transform.position = new Vector3(m_Car.transform.position.x, m_Car.transform.position.y, m_Car.transform.position.z - 1.2f);
            AudioManager.Instance.PlayCarSmallMove();
            m_Car.transform.DOMoveZ(m_Car.transform.position.z + 1.2f, 0.5f).SetEase(Ease.OutQuad).onComplete = () => {
                // StartCarScaleAnimation();
                if (GlobalManager.Instance.IsReward) {
                    m_UIReward.ShowReward();
                }
            };
        }
        else {
            // StartCarScaleAnimation();
        }
    }

    //InputField 输入框回调
    public void OnInputFieldEndEdit(string value) {
        GlobalManager.Instance.PlayerName = value;
        GlobalManager.Instance.SaveGameData();
    }

    // // 开始汽车震动动画
    // public void StartCarScaleAnimation() {
    //     float scaleDuration = 0.1f; // 缩放动画的持续时间
    //     Vector3 minScale = m_Car.transform.localScale * 0.99f; // 缩小到汽车本身的90%
    //
    //     m_Car.transform.DOScale(minScale, scaleDuration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo); // 无限循环，来回缩放
    // }

    //设置关卡列表
    public void SetLevelList() {
        for (int i = 0; i < m_LevelList.Count; i++) {
            if (GlobalManager.Instance.CurrentLevel + i + 1 >= 14 && (GlobalManager.Instance.CurrentLevel + i + 1 - 14) % 10 == 0) {
                //奖励关卡
                m_LevelList[i].transform.GetChild(0).gameObject.SetActive(false);
                m_LevelList[i].transform.GetChild(2).gameObject.SetActive(true);
                m_LevelList[i].transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = roadSignSprites[0];
            }
            else if (GlobalManager.Instance.CurrentLevel + i + 1 >= 20 && (GlobalManager.Instance.CurrentLevel + i + 1 - 20) % 10 == 0) {
                //boss关卡
                m_LevelList[i].transform.GetChild(0).gameObject.SetActive(false);
                m_LevelList[i].transform.GetChild(2).gameObject.SetActive(true);
                m_LevelList[i].transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = roadSignSprites[1];
            }
            else {
                //普通关卡
                m_LevelList[i].transform.GetChild(0).GetComponent<TextMesh>().text = (GlobalManager.Instance.CurrentLevel + i + 1).ToString();
                m_LevelList[i].transform.GetChild(0).gameObject.SetActive(true);
                m_LevelList[i].transform.GetChild(2).gameObject.SetActive(false);
            }
        }
    }

    //设置按钮
    public void OnClickSetting() {
        m_SettingPanel.SetActive(true);
        AudioManager.Instance.PlayButtonClick();
        StartCoroutine(LogHelper.LogToServer("ClickModule", new Dictionary<string, object>() {
            { "ModuleId", "B0" }
        }));
    }

    //关闭设置
    public void OnClickCloseSetting() {
        m_SettingPanel.SetActive(false);
        AudioManager.Instance.PlayButtonClick();
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

    //隐私政策按钮
    public void OnClickPrivacy() {
        Application.OpenURL("https://www.baidu.com");
    }
}
