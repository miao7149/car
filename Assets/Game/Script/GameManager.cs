using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public UI m_UI;
    //游戏行动次数
    public int actionCount = 5;
    //暂定游戏尺寸 11*21 m
    private readonly int[][] roadDataArr = {
        //      00  01  02  03  04  05  06  07  08  09  10  11  12  13  14  15  16  17  18  19  20
        new[] { 00, 00, 00, 00, 00, 00, 01, 00, 00, 00, 12, 08, 10, 00, 00, 12, 10, 00, 00, 00, 00 }, //0
        new[] { 00, 00, 21, 15, 20, 00, 01, 00, 07, 00, 12, 08, 10, 00, 00, 12, 10, 00, 00, 00, 00 }, //1
        new[] { 00, 00, 12, 08, 10, 00, 01, 00, 01, 00, 31, 27, 30, 00, 00, 12, 10, 00, 00, 00, 00 }, //2
        new[] { 02, 02, 03, 03, 03, 02, 03, 02, 03, 02, 03, 03, 03, 02, 02, 03, 03, 02, 02, 02, 02 }, //3
        new[] { 00, 00, 12, 08, 10, 00, 01, 00, 01, 00, 12, 08, 10, 00, 00, 12, 10, 00, 00, 00, 00 }, //4
        new[] { 00, 00, 25, 17, 24, 00, 01, 00, 05, 00, 12, 08, 10, 00, 00, 12, 10, 00, 00, 00, 00 }, //5
        new[] { 00, 00, 00, 00, 00, 00, 01, 00, 00, 00, 12, 08, 10, 00, 00, 12, 10, 00, 00, 00, 00 }, //6
        new[] { 00, 00, 00, 00, 18, 13, 03, 13, 13, 13, 03, 03, 03, 28, 13, 03, 03, 13, 13, 13, 13 }, //7
        new[] { 00, 00, 00, 00, 14, 09, 03, 09, 09, 09, 03, 03, 03, 29, 11, 03, 03, 11, 11, 11, 11 }, //8
        new[] { 00, 00, 00, 00, 19, 11, 03, 11, 11, 11, 03, 08, 10, 00, 00, 12, 10, 00, 00, 00, 00 }, //9
        new[] { 00, 00, 00, 00, 00, 00, 01, 00, 00, 00, 12, 08, 10, 00, 00, 12, 10, 00, 00, 00, 00 } //10
    };

    //目前车有2m 和 3m 茅点为车头
    private CarInfo[] carDataArr = {
        new(CarType.Small, 8, 7, 1, 1), //x，y，方向，转弯方向
        new(CarType.Small, 7, 6, 1, 4),
        new(CarType.Small, 3, 16, 2, 1),
        new(CarType.Small, 2, 16, 2, 1),
        new(CarType.Bulldozer, 5, 12, 2, 2),
    };


    //人数据
    private PeoInfoItem[] peopleDataArr = {
        new PeoInfoItem(new IntPos(9, 13), new IntPos(6, 13)),
        new PeoInfoItem(new IntPos(2, 9), new IntPos(2, 13))
    };
    //信号灯数据
    private TrafficLightInfo[] trafficLightDataArr = {
        new(new IntPos(4, 9), 4),
    };
    //路障数据
    private BlockInfo[] blockDataArr = {
        new BlockInfo(new IntPos(7, 17)),
    };
    public GameObject[] prefabRoadArr;

    public GameObject m_PrefabSmallCar;//小车预制体
    public GameObject m_PrefabBigCar; //大车预制体
    public GameObject m_PrefabBulldozer;//推土机预制体

    public GameObject prefabPeople;
    public GameObject prefabTrafficLight;
    public GameObject prefabBlock;


    private List<RoadItem> roadArr = new();
    private List<RoadItem> roadPool = new();
    private List<Car> carPool = new();
    private List<Car> carArr = new();

    private List<People> peopleArr = new();
    private List<People> peoplePool = new();

    public GameStatu gameStatu;//游戏状态
    public FailReason failReason;//失败原因
    public bool IsUseItem = false; //是否使用道具
    public Car hitPeopleCar = null;//撞到行人的车

    private void Start()
    {
        Instance = this;
        m_UI.ChangeItemCount(GlobalManager.Instance.ItemCount);
        InitGame();
    }
    public void InitGame()
    {
        SetGameStatu(GameStatu.preparing);
        CleanGame();
        //初始化路
        for (var i = 0; i < roadDataArr.Length; i++)
            for (var j = 0; j < roadDataArr[i].Length; j++)
                if (roadDataArr[i][j] != 0)
                {
                    var road = BornRoad(roadDataArr[i][j]);
                    road.transform.localPosition = ConvertPos(new Vector3(i, 0, j));
                    road.transform.SetParent(transform, false);
                    road.gameObject.SetActive(true);
                    roadArr.Add(road);
                }

        //初始化车
        foreach (var carInfo in carDataArr)
        {
            var car = BornCar(carInfo.type);
            car.Init(carInfo);
            car.transform.SetParent(transform, false);
            car.gameObject.SetActive(true);
            car.transform.localPosition = ConvertPos(new Vector3(carInfo.posX, 0, carInfo.posY));
            carArr.Add(car);
        }
        //初始化人
        foreach (var item in peopleDataArr)
        {
            var p = BornPeople();
            peopleArr.Add(p);
            p.gameObject.SetActive(true);
            p.transform.SetParent(transform, false);
            var p1 = ConvertPos(new Vector3(item.pos1.x, 0, item.pos1.y));
            var p2 = ConvertPos(new Vector3(item.pos2.x, 0, item.pos2.y));
            p.transform.localPosition = p1;
            p.action = DOTween.Sequence();
            p.action.Append(p.transform.DOLocalMove(p2, 4));
            p.action.Append(p.transform.DOLocalMove(p1, 4));
            p.action.SetLoops(-1, LoopType.Restart);
            p.action.Play();
            p.Init();
        }
        //初始化信号灯
        foreach (var item in trafficLightDataArr)
        {
            var light = Instantiate(prefabTrafficLight).GetComponent<TrafficLight>();
            light.transform.SetParent(transform, false);
            light.transform.localPosition = ConvertPos(new Vector3(item.pos.x, 0, item.pos.y));
            light.Init(item);
        }
        //初始化路障
        foreach (var item in blockDataArr)
        {
            var block = Instantiate(prefabBlock).GetComponent<Block>();
            block.transform.SetParent(transform, false);
            block.transform.localPosition = ConvertPos(new Vector3(item.pos.x, 0, item.pos.y));
        }
        //初始化行动次数
        actionCount = 5;
        m_UI.ChangeActionCount(actionCount);
        StartCoroutine(PlayStartAni());
    }

    private void OnDestroy()
    {
        //GlobalManager.Instance.SaveGameData();
    }
    private void Update()
    {
        if (gameStatu == GameStatu.playing && Input.touchCount > 0 && actionCount > 0)
        {
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                // 将触摸点转换为世界坐标
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                // 射线投射
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("car")))
                {
                    // 检查射线是否击中物体
                    if (hit.collider != null)
                    {
                        TriggerVibration();
                        if (IsUseItem) //使用道具
                            StartCoroutine(HelicopterFlyAnimation(hit.collider.transform.parent.parent.GetComponent<Car>().transform));
                        else
                            OnTouchCar(hit.collider.transform.parent.parent.GetComponent<Car>());
                    }
                }
            }
        }
    }
    IEnumerator HelicopterFlyAnimation(Transform car)
    {
        Vector3 startPosition = car.position; // 起始位置
        Vector3 endPosition = Camera.main.transform.position + Camera.main.transform.forward * 2 + Vector3.right * 4f; // 结束位置
        Vector3 controlPoint = startPosition + (endPosition - startPosition) / 2 + Vector3.left * 10f; // 控制点，用于定义弧线

        float startTime = Time.time;

        while (Time.time - startTime < 3)
        {
            float t = (Time.time - startTime) / 3;
            Vector3 m1 = Vector3.Lerp(startPosition, controlPoint, t);
            Vector3 m2 = Vector3.Lerp(controlPoint, endPosition, t);
            car.position = Vector3.Lerp(m1, m2, t); // 更新直升飞机的位置

            // 更新直升飞机的旋转，使其面向下一个点
            if (t < 1f) // 防止在动画结束时计算方向时出现除以0的情况
            {
                Vector3 nextPosition = Vector3.Lerp(Vector3.Lerp(startPosition, controlPoint, t + 0.01f), Vector3.Lerp(controlPoint, endPosition, t + 0.01f), t + 0.01f);
                car.rotation = Quaternion.LookRotation(nextPosition - car.position);
            }

            yield return null;
        }

        car.position = endPosition; // 确保动画结束时，直升飞机正好在结束位置
        IsUseItem = false;
        GlobalManager.Instance.ItemCount--;//道具数量减一
        GlobalManager.Instance.SaveGameData();//保存数据
        m_UI.ChangeItemCount(GlobalManager.Instance.ItemCount);//更新道具数量UI
        DeleteCar(car.GetComponent<Car>());//删除车
    }
    List<Vector2Int> posArr = new List<Vector2Int>();
    int PosArrIndex = 0;
    Car CurrentCar;
    public void OnTouchCar(Car car)
    {
        Debug.Log("touchCar");
        if (car.dead) return;
        CurrentCar = car;
        posArr.Clear();
        posArr.Add(car.pos);
        Vector2Int dirction = Vector2Int.zero;
        Vector2Int nextStep = Vector2Int.zero;
        bool hitcar = false;
        bool outmap = false;
        bool hitBlock = false;
        int hitCarIndex = 0;
        int turnCount = 0;
        switch (car.turn)
        {
            case 1: //直线
                dirction = car.dir;
                nextStep = car.pos + dirction;
                do
                {
                    outmap = PosOutMap(nextStep);
                    if (outmap == false)
                    {
                        hitcar = HasCarOnPos(nextStep, out hitCarIndex);
                        hitBlock = HasBlockOnPos(nextStep, car);
                        if (hitcar || hitBlock)
                        {
                            posArr.Add(nextStep);
                        }
                    }
                    else
                    {
                        posArr.Add(dirction * 10 + nextStep);
                    }

                    nextStep = nextStep + dirction;
                } while (hitcar == false && outmap == false && hitBlock == false);

                break;
            case 2: //左转
                turnCount = 1;
                dirction = car.dir;
                nextStep = car.pos + dirction;
                do
                {
                    outmap = PosOutMap(nextStep);
                    if (outmap == false)
                    {
                        hitcar = HasCarOnPos(nextStep, out hitCarIndex);
                        hitBlock = HasBlockOnPos(nextStep, car);
                        if (hitcar || hitBlock)
                        {
                            posArr.Add(nextStep);
                        }
                    }
                    else
                    {
                        posArr.Add(dirction * 10 + nextStep);
                    }

                    if (turnCount == 0)
                    {
                        nextStep = nextStep + dirction;
                    }
                    else
                    {
                        switch (roadDataArr[nextStep.x][nextStep.y])
                        {
                            case 3:
                            case 4:
                            case 5:
                            case 6:
                            case 7:
                                posArr.Add(nextStep);
                                dirction = new Vector2Int(-dirction.y, dirction.x);
                                nextStep = nextStep + dirction;
                                turnCount--;
                                break;
                            default:
                                nextStep = nextStep + dirction;
                                break;
                        }
                    }
                } while (hitcar == false && outmap == false && hitBlock == false);

                break;
            case 3: //右转
                turnCount = 1;
                dirction = car.dir;
                nextStep = car.pos + dirction;
                do
                {
                    outmap = PosOutMap(nextStep);
                    if (outmap == false)
                    {
                        hitcar = HasCarOnPos(nextStep, out hitCarIndex);
                        hitBlock = HasBlockOnPos(nextStep, car);
                        if (hitcar || hitBlock)
                        {
                            posArr.Add(nextStep);
                        }
                    }
                    else
                    {
                        posArr.Add(dirction * 10 + nextStep);
                    }

                    if (turnCount == 0)
                    {
                        nextStep = nextStep + dirction;
                    }
                    else
                    {
                        switch (roadDataArr[nextStep.x][nextStep.y])
                        { //十字路口可以转
                            case 3:
                            case 4:
                            case 5:
                            case 6:
                            case 7:
                                posArr.Add(nextStep);
                                dirction = new Vector2Int(dirction.y, -dirction.x);
                                nextStep = nextStep + dirction;
                                turnCount--;
                                break;
                            default:
                                nextStep = nextStep + dirction;
                                break;
                        }
                    }
                } while (hitcar == false && outmap == false && hitBlock == false);

                break;
            case 4: //左掉头
                turnCount = 2;
                dirction = car.dir;
                nextStep = car.pos + dirction;
                int tempStep = 0; //转弯后走的距离,防止原地掉头
                do
                {
                    outmap = PosOutMap(nextStep);
                    if (outmap == false)
                    {
                        hitcar = HasCarOnPos(nextStep, out hitCarIndex);
                        hitBlock = HasBlockOnPos(nextStep, car);
                        if (hitcar || hitBlock)
                        {
                            posArr.Add(nextStep);
                        }
                    }
                    else
                    {
                        posArr.Add(dirction * 10 + nextStep);
                    }

                    if (turnCount == 0)
                    {
                        nextStep = nextStep + dirction;
                    }
                    else
                    {
                        switch (roadDataArr[nextStep.x][nextStep.y])
                        {
                            case 3:
                            case 4:
                            case 5:
                            case 6:
                            case 7:
                                if (tempStep > 0)
                                {
                                    tempStep--;
                                    nextStep = nextStep + dirction;
                                }
                                else
                                {
                                    posArr.Add(nextStep);
                                    dirction = new Vector2Int(-dirction.y, dirction.x);
                                    nextStep = nextStep + dirction;
                                    turnCount--;
                                    tempStep = 1;
                                }

                                break;
                            default:
                                if (tempStep > 0)
                                {
                                    tempStep--;
                                }

                                nextStep = nextStep + dirction;
                                break;
                        }
                    }
                } while (hitcar == false && outmap == false && hitBlock == false);

                break;
            case 5: //右掉头
                turnCount = 2;
                dirction = car.dir;
                nextStep = car.pos + dirction;
                int tempStepp = 0; //转弯后走的距离,防止原地掉头
                do
                {
                    outmap = PosOutMap(nextStep);//是否出界
                    if (outmap == false)
                    {
                        hitcar = HasCarOnPos(nextStep, out hitCarIndex);
                        hitBlock = HasBlockOnPos(nextStep, car);
                        if (hitcar || hitBlock)
                        {
                            posArr.Add(nextStep);
                        }
                    }
                    else
                    {
                        posArr.Add(dirction * 10 + nextStep);
                    }

                    if (turnCount == 0)
                    {
                        nextStep = nextStep + dirction;
                    }
                    else
                    {
                        switch (roadDataArr[nextStep.x][nextStep.y])
                        {
                            case 3:
                            case 4:
                            case 5:
                            case 6:
                            case 7:
                                if (tempStepp > 0)
                                {
                                    tempStepp--;
                                    nextStep = nextStep + dirction;
                                }
                                else
                                {
                                    posArr.Add(nextStep);
                                    dirction = new Vector2Int(dirction.y, -dirction.x);
                                    nextStep = nextStep + dirction;
                                    turnCount--;
                                    tempStepp = 1;
                                }

                                break;
                            default:
                                if (tempStepp > 0)
                                {
                                    tempStepp--;
                                }

                                nextStep = nextStep + dirction;
                                break;
                        }
                    }
                } while (hitcar == false && outmap == false && hitBlock == false);

                break;
        }

        if (outmap)
        {
            //carArr.Remove(car);
            car.dead = true;
            car.posArr.Clear();
            int index = 1;
            car.moveAction = car.transform.DOLocalMove(ConvertPos(new Vector3(posArr[index].x, 0, posArr[index].y)), getDuring(speedCarSmall, posArr[index - 1], posArr[index])).OnComplete(() =>
            {
                index++;
                if (index < posArr.Count)
                {
                    car.transform.DOLocalRotateQuaternion(Quaternion.LookRotation(new Vector3(posArr[index].x, 0, posArr[index].y) - new Vector3(posArr[index - 1].x, 0, posArr[index - 1].y)), speedCarRotate).Play();
                    //car.transform.LookAt(ConvertPos(new Vector3(posArr[index].x, 0, posArr[index].y)));
                    car.moveAction = car.transform.DOLocalMove(ConvertPos(new Vector3(posArr[index].x, 0, posArr[index].y)), getDuring(speedCarSmall, posArr[index - 1], posArr[index])).OnComplete(() =>
                    {
                        index++;
                        if (index < posArr.Count)
                        {
                            car.transform.DOLocalRotateQuaternion(Quaternion.LookRotation(new Vector3(posArr[index].x, 0, posArr[index].y) - new Vector3(posArr[index - 1].x, 0, posArr[index - 1].y)), speedCarRotate).Play();
                            //car.transform.LookAt(ConvertPos(new Vector3(posArr[index].x, 0, posArr[index].y)));
                            car.moveAction = car.transform.DOLocalMove(ConvertPos(new Vector3(posArr[index].x, 0, posArr[index].y)), getDuring(speedCarSmall, posArr[index - 1], posArr[index])).OnComplete(() =>
                            {
                                DeleteCar(car);
                            }).Play();
                        }
                        else
                        {
                            DeleteCar(car);
                        }
                    }).Play();
                }
                else
                {
                    DeleteCar(car);
                }
            }).Play();
        }
        else if (hitcar || hitBlock)
        {
            SetGameStatu(GameStatu.waiting);
            PosArrIndex = 1;
            car.moveAction = car.transform.DOLocalMove(ConvertPos(new Vector3(posArr[PosArrIndex].x, 0, posArr[PosArrIndex].y)), getDuring(speedCarSmall, posArr[PosArrIndex - 1], posArr[PosArrIndex])).OnComplete(() =>
            {
                PosArrIndex++;
                if (PosArrIndex < posArr.Count)
                {
                    car.transform.DOLocalRotateQuaternion(Quaternion.LookRotation(new Vector3(posArr[PosArrIndex].x, 0, posArr[PosArrIndex].y) - new Vector3(posArr[PosArrIndex - 1].x, 0, posArr[PosArrIndex - 1].y)), speedCarRotate).Play();
                    // car.transform.LookAt(ConvertPos(new Vector3(posArr[index].x, 0, posArr[index].y)));
                    car.moveAction = car.transform.DOLocalMove(ConvertPos(new Vector3(posArr[PosArrIndex].x, 0, posArr[PosArrIndex].y)), getDuring(speedCarSmall, posArr[PosArrIndex - 1], posArr[PosArrIndex])).OnComplete(() =>
                    {
                        PosArrIndex++;
                        if (PosArrIndex < posArr.Count)
                        {
                            car.transform.DOLocalRotateQuaternion(Quaternion.LookRotation(new Vector3(posArr[PosArrIndex].x, 0, posArr[PosArrIndex].y) - new Vector3(posArr[PosArrIndex - 1].x, 0, posArr[PosArrIndex - 1].y)), speedCarRotate).Play();
                            //         car.transform.LookAt(ConvertPos(new Vector3(posArr[index].x, 0, posArr[index].y)));
                            car.moveAction = car.transform.DOLocalMove(ConvertPos(new Vector3(posArr[PosArrIndex].x, 0, posArr[PosArrIndex].y)), getDuring(speedCarSmall, posArr[PosArrIndex - 1], posArr[PosArrIndex])).OnComplete(() =>
                            {
                                PosArrIndex++;
                                if (hitcar)
                                {
                                    HitCar(carArr[hitCarIndex], dirction);
                                }
                                backCar();
                            }).Play();
                        }
                        else
                        {
                            if (hitcar)
                            {
                                HitCar(carArr[hitCarIndex], dirction);
                            }
                            backCar();
                        }
                    }).Play();
                }
                else
                {
                    if (hitcar)
                    {
                        HitCar(carArr[hitCarIndex], dirction);
                    }
                    backCar();
                }
            }).Play();

        }
    }

    public float speedBackCar = 30f;
    //public float speedBackRotate = 0.3f;

    public float speedCarSmall = 15f;
    public float speedCarBig = 10f;

    public float speedCarRotate = 0.05f;

    void backCar()
    {
        PosArrIndex -= 2;//回退两步

        var durning = getDuring(speedBackCar, posArr[PosArrIndex], posArr[PosArrIndex + 1]);//回退时间
        if (PosArrIndex > 0)
        {
            CurrentCar.transform.DOLocalRotateQuaternion(Quaternion.LookRotation(new Vector3(posArr[PosArrIndex].x, 0, posArr[PosArrIndex].y) - new Vector3(posArr[PosArrIndex - 1].x, 0, posArr[PosArrIndex - 1].y)), durning).Play();
        }

        CurrentCar.transform.DOLocalMove(ConvertPos(new Vector3(posArr[PosArrIndex].x, 0, posArr[PosArrIndex].y)), durning).OnComplete(() =>
        {
            if (PosArrIndex == 0)
            {
                if (actionCount > 0)
                    SetGameStatu(GameStatu.playing);
            }
            else
            {
                PosArrIndex--;

                durning = getDuring(speedBackCar, posArr[PosArrIndex], posArr[PosArrIndex + 1]);
                if (PosArrIndex > 0)
                {
                    CurrentCar.transform.DOLocalRotateQuaternion(Quaternion.LookRotation(new Vector3(posArr[PosArrIndex].x, 0, posArr[PosArrIndex].y) - new Vector3(posArr[PosArrIndex - 1].x, 0, posArr[PosArrIndex - 1].y)), durning).Play();
                }

                CurrentCar.transform.DOLocalMove(ConvertPos(new Vector3(posArr[PosArrIndex].x, 0, posArr[PosArrIndex].y)), durning).OnComplete(() =>
                {
                    if (PosArrIndex == 0)
                    {
                        if (actionCount > 0)
                            SetGameStatu(GameStatu.playing);
                    }
                    else
                    {
                        PosArrIndex--;
                        CurrentCar.transform.DOLocalMove(ConvertPos(new Vector3(posArr[PosArrIndex].x, 0, posArr[PosArrIndex].y)), getDuring(speedBackCar, posArr[PosArrIndex], posArr[PosArrIndex + 1])).OnComplete(() =>
                        {
                            if (actionCount > 0)
                                SetGameStatu(GameStatu.playing);
                        }).Play();
                    }
                }).Play();
            }
        }).Play();
        ChangeActionCount(-1);//行动次数减一
    }
    public void HitCar(Car car, Vector2Int dir)
    {
        var pos = car.transform.localPosition;
        car.transform.DOLocalMove(new Vector3(dir.x * 0.5f, 0, dir.y * 0.5f) + pos, 0.2f).OnComplete(() =>
        {
            car.transform.DOLocalMove(pos, 0.2f).Play();
        }).Play();
    }

    public void DeleteCar(Car car)
    {
        carArr.Remove(car);
        car.transform.SetParent(null, false);
        car.gameObject.SetActive(false);
        carPool.Add(car);
        ChangeActionCount(-1);//行动次数减一
        if (carArr.Count == 0)
        {
            SetGameStatu(GameStatu.finish);
        }
    }

    public float getDuring(float speed, Vector2Int p1, Vector2Int p2)
    {
        return Vector2Int.Distance(p1, p2) / speed;
    }

    public bool PosOutMap(Vector2Int pos) //判断是否出界
    {
        return pos.x < 0 || pos.x > 10 || pos.y > 20 || pos.y < 0;
    }

    public bool HasCarOnPos(Vector2Int pos, out int carIndex) //判断是否有车
    {
        for (int i = 0; i < carArr.Count; i++)
        {
            var car = carArr[i];
            foreach (var p in car.posArr)
            {
                if (p == pos)
                {
                    carIndex = i;
                    return true;
                }
            }
        }

        carIndex = 0;
        return false;
    }
    public bool HasBlockOnPos(Vector2Int pos, Car car)//判断是否有路障
    {
        foreach (var block in blockDataArr)
        {
            if (block.pos.x == pos.x && block.pos.y == pos.y)
            {
                if (car.type == CarType.Bulldozer)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        return false;
    }
    public void CleanGame()
    {
        foreach (var o in roadArr)
        {
            o.transform.SetParent(null, false);
            o.gameObject.SetActive(false);
            roadPool.Add(o);
        }

        roadArr.Clear();

        foreach (var car in carArr)
        {
            car.transform.SetParent(null, false);
            car.gameObject.SetActive(false);
            carPool.Add(car);
        }

        carArr.Clear();

        foreach (var people in peopleArr)
        {
            people.transform.SetParent(null, false);
            people.gameObject.SetActive(false);
            peoplePool.Add(people);
            if (people.action.IsPlaying())
                people.action.Kill();
        }

        peopleArr.Clear();
    }

    public void SetGameStatu(GameStatu statu)
    {
        gameStatu = statu;
        if (statu == GameStatu.faled)
        {
            if (failReason == FailReason.PeopleCrash)
            {
                m_UI.ShowCarHitPeopleRoot(true);
            }
            else if (failReason == FailReason.ActionNotEnough)
            {
                m_UI.ShowActionOverRoot(true);
            }
        }
        else if (statu == GameStatu.finish)
        {
            m_UI.ShowFinishRoot(true);
        }
    }

    IEnumerator PlayStartAni()
    {
        foreach (var car in carArr)
        {
            car.transform.localScale = Vector3.zero;
            car.transform.DOScale(Vector3.one, 2f).SetEase(Ease.OutBack).Play();
        }

        yield return new WaitForSeconds(2f);
        SetGameStatu(GameStatu.playing);
    }

    //左下角坐标 转 中心坐标
    public Vector3 ConvertPos(Vector3 p)
    {
        return new Vector3(p.x - 5, 0, p.z - 10);
    }

    public Car BornCar(CarType type)
    {
        Car result = null;
        for (int i = 0; i < carPool.Count; i++)
        {
            if (carPool[i].type == type)
            {
                result = carPool[i];
                carPool.RemoveAt(i);
                break;
            }
        }

        if (result == null)
        {
            switch (type)
            {
                case CarType.Small:
                    return Instantiate(m_PrefabSmallCar).GetComponent<Car>();
                case CarType.Big:
                    return Instantiate(m_PrefabBigCar).GetComponent<Car>();
                case CarType.Bulldozer:
                    return Instantiate(m_PrefabBulldozer).GetComponent<Car>();
                default:
                    return null;
            }
        }
        else
        {
            return result;
        }
    }

    public RoadItem BornRoad(int type)
    {
        RoadItem result = null;
        for (int i = 0; i < roadPool.Count; i++)
        {
            if (roadPool[i].type == type)
            {
                result = roadPool[i];
                roadPool.RemoveAt(i);
                break;
            }
        }

        if (result == null)
        {
            return Instantiate(prefabRoadArr[type - 1]).GetComponent<RoadItem>();
        }
        else
        {
            return result;
        }
    }
    public People BornPeople()
    {
        People result = null;
        if (peoplePool.Count > 0)
        {
            result = peoplePool[0];
            peoplePool.RemoveAt(0);
            return result;
        }
        else
        {
            return Instantiate(prefabPeople).GetComponent<People>();
        }
    }
    //行动次数修改
    public void ChangeActionCount(int count)
    {
        actionCount += count;
        m_UI.ChangeActionCount(actionCount);
        if (actionCount <= 0 && carArr.Count > 1)
        {
            failReason = FailReason.ActionNotEnough;
            SetGameStatu(GameStatu.faled);
            Debug.Log("游戏失败" + gameStatu);
        }
    }
    //继续游戏
    public void ContinueGame()
    {
        SetGameStatu(GameStatu.playing);
        if (failReason == FailReason.PeopleCrash)
        {
            ResetCar();
            m_UI.ShowCarHitPeopleRoot(false);
        }
        else if (failReason == FailReason.ActionNotEnough)
        {
            ChangeActionCount(5);
            m_UI.ShowActionOverRoot(false);
        }
    }
    //重置车辆
    public void ResetCar()
    {
        if (hitPeopleCar != null)
        {
            foreach (var item in carDataArr)
            {
                if (item.type == hitPeopleCar.type && item.posX == hitPeopleCar.pos.x && item.posY == hitPeopleCar.pos.y)
                {
                    hitPeopleCar.Init(item);
                    hitPeopleCar.transform.localPosition = ConvertPos(new Vector3(item.posX, 0, item.posY));
                    hitPeopleCar.gameObject.SetActive(true);
                    break;
                }
            }
        }
    }
    //震动
    private void TriggerVibration()
    {
        Handheld.Vibrate();
    }
}
