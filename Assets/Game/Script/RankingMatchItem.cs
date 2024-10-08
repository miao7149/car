using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankingMatchItem : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject m_ItemBg;
    public GameObject m_IdBg;
    public TMP_Text m_IdText;
    public TMP_Text m_NameText;
    public TMP_Text m_ScoreText;
    public Sprite[] m_ItemBgSprite;//0:升级，1默认，2降级，3自己
    public Sprite[] m_IdBgSprite;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Init(PlayerInfo playerInfo, int index)
    {
        m_IdText.text = index.ToString();
        m_NameText.text = playerInfo.Name;
        m_ScoreText.text = playerInfo.TrophyProp.Trophy.Count.ToString();
        if (index <= 30)
        {
            m_IdBg.GetComponent<Image>().sprite = m_IdBgSprite[0];
            m_ItemBg.GetComponent<Image>().sprite = m_ItemBgSprite[0];
        }
        else if (index > 60 && GlobalManager.Instance.CurrentRank > 1)
        {
            m_IdBg.GetComponent<Image>().sprite = m_IdBgSprite[2];
            m_ItemBg.GetComponent<Image>().sprite = m_ItemBgSprite[2];
        }
        else
        {
            m_IdBg.GetComponent<Image>().sprite = m_IdBgSprite[1];
            m_ItemBg.GetComponent<Image>().sprite = m_ItemBgSprite[1];
        }
        if (playerInfo.Id == -1)
        {
            m_IdBg.GetComponent<Image>().sprite = m_IdBgSprite[3];
            m_ItemBg.GetComponent<Image>().sprite = m_ItemBgSprite[3];
        }
    }
}
