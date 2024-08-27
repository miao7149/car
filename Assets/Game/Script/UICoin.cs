using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class UICoin : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text m_CoinText;
    void Start()
    {
        m_CoinText.text = GlobalManager.Instance.PlayerCoin.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void UpdateCoin()
    {
        int startValue = int.Parse(m_CoinText.text);
        int endValue = GlobalManager.Instance.PlayerCoin;

        // 使用 DOTween 进行数字上涨动画
        DOTween.To(() => startValue, x => startValue = x, endValue, 1f)
            .OnUpdate(() => m_CoinText.text = startValue.ToString())
            .SetEase(Ease.OutQuad);
    }
}
