using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Car : MonoBehaviour {
    public int type; //车型 1-2m小车   2-3m大车
    public Vector2Int pos; //车头位置

    public Vector2Int dir; //方向 1上 2右  3下  4左
    public int turn; //转向 1直行 2左转 3右转 4左掉头 5右掉头

    public List<Vector2Int> posArr = new();


    public bool dead = false;


    public Tweener moveAction = null;

    public void Init(CarInfo info) {
        dead = false;
        posArr.Clear();

        type = info.type;
        switch (info.dir) {
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
        transform.localPosition = GameManager.Instance.ConvertPos(new Vector3(pos.x, 0, pos.y));

        if (type == 1)
            switch (info.dir) {
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
            switch (info.dir) {
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

        switch (info.dir) {
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
}
