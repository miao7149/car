using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class FiveStar : MonoBehaviour {
    public GameObject m_Root; // 根节点对象
    public Image m_Board; //底板
    public Image[] stars; // 五个星星对象
    public Sprite starOn; // 亮起的星星图片
    public Sprite starOff; // 熄灭的星星图片
    public Image halo; // 光圈对象
    public GameObject m_InputField; //输入框

    private int mStarIndex = 0; // 星星索引

    //描述文本
    public Text m_DesText;

    //评价文本
    public Text m_Evaluate;


    //是否弹出过好评界面
    bool m_IsShowFiveStar = false;

    void Start() {
        m_IsShowFiveStar = PlayerPrefs.GetInt("IsShowFiveStar", 0) == 1;
        if (GlobalManager.Instance.CurrentLevel > 8 && m_IsShowFiveStar == false) {
            m_Root.SetActive(true);
            StartCoroutine(RunMarquee());
            StartCoroutine(RunHaloAnimation());
        }
    }

    IEnumerator RunMarquee() {
        while (true) {
            for (int i = 0; i < stars.Length; i++) {
                stars[i].sprite = starOn;
                yield return new WaitForSeconds(0.5f);
                if (i == 4) {
                    for (int j = 0; j < stars.Length; j++) {
                        stars[j].sprite = starOff;
                    }

                    yield return new WaitForSeconds(0.3f);
                }
            }
        }
    }

    IEnumerator RunHaloAnimation() {
        while (true) {
            // 放大
            for (float scale = 1f; scale <= 1.5f; scale += 0.01f) {
                halo.transform.localScale = new Vector3(scale, scale, 1);
                halo.color = new Color(halo.color.r, halo.color.g, halo.color.b, 1.5f - scale);
                yield return new WaitForSeconds(0.02f);
                halo.transform.localScale = new Vector3(1, 1, 1);
                halo.color = new Color(1, 1, 1, 1);
            }
        }
    }

    public void OnStarClick(int index) {
        StopAllCoroutines(); // 停止跑马灯动画
        halo.gameObject.SetActive(false); // 隐藏光圈
        for (int i = 0; i < stars.Length; i++) {
            if (i <= index) {
                stars[i].sprite = starOn;
            }
            else {
                stars[i].sprite = starOff;
            }
        }

        if (index < 4) {
            m_Board.GetComponent<RectTransform>().sizeDelta = new Vector2(m_Board.GetComponent<RectTransform>().sizeDelta.x, 1188);
            m_InputField.gameObject.SetActive(true);
            m_DesText.text = GlobalManager.Instance.GetLanguageValue("Proposal");
            m_Evaluate.text = GlobalManager.Instance.GetLanguageValue("InputProposal");
        }
        else {
            m_Board.GetComponent<RectTransform>().sizeDelta = new Vector2(m_Board.GetComponent<RectTransform>().sizeDelta.x, 1032);
            m_InputField.gameObject.SetActive(false);
            m_DesText.text = GlobalManager.Instance.GetLanguageValue("FiveStarRating");
        }

        mStarIndex = index;
    }

    //关闭按钮
    public void OnCloseClick() {
        m_IsShowFiveStar = true;
        PlayerPrefs.SetInt("IsShowFiveStar", 1);
        m_Root.SetActive(false);
    }

    //提交按钮
    public void OnSubmitClick() {
        m_IsShowFiveStar = true;
        PlayerPrefs.SetInt("IsShowFiveStar", 1);
        if (mStarIndex == 4) {
            //跳转网页
            Application.OpenURL("https://play.google.com/store/apps/details?id=com.game.tjm_android");
        }
        else {
            string s = m_InputField.GetComponent<InputField>().text;
            //StartCoroutine(Global.PostRequest("", s));
        }

        m_Root.SetActive(false);
        ApplovinSDKManager.Instance().interstitialAdsManager.ShowInterstitialAd(null);
    }
}
