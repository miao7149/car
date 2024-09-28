using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TipsManager : MonoBehaviour
{
    // Start is called before the first frame update
    // 单例实例
    private static TipsManager instance;
    // 公共访问点
    public static TipsManager Instance
    {
        get
        {
            return instance;
        }
    }
    public GameObject m_TipsRoot;
    public GameObject m_TipsText;
    void Start()
    {
        instance = this;
    }
    //显示提示，传入提示内容，显示一秒后自动隐藏
    public void ShowTips(string tips)
    {
        m_TipsText.GetComponent<TMP_Text>().text = tips;
        m_TipsRoot.SetActive(true);
        StartCoroutine(HideTips());
    }
    //隐藏提示
    IEnumerator HideTips()
    {
        yield return new WaitForSeconds(0.8f);
        m_TipsRoot.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
