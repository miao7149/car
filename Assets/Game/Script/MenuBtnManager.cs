using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuBtnManager : MonoBehaviour
{
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
    void OnEnable()
    {
        m_WheelOfFortune.OnDailySpinEvent += RedPointAnim;
        m_Racing.OnDailyCompetition += RedPointAnim;
        m_SwitchSkin.OnNewSkin += RedPointAnim;
        m_HardMode.OnNewHardModeLevel += RedPointAnim;
        m_WheelOfFortune.CloseRedPointEvent += HideRedPoint;
        m_Racing.CloseRedPointEvent += HideRedPoint;
        m_SwitchSkin.CloseRedPointEvent += HideRedPoint;
        m_HardMode.CloseRedPointEvent += HideRedPoint;
    }
    void Start()
    {
        if (GlobalManager.Instance.CurrentLevel > 4)
        {
            m_RacingBtn.transform.GetChild(2).gameObject.SetActive(false);
            m_RacingBtn.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().fontSize = 48;
            m_RacingBtn.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "Racing";
            m_RacingBtn.GetComponent<Button>().interactable = true;
        }
        else
        {
            m_RacingBtn.transform.GetChild(2).gameObject.SetActive(true);
            m_RacingBtn.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().fontSize = 36;
            m_RacingBtn.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "Lv5 unlock";
            m_RacingBtn.GetComponent<Button>().interactable = false;
        }
        if (GlobalManager.Instance.CurrentLevel > 6)
        {
            m_WheelBtn.transform.GetChild(2).gameObject.SetActive(false);
            m_WheelBtn.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().fontSize = 48;
            m_WheelBtn.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "Spin";
            m_WheelBtn.GetComponent<Button>().interactable = true;
        }
        else
        {
            m_WheelBtn.transform.GetChild(2).gameObject.SetActive(true);
            m_WheelBtn.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().fontSize = 36;
            m_WheelBtn.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "Lv7 unlock";
            m_WheelBtn.GetComponent<Button>().interactable = false;
        }
        if (GlobalManager.Instance.CurrentLevel > 13)
        {
            m_QualifyingBtn.transform.GetChild(2).gameObject.SetActive(false);
            m_QualifyingBtn.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().fontSize = 48;
            m_QualifyingBtn.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "League";
            m_QualifyingBtn.GetComponent<Button>().interactable = true;
        }
        else
        {
            m_QualifyingBtn.transform.GetChild(2).gameObject.SetActive(true);
            m_QualifyingBtn.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().fontSize = 36;
            m_QualifyingBtn.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "Lv14 unlock";
            m_QualifyingBtn.GetComponent<Button>().interactable = false;
        }

        if (GlobalManager.Instance.CurrentLevel > 14)
        {
            m_HardBtn.transform.GetChild(2).gameObject.SetActive(false);
            m_HardBtn.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().fontSize = 48;
            m_HardBtn.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "Hard";
            m_HardBtn.GetComponent<Button>().interactable = true;
        }
        else
        {
            m_HardBtn.transform.GetChild(2).gameObject.SetActive(true);
            m_HardBtn.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().fontSize = 36;
            m_HardBtn.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "Lv15 unlock";
            m_HardBtn.GetComponent<Button>().interactable = false;
        }
        if (GlobalManager.Instance.CurrentLevel > 16)
        {
            m_SkinBtn.transform.GetChild(2).gameObject.SetActive(false);
            m_SkinBtn.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().fontSize = 48;
            m_SkinBtn.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "Skin";
            m_SkinBtn.GetComponent<Button>().interactable = true;
        }
        else
        {
            m_SkinBtn.transform.GetChild(2).gameObject.SetActive(true);
            m_SkinBtn.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().fontSize = 36;
            m_SkinBtn.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "Lv17 unlock";
            m_SkinBtn.GetComponent<Button>().interactable = false;
        }
    }
    void OnDisable()
    {
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
    void Update()
    {

    }
    public void RedPointAnim(string name)
    {
        switch (name)
        {
            case "Spin":
                if (GlobalManager.Instance.CurrentLevel > 7)
                {
                    m_WheelBtn.transform.GetChild(1).gameObject.SetActive(true);
                    //红点跳动动画
                    m_WheelBtn.transform.GetChild(1).DOLocalMoveY(m_WheelBtn.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition.y + 10, 0.5f).SetLoops(-1, LoopType.Yoyo);
                }
                break;
            case "Racing":
                if (GlobalManager.Instance.CurrentLevel > 5)
                {
                    m_RacingBtn.transform.GetChild(1).gameObject.SetActive(true);
                    m_RacingBtn.transform.GetChild(1).DOLocalMoveY(m_RacingBtn.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition.y + 10, 0.5f).SetLoops(-1, LoopType.Yoyo);
                }
                break;
            case "Skin":
                if (GlobalManager.Instance.CurrentLevel > 17)
                {
                    m_SkinBtn.transform.GetChild(1).gameObject.SetActive(true);
                    m_SkinBtn.transform.GetChild(1).DOLocalMoveY(m_SkinBtn.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition.y + 10, 0.5f).SetLoops(-1, LoopType.Yoyo);
                }
                break;
            case "Hard":
                if (GlobalManager.Instance.CurrentLevel > 15)
                {
                    m_HardBtn.transform.GetChild(1).gameObject.SetActive(true);
                    m_HardBtn.transform.GetChild(1).DOLocalMoveY(m_HardBtn.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition.y + 10, 0.5f).SetLoops(-1, LoopType.Yoyo);
                }
                break;
        }
    }
    public void HideRedPoint(string name)
    {
        switch (name)
        {
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
}
