using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.Mathematics;
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
    //汽车图片
    public Sprite[] carSprite;
    //关卡数图片
    public Sprite[] levelSprite;
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
        playerLevels.text = math.min(data.CompleteLevel, 30).ToString();
        if (data.Id >= 0)
        {
            gameObject.GetComponent<Image>().sprite = playerItemSprite[0];
            car.GetComponent<Image>().sprite = carSprite[0];
            playerLevels.transform.parent.GetComponent<Image>().sprite = levelSprite[0];
        }
        else
        {
            gameObject.GetComponent<Image>().sprite = playerItemSprite[1];
            car.GetComponent<Image>().sprite = carSprite[1];
            playerLevels.transform.parent.GetComponent<Image>().sprite = levelSprite[1];
        }
        car.transform.localPosition = new Vector3(-50, 0, 0);
        car.transform.DOLocalMoveX(-50 + math.min(data.CompleteLevel, 30) * 12.3f, 1f);
    }
}
