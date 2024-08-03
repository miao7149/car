using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
public class Car : MonoBehaviour
{
    public CarType type; //车型 1-2m小车   2-3m大车
    public Vector2Int pos; //车头位置

    public Vector2Int dir; //方向 1上 2右  3下  4左
    public int turn; //转向 1直行 2左转 3右转 4左掉头 5右掉头
    public List<Vector2Int> posArr = new();
    public bool dead = false;//是否死亡
    public Tweener moveAction = null;
    //气球物体
    public GameObject m_Balloon;
    //方向图片
    public List<Sprite> m_Textures = new();
    //方向图片
    public SpriteRenderer sprite;

    public void Init(CarInfo info)
    {
        dead = false;
        posArr.Clear();

        type = info.type;
        switch (info.dir)
        {
            case 1:
                dir = Vector2Int.up;
                break;
            case 2:
                dir = Vector2Int.right;
                break;
            case 3:
                dir = Vector2Int.down;
                break;
            case 4:
                dir = Vector2Int.left;
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
                sprite.flipX = true;
                break;
            case 5:
                sprite.sprite = m_Textures[2];
                break;
            default:
                break;
        }
        transform.localPosition = GameManager.Instance.ConvertPos(new Vector3(pos.x, 0, pos.y));

        if (type == CarType.Small)
            switch (info.dir)
            {
                case 1:
                    posArr.Add(new Vector2Int(pos.x, pos.y - 1));
                    break;
                case 2:
                    posArr.Add(new Vector2Int(pos.x - 1, pos.y));
                    break;
                case 3:
                    posArr.Add(new Vector2Int(pos.x, pos.y + 1));
                    break;
                case 4:
                    posArr.Add(new Vector2Int(pos.x + 1, pos.y));
                    break;
            }
        else
            switch (info.dir)
            {
                case 1:
                    posArr.Add(new Vector2Int(pos.x, pos.y - 1));
                    posArr.Add(new Vector2Int(pos.x, pos.y - 2));
                    break;
                case 2:
                    posArr.Add(new Vector2Int(pos.x - 1, pos.y));
                    posArr.Add(new Vector2Int(pos.x - 2, pos.y));
                    break;
                case 3:
                    posArr.Add(new Vector2Int(pos.x, pos.y + 1));
                    posArr.Add(new Vector2Int(pos.x, pos.y + 2));
                    break;
                case 4:
                    posArr.Add(new Vector2Int(pos.x + 1, pos.y));
                    posArr.Add(new Vector2Int(pos.x + 2, pos.y));
                    break;
            }

        switch (info.dir)
        {
            case 1:
                transform.localEulerAngles = new Vector3(0, 0, 0);
                break;
            case 2:
                transform.localEulerAngles = new Vector3(0, 90, 0);
                break;
            case 3:
                transform.localEulerAngles = new Vector3(0, 180, 0);
                break;
            case 4:
                transform.localEulerAngles = new Vector3(0, -90, 0);
                break;
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
    //显示气球
    public void ShowBalloon()
    {
        m_Balloon.SetActive(true);
    }
    // 开始汽车震动动画
    public void StartCarScaleAnimation()
    {
        float scaleDuration = 0.1f; // 缩放动画的持续时间
        Vector3 minScale = transform.localScale * 0.99f; // 缩小到汽车本身的90%
        Vector3 maxScale = transform.localScale * 1.01f; // 放大到汽车本身的110%

        transform.DOScale(minScale, scaleDuration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo); // 无限循环，来回缩放
    }
}
