using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Block : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector2Int pos;
    public GameObject m_Balloon;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Init(BlockInfo info)
    {
        this.pos.x = info.pos.x;
        this.pos.y = info.pos.y;
        transform.parent.parent.SetParent(transform, false);
        transform.parent.parent.localPosition = GameManager.Instance.ConvertPos(new Vector3(info.pos.x, 0, info.pos.y));
    }
    //碰撞检测
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("路障碰撞检测");
        var car = other.transform.parent.parent.GetComponent<Car>();

        if (car.type == CarType.Bulldozer)//推土机
        {
            gameObject.SetActive(false);
        }
        else
        {
            //car.moveAction.Kill();
            //GameManager.Instance.SetGameStatu(GameStatu.faled);
        }

    }
    //显示气球
    public void ShowBalloon()
    {
        m_Balloon.SetActive(true);
    }
}
