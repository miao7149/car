using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
public class Car : MonoBehaviour
{
    public CarType type; //车型 1-2m小车   2-3m大车
    public Vector2Int pos; //车头位置

    public Vector2Int dir; //方向 1上 2下  3左  4右
    public int turn; //转向 1直行 2左转 3右转 4右掉头 5左掉头
    public List<Vector2Int> posArr = new();
    public bool dead = false;//是否死亡
    public Tweener moveAction = null;
    //气球物体
    public GameObject m_Balloon;
    //无人机物体
    public GameObject m_Drone;
    //方向图片
    public List<Sprite> m_Textures = new();
    //方向图片
    public SpriteRenderer sprite;
    // 无人机目标位置（车顶）
    public Transform droneTargetPosition;
    //是否播放完金币动画
    public bool isPlayCoinAnimation = false;
    public delegate void CarOutOfBoundsHandler(Vector3 pos, Vector3 dir);
    public event CarOutOfBoundsHandler CarOutOfBounds;

    public void Init(CarInfo info)
    {
        dead = false;
        posArr.Clear();

        type = info.type;
        switch (info.dir)
        {
            case 0:
                dir = Vector2Int.up;
                break;
            case 1:
                dir = Vector2Int.down;
                break;
            case 2:
                dir = Vector2Int.left;
                break;
            case 3:
                dir = Vector2Int.right;
                break;
        }

        pos = new Vector2Int(info.posX, info.posY);
        posArr.Add(pos);
        turn = info.turn;
        sprite.flipX = false;
        switch (turn)
        {
            case 1:
                sprite.sprite = m_Textures[0];
                break;
            case 2:
                sprite.sprite = m_Textures[1];
                sprite.flipX = true;
                break;
            case 3:
                sprite.sprite = m_Textures[1];
                break;
            case 4:
                sprite.sprite = m_Textures[2];
                break;
            case 5:
                sprite.sprite = m_Textures[2];
                sprite.flipX = true;
                break;
            default:
                break;
        }
        transform.localPosition = GameManager.Instance.ConvertPos(new Vector3(pos.x, 0, pos.y));

        if (type == CarType.Small)
            switch (info.dir)
            {
                case 0:
                    posArr.Add(new Vector2Int(pos.x, pos.y - 1));
                    break;
                case 1:
                    posArr.Add(new Vector2Int(pos.x, pos.y + 1));
                    break;
                case 2:
                    posArr.Add(new Vector2Int(pos.x + 1, pos.y));
                    break;
                case 3:
                    posArr.Add(new Vector2Int(pos.x - 1, pos.y));
                    break;
            }
        else
            switch (info.dir)
            {
                case 0:
                    posArr.Add(new Vector2Int(pos.x, pos.y - 1));
                    posArr.Add(new Vector2Int(pos.x, pos.y - 2));
                    break;
                case 1:
                    posArr.Add(new Vector2Int(pos.x, pos.y + 1));
                    posArr.Add(new Vector2Int(pos.x, pos.y + 2));
                    break;
                case 2:
                    posArr.Add(new Vector2Int(pos.x + 1, pos.y));
                    posArr.Add(new Vector2Int(pos.x + 2, pos.y));
                    break;
                case 3:
                    posArr.Add(new Vector2Int(pos.x - 1, pos.y));
                    posArr.Add(new Vector2Int(pos.x - 2, pos.y));
                    break;
            }

        switch (info.dir)
        {
            case 0:
                transform.localEulerAngles = new Vector3(0, 0, 0);
                break;
            case 1:
                transform.localEulerAngles = new Vector3(0, 180, 0);
                break;
            case 2:
                transform.localEulerAngles = new Vector3(0, -90, 0);
                break;
            case 3:
                transform.localEulerAngles = new Vector3(0, 90, 0);
                break;
        }
    }
    void Update()
    {
        if (moveAction != null && moveAction.IsActive() && isPlayCoinAnimation == false)
        {
            if (moveAction.IsPlaying())
            {
                //判断汽车是否出屏幕
                Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
                bool isOutOfScreen = screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height;

                if (isOutOfScreen)
                {
                    CarOutOfBounds?.Invoke(transform.position, transform.forward);
                    isPlayCoinAnimation = true;
                    AudioManager.Instance.PlayGetCoin();
                }

            }
        }
    }
    void OnDestroy()
    {
        if (moveAction != null)
        {
            moveAction.Kill();
            moveAction = null;
            transform.DOKill();
        }
    }
    void OnDisable()
    {
        if (moveAction != null)
        {
            moveAction.Kill();
            moveAction = null;
            transform.DOKill();

        }
    }
    //显示气球
    public void ShowBalloon()
    {
        m_Balloon.SetActive(true);
    }
    // 开始汽车震动动画
    public void StartCarScaleAnimation()
    {
        float scaleDuration = 0.1f; // 缩放动画的持续时间
        transform.localScale = transform.localScale * 1.1f; // 缩小到汽车本身的90%
        Vector3 minScale = transform.localScale * 0.99f; // 缩小到汽车本身的90%
        transform.DOScale(minScale, scaleDuration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo); // 无限循环，来回缩放
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
