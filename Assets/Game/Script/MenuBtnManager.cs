using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuBtnManager : MonoBehaviour {
    // Start is called before the first frame update
    //转盘按钮
    public GameObject m_WheelBtn;

    //竞赛按钮
    public GameObject m_RacingBtn;

    //换皮肤按钮
    public GameObject m_SkinBtn;

    //困难按钮
    public GameObject m_HardBtn;

    //排位赛按钮
    public GameObject m_QualifyingBtn;

    //转盘脚本
    public WheelOfFortune m_WheelOfFortune;

    //竞赛脚本
    public RacingCompetition m_Racing;

    //换皮肤脚本
    public UISwitchSkin m_SwitchSkin;

    //困难脚本
    public UIHardMode m_HardMode;

    //排位赛脚本
    public RankingMatch m_RankingMatch;

    //是否第一次进入换皮肤界面
    public bool IsSkinFirstEnter = true;

    //是否第一次进入转哦安界面
    public bool IsWheelFirstEnter = true;

    //是否第一次进入困难模式界面
    public bool IsHardFirstEnter = true;

    //是否第一次进入排位赛界面
    public bool IsQualifyingFirstEnter = true;

    //是否第一次进入竞赛界面
    public bool IsRacingFirstEnter = true;


    public GameObject SignLayer;
    public GameObject SignRewardLayer;

    void OnEnable() {
        m_WheelOfFortune.OnDailySpinEvent += RedPointAnim;
        m_Racing.OnDailyCompetition += RedPointAnim;
        m_SwitchSkin.OnNewSkin += RedPointAnim;
        m_HardMode.OnNewHardModeLevel += RedPointAnim;
        m_WheelOfFortune.CloseRedPointEvent += HideRedPoint;
        m_Racing.CloseRedPointEvent += HideRedPoint;
        m_SwitchSkin.CloseRedPointEvent += HideRedPoint;
        m_HardMode.CloseRedPointEvent += HideRedPoint;


        if (CheckShowRedPoint()) {
            ShowRedPointHard();
        }
    }

    IEnumerator ShowRacingStartLayer() {
        yield return new WaitForSeconds(2);
        while (SignLayer.activeSelf || SignRewardLayer.activeSelf) {
            yield return null;
        }

        m_Racing.OnRacingBtn();
        IsRacingFirstEnter = false;
        PlayerPrefs.SetInt("IsRacingFirstEnter", 0);
    }

    void Start() {
        var str = GlobalManager.Instance.GetLanguageValue("Unlock");
        if (GlobalManager.Instance.CurrentLevel > 5) {
            m_RacingBtn.transform.GetChild(2).gameObject.SetActive(false);
            m_RacingBtn.transform.GetChild(0).GetChild(0).GetComponent<Text>().fontSize = 48;
            m_RacingBtn.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = GlobalManager.Instance.GetLanguageValue("Racing");
            m_RacingBtn.GetComponent<Button>().interactable = true;
            IsRacingFirstEnter = PlayerPrefs.GetInt("IsRacingFirstEnter", 1) == 1;
            if (IsRacingFirstEnter) {
                // //DoTween等待两秒
                // DOVirtual.DelayedCall(2f, () => {
                //     m_Racing.OnRacingBtn();
                //     IsRacingFirstEnter = false;
                //     PlayerPrefs.SetInt("IsRacingFirstEnter", 0);
                // });
                StartCoroutine(ShowRacingStartLayer());
            }
        }
        else {
            m_RacingBtn.transform.GetChild(2).gameObject.SetActive(true);
            m_RacingBtn.transform.GetChild(0).GetChild(0).GetComponent<Text>().fontSize = 36;
            m_RacingBtn.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = str.Replace("xx", "6");
            m_RacingBtn.GetComponent<Button>().interactable = false;
        }

        if (GlobalManager.Instance.CurrentLevel > 6) {
            m_WheelBtn.transform.GetChild(2).gameObject.SetActive(false);
            m_WheelBtn.transform.GetChild(0).GetChild(0).GetComponent<Text>().fontSize = 48;
            m_WheelBtn.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = GlobalManager.Instance.GetLanguageValue("Spin");
            m_WheelBtn.GetComponent<Button>().interactable = true;
            IsWheelFirstEnter = PlayerPrefs.GetInt("IsWheelFirstEnter", 1) == 1;
            if (IsWheelFirstEnter) {
                DOVirtual.DelayedCall(2f, () => {
                    m_WheelOfFortune.OnClickWheel();
                    IsWheelFirstEnter = false;
                    PlayerPrefs.SetInt("IsWheelFirstEnter", 0);
                });
            }
        }
        else {
            m_WheelBtn.transform.GetChild(2).gameObject.SetActive(true);
            m_WheelBtn.transform.GetChild(0).GetChild(0).GetComponent<Text>().fontSize = 36;
            m_WheelBtn.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = str.Replace("xx", "7");
            m_WheelBtn.GetComponent<Button>().interactable = false;
        }

        if (GlobalManager.Instance.CurrentLevel > 13) {
            m_QualifyingBtn.transform.GetChild(2).gameObject.SetActive(false);
            m_QualifyingBtn.transform.GetChild(0).GetChild(0).GetComponent<Text>().fontSize = 48;
            m_QualifyingBtn.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = GlobalManager.Instance.GetLanguageValue("League");
            m_QualifyingBtn.GetComponent<Button>().interactable = true;
            IsQualifyingFirstEnter = PlayerPrefs.GetInt("IsQualifyingFirstEnter", 1) == 1;
            if (IsQualifyingFirstEnter) {
                DOVirtual.DelayedCall(2f, () => {
                    m_RankingMatch.FirstOpenRankingMatch();
                    IsQualifyingFirstEnter = false;
                    PlayerPrefs.SetInt("IsQualifyingFirstEnter", 0);
                });
            }
        }
        else {
            m_QualifyingBtn.transform.GetChild(2).gameObject.SetActive(true);
            m_QualifyingBtn.transform.GetChild(0).GetChild(0).GetComponent<Text>().fontSize = 36;
            m_QualifyingBtn.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = str.Replace("xx", "14");
            m_QualifyingBtn.GetComponent<Button>().interactable = false;
        }

        if (GlobalManager.Instance.CurrentLevel > 15) {
            m_HardBtn.transform.GetChild(2).gameObject.SetActive(false);
            m_HardBtn.transform.GetChild(0).GetChild(0).GetComponent<Text>().fontSize = 48;
            m_HardBtn.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = GlobalManager.Instance.GetLanguageValue("HardMode");
            m_HardBtn.GetComponent<Button>().interactable = true;
            IsHardFirstEnter = PlayerPrefs.GetInt("IsHardFirstEnter", 1) == 1;
            if (IsHardFirstEnter) {
                DOVirtual.DelayedCall(2f, () => {
                    m_HardMode.OnHardModeBtn();
                    IsHardFirstEnter = false;
                    PlayerPrefs.SetInt("IsHardFirstEnter", 0);
                });
            }
        }
        else {
            m_HardBtn.transform.GetChild(2).gameObject.SetActive(true);
            m_HardBtn.transform.GetChild(0).GetChild(0).GetComponent<Text>().fontSize = 36;
            m_HardBtn.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = str.Replace("xx", "15");
            m_HardBtn.GetComponent<Button>().interactable = false;
        }

        if (GlobalManager.Instance.CurrentLevel > 16) {
            m_SkinBtn.transform.GetChild(2).gameObject.SetActive(false);
            m_SkinBtn.transform.GetChild(0).GetChild(0).GetComponent<Text>().fontSize = 48;
            m_SkinBtn.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = GlobalManager.Instance.GetLanguageValue("Skin");
            m_SkinBtn.GetComponent<Button>().interactable = true;
            IsSkinFirstEnter = PlayerPrefs.GetInt("IsSkinFirstEnter", 1) == 1;
            if (IsSkinFirstEnter) {
                DOVirtual.DelayedCall(2f, () => {
                    m_SwitchSkin.OnOpenSwitchBtn();
                    IsSkinFirstEnter = false;
                    PlayerPrefs.SetInt("IsSkinFirstEnter", 0);
                });
            }
        }
        else {
            m_SkinBtn.transform.GetChild(2).gameObject.SetActive(true);
            m_SkinBtn.transform.GetChild(0).GetChild(0).GetComponent<Text>().fontSize = 36;
            m_SkinBtn.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = str.Replace("xx", "17");
            m_SkinBtn.GetComponent<Button>().interactable = false;
        }
    }

    void OnDisable() {
        m_WheelOfFortune.OnDailySpinEvent -= RedPointAnim;
        m_Racing.OnDailyCompetition -= RedPointAnim;
        m_SwitchSkin.OnNewSkin -= RedPointAnim;
        m_HardMode.OnNewHardModeLevel -= RedPointAnim;
        m_WheelOfFortune.CloseRedPointEvent -= HideRedPoint;
        m_Racing.CloseRedPointEvent -= HideRedPoint;
        m_SwitchSkin.CloseRedPointEvent -= HideRedPoint;
        m_HardMode.CloseRedPointEvent -= HideRedPoint;
    }

    // Update is called once per frame
    void Update() {
    }

    public void RedPointAnim(string name) {
        switch (name) {
            case "Spin":
                if (GlobalManager.Instance.CurrentLevel > 7) {
                    m_WheelBtn.transform.GetChild(1).gameObject.SetActive(true);
                    //红点跳动动画
                    m_WheelBtn.transform.GetChild(1).DOLocalMoveY(m_WheelBtn.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition.y + 10, 0.5f).SetLoops(-1, LoopType.Yoyo);
                }

                break;
            case "Racing":
                if (GlobalManager.Instance.CurrentLevel > 5) {
                    m_RacingBtn.transform.GetChild(1).gameObject.SetActive(true);
                    m_RacingBtn.transform.GetChild(1).DOLocalMoveY(m_RacingBtn.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition.y + 10, 0.5f).SetLoops(-1, LoopType.Yoyo);
                }

                break;
            case "Skin":
                if (GlobalManager.Instance.CurrentLevel > 17) {
                    m_SkinBtn.transform.GetChild(1).gameObject.SetActive(true);
                    m_SkinBtn.transform.GetChild(1).DOLocalMoveY(m_SkinBtn.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition.y + 10, 0.5f).SetLoops(-1, LoopType.Yoyo);
                }

                break;
            case "Hard":
                Debug.Log("redPoint" + GlobalManager.Instance.CurrentLevel);
                if (GlobalManager.Instance.CurrentLevel > 15) {
                    m_HardBtn.transform.GetChild(1).gameObject.SetActive(true);
                    m_HardBtn.transform.GetChild(1).DOLocalMoveY(m_HardBtn.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition.y + 10, 0.5f).SetLoops(-1, LoopType.Yoyo);
                }

                break;
        }
    }

    public void HideRedPoint(string name) {
        switch (name) {
            case "Spin":
                m_WheelBtn.transform.GetChild(1).gameObject.SetActive(false);
                m_WheelBtn.transform.GetChild(1).DOKill();
                break;
            case "Racing":
                m_RacingBtn.transform.GetChild(1).gameObject.SetActive(false);
                m_RacingBtn.transform.GetChild(1).DOKill();
                break;
            case "Skin":
                m_SkinBtn.transform.GetChild(1).gameObject.SetActive(false);
                m_SkinBtn.transform.GetChild(1).DOKill();
                break;
            case "Hard":

                m_HardBtn.transform.GetChild(1).gameObject.SetActive(false);
                m_HardBtn.transform.GetChild(1).DOKill();
                break;
        }
    }

    public void ShowRedPointHard() {
        m_HardBtn.transform.GetChild(1).gameObject.SetActive(true);
        m_HardBtn.transform.GetChild(1).DOLocalMoveY(m_HardBtn.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition.y + 10, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }

    public void HideRedPointHard() {
        m_HardBtn.transform.GetChild(1).gameObject.SetActive(false);
        m_HardBtn.transform.GetChild(1).DOKill();
    }


    public bool CheckShowRedPoint() {
        var level = GlobalManager.Instance.CurrentLevel;
        //Debug.Log("currentlevel" + GlobalManager.Instance.CurrentLevel);


        for (int i = 266; i >= 0; i--) {
            var unlockLevel = 25 + (i + 1) * 10;
            if (level < unlockLevel) {
                continue;
            }
            else {
                var id = i + 4;
                var unlock = PlayerPrefs.GetInt("HardLevelStatus" + id, -1) == 1; //0已完成 1已解锁
                if (!unlock) return true;
            }
        }

        if (level >= 25) {
            var unlock = PlayerPrefs.GetInt("HardLevelStatus" + 3, -1) == 1; //0已完成 1已解锁
            if (!unlock) return true;
        }

        if (level >= 20) {
            var unlock = PlayerPrefs.GetInt("HardLevelStatus" + 2, -1) == 1; //0已完成 1已解锁
            if (!unlock) return true;
        }

        if (level >= 16) {
            var unlock = PlayerPrefs.GetInt("HardLevelStatus" + 1, -1) == 1; //0已完成 1已解锁
            if (!unlock) return true;
        }


        return false;
    }
}
