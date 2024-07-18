using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Block : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
}
