using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
public class Block : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector2Int pos;
    public GameObject m_Balloon;
    public GameObject m_Drone;
    // 无人机目标位置（车顶）
    Transform droneTargetPosition;
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
    public void MoveDroneToCarTop(Action callback)
    {
        m_Drone.transform.position = new Vector3(Camera.main.transform.position.x - 2, Camera.main.transform.position.y, Camera.main.transform.position.z); // 无人机初始位置在车顶上方
        m_Drone.transform.rotation = Quaternion.Euler(0, 0, 0); // 无人机初始朝向
        droneTargetPosition = m_Balloon.transform; // 无人机飞到气球上方
        if (m_Drone != null && droneTargetPosition != null)
        {
            m_Drone.SetActive(true); // 确保无人机是激活的
            Vector3[] path = new Vector3[]
            {
                m_Drone.transform.position,
                new Vector3(droneTargetPosition.position.x + 2, (m_Drone.transform.position.y +droneTargetPosition.position.y)/2, (m_Drone.transform.position.z + droneTargetPosition.position.z) / 2),
                new Vector3(droneTargetPosition.position.x, droneTargetPosition.position.y + 2, droneTargetPosition.position.z)
            };

            // 使用 DOPath 方法实现弧线运动
            m_Drone.transform.DOPath(path, 3f, PathType.CatmullRom).SetEase(Ease.InOutQuad).OnComplete(() =>
            {
                m_Drone.transform.DOMove(droneTargetPosition.position, 1f).SetEase(Ease.InOutQuad).OnComplete(() =>
                {
                    callback?.Invoke();
                });
                m_Drone.transform.DOLocalRotate(new Vector3(0, 0, 0), 1f).SetEase(Ease.InOutQuad); // 1秒内旋转到0度
            }); // 3秒内飞到车顶
        }
    }
}
