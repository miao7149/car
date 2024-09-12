using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RacingPlayerItem : MonoBehaviour
{
    // Start is called before the first frame update
    //玩家名字
    public TMP_Text playerName;
    //玩家关卡数
    public TMP_Text playerLevels;
    //玩家itemSprite
    public Sprite[] playerItemSprite;
    //汽车
    public GameObject car;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Init(RacingPlayerItemData data)//370
    {
        playerName.text = data.Name;
        playerLevels.text = data.CompleteLevel.ToString();
        if (data.Id >= 0)
        {
            gameObject.GetComponent<Image>().sprite = playerItemSprite[0];
        }
        else
        {
            gameObject.GetComponent<Image>().sprite = playerItemSprite[1];
        }
        car.transform.localPosition = new Vector3(-50, 0, 0);
        car.transform.DOLocalMoveX(-50 + data.CompleteLevel * 12.3f, 1f);
    }
}
