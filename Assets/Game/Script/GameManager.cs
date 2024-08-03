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
        //      00  01  02  03  04  05  06  07  08  09  10  11  12  13  14  15  16  17  18  19  20  21  22  23  24  25  26  27
        new[] { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 01, 00, 00, 01, 00, 00, 01, 00, 00, 01, 00, 00, 00, 00, 00, 00, 00, 00 }, //0
        new[] { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 01, 00, 00, 01, 00, 00, 01, 00, 00, 01, 00, 00, 00, 00, 00, 00, 00, 00 }, //1
        new[] { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 01, 00, 00, 01, 00, 00, 01, 00, 00, 01, 00, 00, 00, 00, 00, 00, 00, 00 }, //2
        new[] { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 01, 00, 00, 01, 00, 00, 01, 00, 00, 01, 00, 00, 00, 00, 00, 00, 00, 00 }, //3
        new[] { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 01, 00, 00, 01, 00, 00, 01, 00, 00, 01, 00, 00, 00, 00, 00, 00, 00, 00 }, //4
        new[] { 02, 02, 02, 02, 02, 02, 02, 02, 02, 02, 03, 02, 02, 03, 02, 02, 37, 02, 02, 03, 02, 02, 02, 02, 02, 02, 02, 02 }, //5
        new[] { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 01, 00, 00, 01, 00, 00, 00, 00, 00, 01, 00, 00, 00, 00, 00, 00, 00, 00 }, //6
        new[] { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 01, 00, 00, 01, 00, 00, 00, 00, 00, 01, 00, 00, 00, 00, 00, 00, 00, 00 }, //7
        new[] { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 01, 00, 00, 01, 00, 00, 00, 00, 00, 01, 00, 00, 00, 00, 00, 00, 00, 00 }, //8
        new[] { 02, 02, 02, 02, 02, 02, 02, 33, 02, 02, 03, 02, 02, 03, 02, 02, 35, 02, 02, 03, 02, 02, 02, 02, 02, 02, 02, 02 }, //9
        new[] { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 01, 00, 00, 01, 00, 00, 01, 00, 00, 01, 00, 00, 00, 00, 00, 00, 00, 00 }, //10
        new[] { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 32, 00, 00, 01, 00, 00, 01, 00, 00, 01, 00, 00, 00, 00, 00, 00, 00, 00 }, //11
        new[] { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 01, 00, 00, 01, 00, 00, 01, 00, 00, 01, 00, 00, 00, 00, 00, 00, 00, 00 }, //12
        new[] { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 01, 00, 00, 01, 00, 00, 01, 00, 00, 01, 00, 00, 00, 00, 00, 00, 00, 00 }, //13
        new[] { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 01, 00, 00, 01, 00, 00, 01, 00, 00, 01, 00, 00, 00, 00, 00, 00, 00, 00 }, //14
    };

    //目前车有2m 和 3m 茅点为车头
    private CarInfo[] carDataArr = {
        new(CarType.Bulldozer, 8, 19, 2, 1), //x，y，方向，转弯方向
        new(CarType.Small, 5, 20, 3, 2),
        new(CarType.Small, 9, 20, 3, 5),
        new(CarType.Small, 5, 18, 1, 3),
        new(CarType.Small, 9, 18, 1, 3),
        new(CarType.Small, 4, 19, 2, 2),
        new(CarType.Small, 4, 16, 2, 2),
        new(CarType.Small, 3, 13, 4, 1),
        new(CarType.Small, 4, 10, 2, 4),
        new(CarType.Small, 10, 16, 4, 3),
        new(CarType.Small, 10, 13, 4, 4),
        new(CarType.Big, 8, 10, 2, 4),
        new(CarType.Big, 8, 13, 2, 3),
        new(CarType.Small, 5, 9, 1, 3),
        new(CarType.Small, 9, 14, 3, 5),
        new(CarType.Small, 9, 11, 3, 1),
        new(CarType.Small, 9, 9, 1, 3),
        new(CarType.Small, 5, 15, 1, 2),
        new(CarType.Small, 5, 12, 1, 5),
        //new(CarType.Bulldozer, 5, 12, 2, 2),
    };


    //人数据
    private PeoInfoItem[] peopleDataArr = {
        new PeoInfoItem(new IntPos(8,7), new IntPos(10, 7)),
        new PeoInfoItem(new IntPos(11, 9), new IntPos(11, 11))
    };
    //信号灯数据
    private TrafficLightInfo[] trafficLightDataArr = {
        new(new IntPos(10, 9), 1),
    };
    //路障数据
    private BlockInfo[] blockDataArr = {
        new BlockInfo(new IntPos(9, 19)),
    };
    public GameObject[] prefabRoadArr;//道路预制体

    public GameObject m_PrefabSmallCar;//小车预制体
    public GameObject m_PrefabBigCar; //大车预制体
    public GameObject m_PrefabBulldozer;//推土机预制体

    public GameObject prefabPeople;//行人预制体
    public GameObject prefabTrafficLight;//信号灯预制体
    public GameObject prefabBlock;//路障预制体


    private List<RoadItem> roadArr = new();
    private List<RoadItem> roadPool = new();
    private List<Car> carPool = new();
    private List<Car> carArr = new();

    private List<People> peopleArr = new();
    private List<People> peoplePool = new();

    private List<Block> blocksArr = new();

    private List<TrafficLight> trafficLightArr = new();

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
            p.Init(item);
        }
        //初始化信号灯
        foreach (var item in trafficLightDataArr)
        {
            var light = Instantiate(prefabTrafficLight).GetComponent<TrafficLight>();
            light.transform.SetParent(transform, false);
            light.transform.localPosition = ConvertPos(new Vector3(item.pos.x, 0, item.pos.y));
            light.Init(item);
            trafficLightArr.Add(light);
        }
        //初始化路障
        foreach (var item in blockDataArr)
        {
            var block = Instantiate(prefabBlock).transform.GetChild(0).GetChild(0).GetComponent<Block>();
            block.Init(item);
            blocksArr.Add(block);
        }
        //初始化行动次数
        actionCount = 20;
        m_UI.ChangeActionCount(actionCount);
        StartCoroutine(PlayStartAni());
    }
    IEnumerator PlayStartAni()
    {
        foreach (var car in carArr)
        {
            car.transform.localScale = Vector3.zero;
            car.transform.DOScale(Vector3.one, 2f).SetEase(Ease.OutBack).Play();
        }
        AudioManager.Instance.PlayGameStart();
        yield return new WaitForSeconds(2f);
        SetGameStatu(GameStatu.playing);
        foreach (var car in carArr)
        {
            car.StartCarScaleAnimation();
        }
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
            if (touch.phase == TouchPhase.Began && !Helper.IsPointerOverUIElement())
            {
                // 将触摸点转换为世界坐标
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                // 射线投射
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("car")))
                {
                    // 检查射线是否击中物体
                    if (hit.collider != null)
                    {
                        VibrationManager.Vibrate(30, 180);
                        if (IsUseItem) //使用道具
                                       //UseItem(hit.collider.transform.parent.parent.GetComponent<Car>().transform);
                            LiftCarWithBalloon(hit.collider.transform.parent.parent.GetComponent<Car>().transform);
                        else
                            OnTouchCar(hit.collider.transform.parent.parent.GetComponent<Car>());
                    }
                }
            }
        }
    }
    //获取推土机路径上的石头
    public Block GetBulldozerPathBlock(Car car)
    {
        Vector2Int dirction = Vector2Int.zero;
        Vector2Int nextStep = Vector2Int.zero;
        bool outmap = false;
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
                        foreach (var item in blocksArr)
                        {
                            if (item.pos == nextStep)
                            {
                                return item;
                            }
                        }
                    }
                    nextStep = nextStep + dirction;
                } while (outmap == false);

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
                        foreach (var item in blocksArr)
                        {
                            if (item.pos == nextStep)
                            {
                                return item;
                            }
                        }
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
                                dirction = new Vector2Int(-dirction.y, dirction.x);
                                nextStep = nextStep + dirction;
                                turnCount--;
                                break;
                            default:
                                nextStep = nextStep + dirction;
                                break;
                        }
                    }
                } while (outmap == false);

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
                        foreach (var item in blocksArr)
                        {
                            if (item.pos == nextStep)
                            {
                                return item;
                            }
                        }
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
                                dirction = new Vector2Int(dirction.y, -dirction.x);
                                nextStep = nextStep + dirction;
                                turnCount--;
                                break;
                            default:
                                nextStep = nextStep + dirction;
                                break;
                        }
                    }
                } while (outmap == false);

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
                        foreach (var item in blocksArr)
                        {
                            if (item.pos == nextStep)
                            {
                                return item;
                            }
                        }
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
                } while (outmap == false);

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
                        foreach (var item in blocksArr)
                        {
                            if (item.pos == nextStep)
                            {
                                return item;
                            }
                        }
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
                } while (outmap == false);

                break;
        }
        return null;
    }
    //使用气球道具
    void LiftCarWithBalloon(Transform car)
    {
        AudioManager.Instance.PlayBalloonFly();
        m_UI.ChangeItemCount(--GlobalManager.Instance.ItemCount);//更新道具数量UI
        Action carAction = () =>
      {
          GlobalManager.Instance.SaveGameData();//保存数据
          DeleteCar(car.GetComponent<Car>());//删除车
          IsUseItem = false;
      };
        car.GetComponent<Car>().ShowBalloon();//显示气球
        StartCoroutine(LiftCarCoroutine(car, carAction));
        if (car.GetComponent<Car>().type == CarType.Bulldozer)
        {
            var block = GetBulldozerPathBlock(car.GetComponent<Car>());
            Action blockAction = () =>
       {
           //删除路障
           blocksArr.Remove(block);
           Destroy(block.transform.parent.parent.gameObject);
       };
            if (block != null)
            {
                block.ShowBalloon();
                StartCoroutine(LiftCarCoroutine(block.transform.parent.parent, blockAction));
            }
        }
        // 启动协程处理汽车被气球拽起的逻辑
        //StartCoroutine(LiftCarCoroutine(car));
    }

    IEnumerator LiftCarCoroutine(Transform objTransform, Action callback)
    {
        // 获取汽车的初始位置
        Vector3 initialPosition = objTransform.position;

        // 将汽车的世界坐标转换为屏幕坐标
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(initialPosition);
        // 判断汽车在屏幕的哪一侧
        bool isOnRightSide = screenPosition.x > Screen.width / 2;

        // 设置目标位置
        Vector3 targetPosition = isOnRightSide ? new Vector3(-20, 20, initialPosition.z) : new Vector3(20, 20, initialPosition.z);

        // 设置拽起的时间
        float liftDuration = 3f;

        // 设置左右晃动的幅度和频率
        float swayAmplitude = 1f;//左右晃动的幅度
        float swayFrequency = 2.0f;//左右晃动的频率

        float elapsedTime = 0.0f;

        while (elapsedTime < liftDuration)
        {
            // 计算当前时间的比例
            float t = elapsedTime / liftDuration;

            // 计算汽车的当前位置
            Vector3 currentPosition = Vector3.Lerp(initialPosition, targetPosition, t);

            // 添加左右晃动效果
            currentPosition.x += Mathf.Sin(elapsedTime * swayFrequency) * swayAmplitude;

            // 更新汽车的位置
            objTransform.position = currentPosition;

            // 更新经过的时间
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // 确保汽车到达目标位置
        objTransform.position = targetPosition;
        callback?.Invoke();
    }

    //触发道具动画
    public void UseItem(Transform car)
    {
        m_UI.ChangeItemCount(--GlobalManager.Instance.ItemCount);//更新道具数量UI
        Action carAction = () =>
        {
            GlobalManager.Instance.SaveGameData();//保存数据
            DeleteCar(car.GetComponent<Car>());//删除车
            IsUseItem = false;
        };
        StartCoroutine(HelicopterFlyAnimation(car, carAction));
        if (car.GetComponent<Car>().type == CarType.Bulldozer)
        {
            var block = GetBulldozerPathBlock(car.GetComponent<Car>());
            Action blockAction = () =>
       {
           //删除路障
           blocksArr.Remove(block);
           Destroy(block.transform.parent.parent.gameObject);
       };
            if (block != null)
            {
                StartCoroutine(HelicopterFlyAnimation(block.transform.parent.parent, blockAction));
            }
        }
    }
    //直升飞机动画
    IEnumerator HelicopterFlyAnimation(Transform car, Action callback = null)
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
        callback?.Invoke();
    }

    public void OnTouchCar(Car car)
    {
        Debug.Log("touchCar");
        if (car.dead) return;
        List<Vector2Int> posArr = new List<Vector2Int>();
        int PosArrIndex = 0;
        Car CurrentCar = car;
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
                            case 34:
                            case 35:
                            case 36:
                            case 37:
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
                            case 34:
                            case 35:
                            case 36:
                            case 37:
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
                            case 34:
                            case 35:
                            case 36:
                            case 37:
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
                            case 34:
                            case 35:
                            case 36:
                            case 37:
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
        float speed = car.type == CarType.Small ? speedCarSmall : speedCarBig;//速度
        Debug.Log("speed" + speed);
        if (outmap)
        {
            if (car.type == CarType.Small) //播放音效
            {
                AudioManager.Instance.PlayCarSmallMove();
            }
            else
            {
                AudioManager.Instance.PlayCarBigMove();
            }
            //carArr.Remove(car);
            car.dead = true;
            car.posArr.Clear();
            int index = 1;
            car.moveAction = car.transform.DOLocalMove(ConvertPos(new Vector3(posArr[index].x, 0, posArr[index].y)), getDuring(speed, posArr[index - 1], posArr[index])).SetEase(Ease.Linear).OnComplete(() =>
            {
                index++;
                if (index < posArr.Count)
                {
                    car.transform.DOLocalRotateQuaternion(Quaternion.LookRotation(new Vector3(posArr[index].x, 0, posArr[index].y) - new Vector3(posArr[index - 1].x, 0, posArr[index - 1].y)), speedCarRotate).Play();
                    //car.transform.LookAt(ConvertPos(new Vector3(posArr[index].x, 0, posArr[index].y)));
                    car.moveAction = car.transform.DOLocalMove(ConvertPos(new Vector3(posArr[index].x, 0, posArr[index].y)), getDuring(speed, posArr[index - 1], posArr[index])).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        index++;
                        if (index < posArr.Count)
                        {
                            car.transform.DOLocalRotateQuaternion(Quaternion.LookRotation(new Vector3(posArr[index].x, 0, posArr[index].y) - new Vector3(posArr[index - 1].x, 0, posArr[index - 1].y)), speedCarRotate).Play();
                            //car.transform.LookAt(ConvertPos(new Vector3(posArr[index].x, 0, posArr[index].y)));
                            car.moveAction = car.transform.DOLocalMove(ConvertPos(new Vector3(posArr[index].x, 0, posArr[index].y)), getDuring(speed, posArr[index - 1], posArr[index])).SetEase(Ease.Linear).OnComplete(() =>
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
            car.moveAction = car.transform.DOLocalMove(ConvertPos(new Vector3(posArr[PosArrIndex].x, 0, posArr[PosArrIndex].y)), getDuring(speed, posArr[PosArrIndex - 1], posArr[PosArrIndex])).OnComplete(() =>
            {
                PosArrIndex++;
                if (PosArrIndex < posArr.Count)
                {
                    if (new Vector3(posArr[PosArrIndex].x, 0, posArr[PosArrIndex].y) - new Vector3(posArr[PosArrIndex - 1].x, 0, posArr[PosArrIndex - 1].y) != Vector3.zero)
                        car.transform.DOLocalRotateQuaternion(Quaternion.LookRotation(new Vector3(posArr[PosArrIndex].x, 0, posArr[PosArrIndex].y) - new Vector3(posArr[PosArrIndex - 1].x, 0, posArr[PosArrIndex - 1].y)), speedCarRotate).Play();
                    // car.transform.LookAt(ConvertPos(new Vector3(posArr[index].x, 0, posArr[index].y)));
                    car.moveAction = car.transform.DOLocalMove(ConvertPos(new Vector3(posArr[PosArrIndex].x, 0, posArr[PosArrIndex].y)), getDuring(speed, posArr[PosArrIndex - 1], posArr[PosArrIndex])).OnComplete(() =>
                    {
                        PosArrIndex++;
                        if (PosArrIndex < posArr.Count)
                        {
                            car.transform.DOLocalRotateQuaternion(Quaternion.LookRotation(new Vector3(posArr[PosArrIndex].x, 0, posArr[PosArrIndex].y) - new Vector3(posArr[PosArrIndex - 1].x, 0, posArr[PosArrIndex - 1].y)), speedCarRotate).Play();
                            //         car.transform.LookAt(ConvertPos(new Vector3(posArr[index].x, 0, posArr[index].y)));
                            car.moveAction = car.transform.DOLocalMove(ConvertPos(new Vector3(posArr[PosArrIndex].x, 0, posArr[PosArrIndex].y)), getDuring(speed, posArr[PosArrIndex - 1], posArr[PosArrIndex])).OnComplete(() =>
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
        ChangeActionCount(-1);//行动次数减一
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
        }
    }
    public float speedBackCar = 15f;
    //public float speedBackRotate = 0.3f;

    float speedCarSmall = 15;
    float speedCarBig = 10;

    float speedCarRotate = 0.05f;

    public void HitCar(Car car, Vector2Int dir)
    {
        AudioManager.Instance.PlayCarCrash();
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
        foreach (var block in blocksArr)
        {
            if (block.pos.x == pos.x && block.pos.y == pos.y)
            {
                if (car.type == CarType.Bulldozer)
                {
                    //删除路障
                    blocksArr.Remove(block);
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
        foreach (var block in blocksArr)
        {
            block.transform.SetParent(null, false);
            Destroy(block.gameObject);
        }
        blocksArr.Clear();
        foreach (var light in trafficLightArr)
        {
            light.transform.SetParent(null, false);
            Destroy(light.gameObject);
        }
        trafficLightArr.Clear();

    }

    public void SetGameStatu(GameStatu statu)
    {
        gameStatu = statu;
        if (statu == GameStatu.faled)
        {
            //延迟显示失败界面
            // 启动协程延迟显示失败界面
            StartCoroutine(ShowFailUIWithDelay());
        }
        else if (statu == GameStatu.finish)
        {
            m_UI.ShowFinishRoot(true);
            AudioManager.Instance.PlayVictory();
        }
    }
    private IEnumerator ShowFailUIWithDelay()
    {
        // 设置延迟时间
        float delay = 1.0f;
        yield return new WaitForSeconds(delay);
        AudioManager.Instance.PlayFail();
        // 根据失败原因显示相应的失败界面
        if (failReason == FailReason.PeopleCrash)
        {
            m_UI.ShowCarHitPeopleRoot(true);
        }
        else if (failReason == FailReason.ActionNotEnough)
        {
            m_UI.ShowActionOverRoot(true);
        }
    }

    //左下角坐标 转 中心坐标
    public Vector3 ConvertPos(Vector3 p)
    {
        return new Vector3(p.x - 7, 0, p.z - 13.5f);
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
        if (peoplePool.Count > 0)
        {
            People result = peoplePool[0];
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
        if (actionCount <= 0 && carArr.Count >= 1)
        {
            if (carArr.Count == 1 && carArr[0].moveAction.IsPlaying())
                return;
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
}
