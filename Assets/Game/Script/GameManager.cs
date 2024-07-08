using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;


    //暂定游戏尺寸 11*21 m
    private readonly int[][] roadDataArr = {
        //      0  1  2  3  4  5  6  7  8  9  0  1  2  3  4  5  6  7  8  9  0
        new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0 }, //0
        new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0 }, //1
        new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0 }, //2
        new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0 }, //3
        new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0 }, //4
        new[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 3, 2, 2, 2, 2, 2, 3, 3, 2, 2, 2, 2 }, //5
        new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0 }, //6
        new[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 3, 2, 2, 2, 2, 2, 3, 3, 2, 2, 2, 2 }, //7
        new[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 3, 2, 2, 2, 2, 2, 3, 3, 2, 2, 2, 2 }, //8
        new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0 }, //9
        new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0 } //10
    };

    //目前车有2m 和 3m 茅点为车头
    private CarInfo[] carDataArr = {
        new(1, 8, 9, 1, 2),
        new(1, 7, 8, 1, 4),
        new(1, 2, 15, 2, 1),
        new(1, 2, 16, 2, 1),
        new(1, 5, 12, 1, 2),
    };


    //人数据
    private PeoInfoItem[] peopleDataArr = {
        new PeoInfoItem(new IntPos(9, 13), new IntPos(6, 13))
    };


    public GameObject[] prefabRoadArr;

    public GameObject prefabCar;

    public GameObject prefabPeople;


    private List<RoadItem> roadArr = new();
    private List<RoadItem> roadPool = new();
    private List<Car> carPool = new();
    private List<Car> carArr = new();

    private List<People> peopleArr = new();
    private List<People> peoplePool = new();

    public GameStatu gameStatu;


    private void Start() {
        Instance = this;
        InitGame();
    }

    private void Update() {
        if (gameStatu == GameStatu.playing && Input.touchCount > 0) {
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began) {
                // 将触摸点转换为世界坐标
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                // 射线投射
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("car"))) {
                    // 检查射线是否击中物体
                    if (hit.collider != null) {
                        OnTouchCar(hit.collider.transform.parent.parent.GetComponent<Car>());
                    }
                }
            }
        }
    }

    public void OnTouchCar(Car car) {
        Debug.Log("touchCar");
        if (car.dead) return;
        List<Vector2Int> posArr = new List<Vector2Int>();
        posArr.Add(car.pos);
        Vector2Int dirction = Vector2Int.zero;
        Vector2Int nextStep = Vector2Int.zero;
        bool hitcar = false;
        bool outmap = false;
        int hitCarIndex = 0;
        int turnCount = 0;
        switch (car.turn) {
            case 1: //直线
                dirction = car.dir;
                nextStep = car.pos + dirction;
                do {
                    outmap = PosOutMap(nextStep);
                    if (outmap == false) {
                        hitcar = HasCarOnPos(nextStep, out hitCarIndex);
                        if (hitcar) {
                            posArr.Add(nextStep);
                        }
                    }
                    else {
                        posArr.Add(dirction * 10 + nextStep);
                    }

                    nextStep = nextStep + dirction;
                } while (hitcar == false && outmap == false);

                break;
            case 2: //左转
                turnCount = 1;
                dirction = car.dir;
                nextStep = car.pos + dirction;
                do {
                    outmap = PosOutMap(nextStep);
                    if (outmap == false) {
                        hitcar = HasCarOnPos(nextStep, out hitCarIndex);
                        if (hitcar) {
                            posArr.Add(nextStep);
                        }
                    }
                    else {
                        posArr.Add(dirction * 10 + nextStep);
                    }

                    if (turnCount == 0) {
                        nextStep = nextStep + dirction;
                    }
                    else {
                        switch (roadDataArr[nextStep.x][nextStep.y]) {
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
                } while (hitcar == false && outmap == false);

                break;
            case 3: //右转
                turnCount = 1;
                dirction = car.dir;
                nextStep = car.pos + dirction;
                do {
                    outmap = PosOutMap(nextStep);
                    if (outmap == false) {
                        hitcar = HasCarOnPos(nextStep, out hitCarIndex);
                        if (hitcar) {
                            posArr.Add(nextStep);
                        }
                    }
                    else {
                        posArr.Add(dirction * 10 + nextStep);
                    }

                    if (turnCount == 0) {
                        nextStep = nextStep + dirction;
                    }
                    else {
                        switch (roadDataArr[nextStep.x][nextStep.y]) { //十字路口可以转
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
                } while (hitcar == false && outmap == false);

                break;
            case 4: //左掉头
                turnCount = 2;
                dirction = car.dir;
                nextStep = car.pos + dirction;
                int tempStep = 0; //转弯后走的距离,防止原地掉头
                do {
                    outmap = PosOutMap(nextStep);
                    if (outmap == false) {
                        hitcar = HasCarOnPos(nextStep, out hitCarIndex);
                        if (hitcar) {
                            posArr.Add(nextStep);
                        }
                    }
                    else {
                        posArr.Add(dirction * 10 + nextStep);
                    }

                    if (turnCount == 0) {
                        nextStep = nextStep + dirction;
                    }
                    else {
                        switch (roadDataArr[nextStep.x][nextStep.y]) {
                            case 3:
                            case 4:
                            case 5:
                            case 6:
                            case 7:
                                if (tempStep > 0) {
                                    tempStep--;
                                    nextStep = nextStep + dirction;
                                }
                                else {
                                    posArr.Add(nextStep);
                                    dirction = new Vector2Int(-dirction.y, dirction.x);
                                    nextStep = nextStep + dirction;
                                    turnCount--;
                                    tempStep = 1;
                                }

                                break;
                            default:
                                if (tempStep > 0) {
                                    tempStep--;
                                }

                                nextStep = nextStep + dirction;
                                break;
                        }
                    }
                } while (hitcar == false && outmap == false);

                break;
            case 5: //右掉头
                turnCount = 2;
                dirction = car.dir;
                nextStep = car.pos + dirction;
                int tempStepp = 0; //转弯后走的距离,防止原地掉头
                do {
                    outmap = PosOutMap(nextStep);
                    if (outmap == false) {
                        hitcar = HasCarOnPos(nextStep, out hitCarIndex);
                        if (hitcar) {
                            posArr.Add(nextStep);
                        }
                    }
                    else {
                        posArr.Add(dirction * 10 + nextStep);
                    }

                    if (turnCount == 0) {
                        nextStep = nextStep + dirction;
                    }
                    else {
                        switch (roadDataArr[nextStep.x][nextStep.y]) {
                            case 3:
                            case 4:
                            case 5:
                            case 6:
                            case 7:
                                if (tempStepp > 0) {
                                    tempStepp--;
                                    nextStep = nextStep + dirction;
                                }
                                else {
                                    posArr.Add(nextStep);
                                    dirction = new Vector2Int(dirction.y, -dirction.x);
                                    nextStep = nextStep + dirction;
                                    turnCount--;
                                    tempStepp = 1;
                                }

                                break;
                            default:
                                if (tempStepp > 0) {
                                    tempStepp--;
                                }

                                nextStep = nextStep + dirction;
                                break;
                        }
                    }
                } while (hitcar == false && outmap == false);

                break;
        }

        if (outmap) {
            //carArr.Remove(car);
            car.dead = true;
            car.posArr.Clear();
            int index = 1;
            car.moveAction = car.transform.DOLocalMove(ConvertPos(new Vector3(posArr[index].x, 0, posArr[index].y)), getDuring(speedCarSmall, posArr[index - 1], posArr[index])).OnComplete(() => {
                index++;
                if (index < posArr.Count) {
                    car.transform.DOLocalRotateQuaternion(Quaternion.LookRotation(new Vector3(posArr[index].x, 0, posArr[index].y) - new Vector3(posArr[index - 1].x, 0, posArr[index - 1].y)), speedCarRotate).Play();
                    //car.transform.LookAt(ConvertPos(new Vector3(posArr[index].x, 0, posArr[index].y)));
                    car.moveAction = car.transform.DOLocalMove(ConvertPos(new Vector3(posArr[index].x, 0, posArr[index].y)), getDuring(speedCarSmall, posArr[index - 1], posArr[index])).OnComplete(() => {
                        index++;
                        if (index < posArr.Count) {
                            car.transform.DOLocalRotateQuaternion(Quaternion.LookRotation(new Vector3(posArr[index].x, 0, posArr[index].y) - new Vector3(posArr[index - 1].x, 0, posArr[index - 1].y)), speedCarRotate).Play();
                            //car.transform.LookAt(ConvertPos(new Vector3(posArr[index].x, 0, posArr[index].y)));
                            car.moveAction = car.transform.DOLocalMove(ConvertPos(new Vector3(posArr[index].x, 0, posArr[index].y)), getDuring(speedCarSmall, posArr[index - 1], posArr[index])).OnComplete(() => {
                                DeleteCar(car);
                            }).Play();
                        }
                        else {
                            DeleteCar(car);
                        }
                    }).Play();
                }
                else {
                    DeleteCar(car);
                }
            }).Play();
        }
        else if (hitcar) {
            SetGameStatu(GameStatu.waiting);
            int index = 1;
            car.moveAction = car.transform.DOLocalMove(ConvertPos(new Vector3(posArr[index].x, 0, posArr[index].y)), getDuring(speedCarSmall, posArr[index - 1], posArr[index])).OnComplete(() => {
                index++;
                if (index < posArr.Count) {
                    car.transform.DOLocalRotateQuaternion(Quaternion.LookRotation(new Vector3(posArr[index].x, 0, posArr[index].y) - new Vector3(posArr[index - 1].x, 0, posArr[index - 1].y)), speedCarRotate).Play();
                    // car.transform.LookAt(ConvertPos(new Vector3(posArr[index].x, 0, posArr[index].y)));
                    car.moveAction = car.transform.DOLocalMove(ConvertPos(new Vector3(posArr[index].x, 0, posArr[index].y)), getDuring(speedCarSmall, posArr[index - 1], posArr[index])).OnComplete(() => {
                        index++;
                        if (index < posArr.Count) {
                            car.transform.DOLocalRotateQuaternion(Quaternion.LookRotation(new Vector3(posArr[index].x, 0, posArr[index].y) - new Vector3(posArr[index - 1].x, 0, posArr[index - 1].y)), speedCarRotate).Play();
                            //         car.transform.LookAt(ConvertPos(new Vector3(posArr[index].x, 0, posArr[index].y)));
                            car.moveAction = car.transform.DOLocalMove(ConvertPos(new Vector3(posArr[index].x, 0, posArr[index].y)), getDuring(speedCarSmall, posArr[index - 1], posArr[index])).OnComplete(() => {
                                index++;
                                HitCar(carArr[hitCarIndex], dirction);
                                backCar();
                            }).Play();
                        }
                        else {
                            HitCar(carArr[hitCarIndex], dirction);
                            backCar();
                        }
                    }).Play();
                }
                else {
                    HitCar(carArr[hitCarIndex], dirction);
                    backCar();
                }
            }).Play();

            void backCar() {
                index -= 2;

                var durning = getDuring(speedBackCar, posArr[index], posArr[index + 1]);
                if (index > 0) {
                    car.transform.DOLocalRotateQuaternion(Quaternion.LookRotation(new Vector3(posArr[index].x, 0, posArr[index].y) - new Vector3(posArr[index - 1].x, 0, posArr[index - 1].y)), durning).Play();
                }

                car.transform.DOLocalMove(ConvertPos(new Vector3(posArr[index].x, 0, posArr[index].y)), durning).OnComplete(() => {
                    if (index == 0) {
                        SetGameStatu(GameStatu.playing);
                    }
                    else {
                        index--;

                        durning = getDuring(speedBackCar, posArr[index], posArr[index + 1]);
                        if (index > 0) {
                            car.transform.DOLocalRotateQuaternion(Quaternion.LookRotation(new Vector3(posArr[index].x, 0, posArr[index].y) - new Vector3(posArr[index - 1].x, 0, posArr[index - 1].y)), durning).Play();
                        }

                        car.transform.DOLocalMove(ConvertPos(new Vector3(posArr[index].x, 0, posArr[index].y)), durning).OnComplete(() => {
                            if (index == 0) {
                                SetGameStatu(GameStatu.playing);
                            }
                            else {
                                index--;
                                car.transform.DOLocalMove(ConvertPos(new Vector3(posArr[index].x, 0, posArr[index].y)), getDuring(speedBackCar, posArr[index], posArr[index + 1])).OnComplete(() => {
                                    SetGameStatu(GameStatu.playing);
                                }).Play();
                            }
                        }).Play();
                    }
                }).Play();
            }
        }
    }

    public float speedBackCar = 30f;
    //public float speedBackRotate = 0.3f;

    public float speedCarSmall = 15f;
    public float speedCarBig = 10f;

    public float speedCarRotate = 0.05f;

    public void HitCar(Car car, Vector2Int dir) {
        var pos = car.transform.localPosition;
        car.transform.DOLocalMove(new Vector3(dir.x * 0.5f, 0, dir.y * 0.5f) + pos, 0.2f).OnComplete(() => {
            car.transform.DOLocalMove(pos, 0.2f).Play();
        }).Play();
    }

    public void DeleteCar(Car car) {
        carArr.Remove(car);
        car.transform.SetParent(null, false);
        car.gameObject.SetActive(false);
        carPool.Add(car);
    }

    public float getDuring(float speed, Vector2Int p1, Vector2Int p2) {
        return Vector2Int.Distance(p1, p2) / speed;
    }

    public bool PosOutMap(Vector2Int pos) {
        return pos.x < 0 || pos.x > 10 || pos.y > 20 || pos.y < 0;
    }

    public bool HasCarOnPos(Vector2Int pos, out int carIndex) {
        for (int i = 0; i < carArr.Count; i++) {
            var car = carArr[i];
            foreach (var p in car.posArr) {
                if (p == pos) {
                    carIndex = i;
                    return true;
                }
            }
        }

        carIndex = 0;
        return false;
    }

    public void CleanGame() {
        foreach (var o in roadArr) {
            o.transform.SetParent(null, false);
            o.gameObject.SetActive(false);
            roadPool.Add(o);
        }

        roadArr.Clear();

        foreach (var car in carArr) {
            car.transform.SetParent(null, false);
            car.gameObject.SetActive(false);
            carPool.Add(car);
        }

        carArr.Clear();

        foreach (var people in peopleArr) {
            people.transform.SetParent(null, false);
            people.gameObject.SetActive(false);
            peoplePool.Add(people);
            if (people.action.IsPlaying())
                people.action.Kill();
        }

        peopleArr.Clear();
    }

    public void SetGameStatu(GameStatu statu) {
        gameStatu = statu;
    }


    //初始化车和路
    public void InitGame() {
        SetGameStatu(GameStatu.preparing);
        CleanGame();
        //初始化路
        for (var i = 0; i < roadDataArr.Length; i++)
        for (var j = 0; j < roadDataArr[i].Length; j++)
            if (roadDataArr[i][j] != 0) {
                var road = BornRoad(roadDataArr[i][j]);
                road.transform.localPosition = ConvertPos(new Vector3(i, 0, j));
                road.transform.SetParent(transform, false);
                road.gameObject.SetActive(true);
                roadArr.Add(road);
            }

        //初始化车
        foreach (var carInfo in carDataArr) {
            var car = BornCar(carInfo.type);
            car.Init(carInfo);
            car.transform.SetParent(transform, false);
            car.gameObject.SetActive(true);
            car.transform.localPosition = ConvertPos(new Vector3(carInfo.posX, 0, carInfo.posY));
            car.gameObject.SetActive(true);
            carArr.Add(car);
        }

        //初始化老逼登
        foreach (var item in peopleDataArr) {
            var p = BornPeople();
            peopleArr.Add(p);
            p.gameObject.SetActive(true);
            p.transform.SetParent(transform, false);
            var p1 = ConvertPos(new Vector3(item.pos1.x, 0, item.pos1.y));
            var p2 = ConvertPos(new Vector3(item.pos2.x, 0, item.pos2.y));
            p.transform.localPosition = p1;
            p.action = DOTween.Sequence();
            p.action.Append(p.transform.DOLocalMove(p2, 2));
            p.action.Append(p.transform.DOLocalMove(p1, 2));
            p.action.SetLoops(-1, LoopType.Restart);
            p.action.Play();
            p.Init();
        }

        StartCoroutine(PlayStartAni());
    }

    IEnumerator PlayStartAni() {
        foreach (var car in carArr) {
            car.transform.localScale = Vector3.zero;
            car.transform.DOScale(Vector3.one, 2f).SetEase(Ease.OutBack).Play();
        }

        yield return new WaitForSeconds(2f);
        SetGameStatu(GameStatu.playing);
    }

    //左下角坐标 转 中心坐标
    public Vector3 ConvertPos(Vector3 p) {
        return new Vector3(p.x - 5, 0, p.z - 10);
    }

    public Car BornCar(int type) {
        Car result = null;
        for (int i = 0; i < carPool.Count; i++) {
            if (carPool[i].type == type) {
                result = carPool[i];
                carPool.RemoveAt(i);
                break;
            }
        }

        if (result == null) {
            switch (type) {
                case 1:
                    return Instantiate(prefabCar).GetComponent<Car>();
                default:
                    return null;
            }
        }
        else {
            return result;
        }
    }

    public RoadItem BornRoad(int type) {
        RoadItem result = null;
        for (int i = 0; i < roadPool.Count; i++) {
            if (roadPool[i].type == type) {
                result = roadPool[i];
                roadPool.RemoveAt(i);
                break;
            }
        }

        if (result == null) {
            return Instantiate(prefabRoadArr[type - 1]).GetComponent<RoadItem>();
        }
        else {
            return result;
        }
    }

    public People BornPeople() {
        People result = null;
        if (peoplePool.Count > 0) {
            result = peoplePool[0];
            peoplePool.RemoveAt(0);
            return result;
        }
        else {
            return Instantiate(prefabPeople).GetComponent<People>();
        }
    }
}
