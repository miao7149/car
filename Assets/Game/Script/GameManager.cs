using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Linq;
using Unity.VisualScripting;
using Unity.Collections;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public UI m_UI;
    public UIGameVictoryPage m_UIGameVictoryPage;
    //游戏行动次数
    private int stepCount;
    public int StepCount
    {
        get { return stepCount; }
        set
        {
            stepCount = value;
            // 触发事件
            OnStepCountChanged?.Invoke(stepCount);
        }
    }
    public delegate void StepCountChangedHandler(int newStepCount);

    // 定义一个事件
    public event StepCountChangedHandler OnStepCountChanged;
    public int boardX;
    public int boardY;
    //暂定游戏尺寸 11*21 m
    private int[,] roadDataArr;
    //目前车有2m 和 3m 茅点为车头
    private CarInfo[] carDataArr;
    //人数据
    private PeoInfoItem[] peopleDataArr;
    //信号灯数据
    private TrafficLightInfo[] trafficLightDataArr;
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
    private List<RoadItem> extendRoadArr = new();
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
    public People hitPeople = null;//撞到的行人
    int[] StepsLoopArray = new int[] { 2, 1, 2, 1, 0, 1, 2, 2, 2, 1 };
    int[] SeedArray = new[] { 0x3B, 0x29, 0x47, 0x1D, 0x3B, 0x29, 0x47, 0x1D, 0x3B, 0x29, 0x47, 0x1D, 0x3B, 0x29, 0x47, 0x1D, 0x3B, 0x29, 0x47, 0x1D };
    LevelDataConfig levelData;
    //地形片
    public GameObject m_Terrain;
    private void Start()
    {
        Instance = this;

        InitGame();
    }
    public int GetStepCount(GameType type, LevelDataConfig data, int index = 0)
    {
        if (data == null)
        {
            throw new Exception("data is null");
        }
        if (data.carDatas == null)
        {
            throw new Exception("CarDatas is null");
        }
        int size = data.carDatas.Count;
        if (type != GameType.Main || index < 0)
        {
            return size;
        }
        Dictionary<string, LevelstepsTemplate> lstLevelstepsTemplate = GlobalManager.Instance.lstLevelstepsTemplate;
        if (lstLevelstepsTemplate == null)
        {
            throw new Exception("lstLevelstepsTemplate is null");
        }
        int Count = lstLevelstepsTemplate.Count;
        int HighestLevelIndex = GlobalManager.Instance.CurrentLevel;
        int v27 = 0;
        int v17 = HighestLevelIndex - Count;
        if (v17 <= 0)
        {
            string v25 = HighestLevelIndex.ToString();
            if (lstLevelstepsTemplate.TryGetValue(v25, out LevelstepsTemplate levelstepsTemplate))
            {
                v27 = levelstepsTemplate.stepsadd;
            }
            else
            {
                if (data.TrafficLightTemplates != null)
                {
                    if (data.TrafficLightTemplates.Count <= 0)
                    {
                        return v27 + size;
                    }
                    else
                    {
                        return v27 + size + 2;
                    }
                }
                else
                {
                    throw new Exception("data.TrafficLightTemplates is null");
                }
            }
        }
        else
        {
            if (StepsLoopArray == null)
            {
                throw new Exception("StepsLoopArray is null");
            }
            int max_length = StepsLoopArray.Length;
            if (v17 % max_length >= max_length)
            {
                throw new Exception("v17 % max_length >= max_length");
            }
            v27 = StepsLoopArray[v17 % max_length];
        }
        if (data.TrafficLightTemplates != null)
        {
            if (data.TrafficLightTemplates.Count <= 0)
            {
                return v27 + size;
            }
            else
            {
                return v27 + size + 2;
            }
        }
        else
        {
            throw new Exception("data.TrafficLightTemplates is null");
        }
    }
    public void LoadLevelData()
    {
        TextAsset json = null;
        int levelIndex = 0;
        string folderName = "";
        if (GlobalManager.Instance.GameType == GameType.Main)
        {
            levelIndex = GetLevelFileIndex();
            folderName = GetFolderName();
            //加载json文件
            json = Resources.Load<TextAsset>("LevelData/" + folderName + "/" + levelIndex);
            Debug.Log("LevelData/" + folderName + "/" + levelIndex);
        }
        else if (GlobalManager.Instance.GameType == GameType.ChallengeHard)
        {
            if (GlobalManager.Instance.CurrentHardLevel > 134)
            {
                levelIndex = GlobalManager.Instance.CurrentHardLevel - 134;
                folderName = "challengesuperhard";
            }
            else
            {
                levelIndex = GlobalManager.Instance.CurrentHardLevel;
                folderName = "challengehard";
            }
            //加载json文件
            json = Resources.Load<TextAsset>("LevelData/" + folderName + "/" + levelIndex);
            Debug.Log("LevelData/" + folderName + "/" + levelIndex);
        }
        if (json == null)
        {
            Debug.LogError("json is null");
            return;
        }
        // 使用 Newtonsoft.Json 解析 json 数据
        levelData = JsonConvert.DeserializeObject<LevelDataConfig>(json.text);
        var BlockDatas = levelData.BlockDatas;
        roadDataArr = new int[levelData.boardSize.x, levelData.boardSize.y];//初始化棋盘
        Camera.main.transform.position = new Vector3(0, levelData.boardSize.magnitude, -4.6f);
        ResetCamera(levelData.boardSize);

        boardX = levelData.boardSize.x;
        boardY = levelData.boardSize.y;
        //生成十字路口
        for (var i = 0; i < BlockDatas.Count; i++)
        {
            for (var j = 0; j < BlockDatas[i].Count; j++)
            {
                if (BlockDatas[i][j].BlockType == 1)//道路
                {
                    //检查是否是十字路口
                    if (BlockDatas[i][j].Cross == 2)
                    {
                        if (BlockDatas[i][j].Cross == 2)
                        {
                            //如果左边和上边没有路
                            if (i - 1 >= 0 && (BlockDatas[i - 1][j].Cross == 0 || BlockDatas[i - 1][j].BlockType != 1 && BlockDatas[i - 1][j].BlockType != 3) && j + 1 < levelData.boardSize.y && (BlockDatas[i][j + 1].Cross == 0 || BlockDatas[i][j + 1].BlockType != 1 && BlockDatas[i][j + 1].BlockType != 3))
                            {
                                roadDataArr[i, j] = 38;
                            }
                            //如果右边和上边没有路
                            else if (i + 1 < levelData.boardSize.x && (BlockDatas[i + 1][j].Cross == 0 || BlockDatas[i + 1][j].BlockType != 1 && BlockDatas[i + 1][j].BlockType != 3) && j + 1 < levelData.boardSize.y && (BlockDatas[i][j + 1].Cross == 0 || BlockDatas[i][j + 1].BlockType != 1 && BlockDatas[i][j + 1].BlockType != 3))
                            {
                                roadDataArr[i, j] = 39;
                            }
                            //如果左边和下边没有路
                            else if (i - 1 >= 0 && (BlockDatas[i - 1][j].Cross == 0 || BlockDatas[i - 1][j].BlockType != 1 && BlockDatas[i - 1][j].BlockType != 3) && j - 1 >= 0 && (BlockDatas[i][j - 1].Cross == 0 || BlockDatas[i][j - 1].BlockType != 1 && BlockDatas[i][j - 1].BlockType != 3))
                            {
                                roadDataArr[i, j] = 40;
                            }
                            //如果右边和下边没有路
                            else if (i + 1 < levelData.boardSize.x && (BlockDatas[i + 1][j].Cross == 0 || BlockDatas[i + 1][j].BlockType != 1 && BlockDatas[i + 1][j].BlockType != 3) && j - 1 >= 0 && (BlockDatas[i][j - 1].Cross == 0 || BlockDatas[i][j - 1].BlockType != 1 && BlockDatas[i][j - 1].BlockType != 3))
                            {
                                roadDataArr[i, j] = 41;
                            }
                            //检查上下左右是否有路
                            else if (i - 1 >= 0 && (BlockDatas[i - 1][j].Cross == 0 || BlockDatas[i - 1][j].BlockType != 1 && BlockDatas[i - 1][j].BlockType != 3))//左
                            {
                                roadDataArr[i, j] = 35;
                            }
                            else if (i + 1 < levelData.boardSize.x && (BlockDatas[i + 1][j].Cross == 0 || BlockDatas[i + 1][j].BlockType != 1 && BlockDatas[i + 1][j].BlockType != 3))//右
                            {
                                roadDataArr[i, j] = 37;
                            }
                            else if (j - 1 >= 0 && (BlockDatas[i][j - 1].Cross == 0 || BlockDatas[i][j - 1].BlockType != 1 && BlockDatas[i][j - 1].BlockType != 3))//上
                            {
                                roadDataArr[i, j] = 36;
                            }
                            else if (j + 1 < levelData.boardSize.y && (BlockDatas[i][j + 1].Cross == 0 || BlockDatas[i][j + 1].BlockType != 1 && BlockDatas[i][j + 1].BlockType != 3))//下
                            {
                                roadDataArr[i, j] = 34;
                            }
                            else
                            {
                                roadDataArr[i, j] = 3;
                            }
                        }
                        // //如果左边和上边没有路
                        // if (i - 1 >= 0 && BlockDatas[i - 1][j].Cross == 0 && j + 1 < levelData.boardSize.y && BlockDatas[i][j + 1].Cross == 0)
                        // {
                        //     roadDataArr[i, j] = 38;
                        // }
                        // //如果右边和上边没有路
                        // else if (i + 1 < levelData.boardSize.x && BlockDatas[i + 1][j].Cross == 0 && j + 1 < levelData.boardSize.y && BlockDatas[i][j + 1].Cross == 0)
                        // {
                        //     roadDataArr[i, j] = 39;
                        // }
                        // //如果左边和下边没有路
                        // else if (i - 1 >= 0 && BlockDatas[i - 1][j].Cross == 0 && j - 1 >= 0 && BlockDatas[i][j - 1].Cross == 0)
                        // {
                        //     roadDataArr[i, j] = 40;
                        // }
                        // //如果右边和下边没有路
                        // else if (i + 1 < levelData.boardSize.x && BlockDatas[i + 1][j].Cross == 0 && j - 1 >= 0 && BlockDatas[i][j - 1].Cross == 0)
                        // {
                        //     roadDataArr[i, j] = 41;
                        // }
                        // //检查上下左右是否有路
                        // else if (i - 1 >= 0 && BlockDatas[i - 1][j].Cross == 0)//左
                        // {
                        //     roadDataArr[i, j] = 35;
                        // }
                        // else if (i + 1 < levelData.boardSize.x && BlockDatas[i + 1][j].Cross == 0)//右
                        // {
                        //     roadDataArr[i, j] = 37;
                        // }
                        // else if (j - 1 >= 0 && BlockDatas[i][j - 1].Cross == 0)//上
                        // {
                        //     roadDataArr[i, j] = 36;
                        // }
                        // else if (j + 1 < levelData.boardSize.y && BlockDatas[i][j + 1].Cross == 0)//下
                        // {
                        //     roadDataArr[i, j] = 34;
                        // }
                        // else
                        // {
                        //     roadDataArr[i, j] = 3;
                        // }
                    }
                }
            }
        }
        var roadTemplates = levelData.RoadTemplates;
        for (int i = 0; i < roadTemplates.Count; i++)
        {
            var roadTemplate = roadTemplates[i];
            var Axis = roadTemplate.Axis;
            if (roadTemplate.RoadBlocks.Length < 3)
            {
                Debug.LogError("道路块数量过少  path: " + "LevelData/" + folderName + "/" + levelIndex);
                continue;
            }
            for (int j = 0; j < roadTemplate.RoadBlocks.Length; j++) //遍历每个路块
            {
                var x = roadTemplate.RoadBlocks[j][0];
                var y = roadTemplate.RoadBlocks[j][1];
                if (j == 0)
                {
                    if (roadTemplate.RoadBlocks[j + 1][0] - x > 1 || roadTemplate.RoadBlocks[j + 1][1] - y > 1)
                    {
                        Debug.LogError("道路块之间距离过大 LinIndex:  " + roadTemplate.LineIndex + "path: LevelData/" + folderName + "/" + levelIndex);
                        continue;
                    }
                }
                if (j == roadTemplate.RoadBlocks.Length - 1)
                {
                    if (Mathf.Abs(roadTemplate.RoadBlocks[j - 1][0] - x) > 1 || Mathf.Abs(roadTemplate.RoadBlocks[j - 1][1] - y) > 1)
                    {
                        Debug.LogError("道路块之间距离过大 LinIndex:  " + roadTemplate.LineIndex + "path: LevelData/" + folderName + "/" + levelIndex);
                        continue;
                    }
                }
                if (BlockDatas[x][y].Cross == 1)//不是十字路口
                {
                    if (Axis == 1)//横（如果横着的路判断上下两边有没有路）
                    {
                        if (y - 1 >= 0 && BlockDatas[x][y - 1].Cross == 0 && y + 1 < levelData.boardSize.y && BlockDatas[x][y + 1].Cross == 0)//上下都没路（单条路）
                        {
                            if (BlockDatas[x][y].BlockType == 1)//如果是道路
                            {
                                if (x - 1 >= 0 && BlockDatas[x - 1][y].Cross == 0)//左边没路
                                {
                                    roadDataArr[x, y] = 5;
                                }
                                else if (x + 1 < levelData.boardSize.x && BlockDatas[x + 1][y].Cross == 0)//右边没路
                                {
                                    roadDataArr[x, y] = 7;
                                }
                                else//左右都有路
                                {
                                    roadDataArr[x, y] = 1;
                                }
                            }
                            else if (BlockDatas[x][y].BlockType == 3)//如果是人行横道
                            {
                                roadDataArr[x, y] = 32;
                            }
                        }
                        else if (y - 1 >= 0 && BlockDatas[x][y - 1].Cross == 0)//下边没路
                        {
                            if (BlockDatas[x][y].BlockType == 1)//如果是道路
                            {
                                if (x - 1 >= 0 && BlockDatas[x - 1][y].Cross == 0)//左边没路
                                {
                                    roadDataArr[x, y] = 21;
                                }
                                else if (x + 1 < levelData.boardSize.x && BlockDatas[x + 1][y].Cross == 0)//右边没路
                                {
                                    roadDataArr[x, y] = 25;
                                }
                                else//左右都有路
                                {
                                    roadDataArr[x, y] = 12;
                                }
                            }
                            else if (BlockDatas[x][y].BlockType == 3)//如果是人行横道
                            {
                                roadDataArr[x, y] = 31;
                            }
                        }
                        else if (y + 1 < levelData.boardSize.y && BlockDatas[x][y + 1].Cross == 0)//上边没路
                        {
                            if (BlockDatas[x][y].BlockType == 1)//如果是道路
                            {
                                if (x - 1 >= 0 && BlockDatas[x - 1][y].Cross == 0)//左边没路
                                {
                                    roadDataArr[x, y] = 20;
                                }
                                else if (x + 1 < levelData.boardSize.x && BlockDatas[x + 1][y].Cross == 0)//右边没路
                                {
                                    roadDataArr[x, y] = 24;
                                }
                                else//左右都有路
                                {
                                    roadDataArr[x, y] = 10;
                                }
                            }
                            else if (BlockDatas[x][y].BlockType == 3)//如果是人行横道
                            {
                                roadDataArr[x, y] = 30;
                            }
                        }
                        else//上下都有路
                        {
                            if (BlockDatas[x][y].BlockType == 1)//如果是道路
                            {
                                if (x - 1 >= 0 && BlockDatas[x - 1][y].Cross == 0)//左边没路
                                {
                                    roadDataArr[x, y] = 15;
                                }
                                else if (x + 1 < levelData.boardSize.x && BlockDatas[x + 1][y].Cross == 0)//右边没路
                                {
                                    roadDataArr[x, y] = 17;
                                }
                                else//左右都有路
                                {
                                    roadDataArr[x, y] = 8;
                                }
                            }
                            else if (BlockDatas[x][y].BlockType == 3)//如果是人行横道
                            {
                                roadDataArr[x, y] = 27;
                            }

                        }
                    }
                    else//竖（如果竖着的路判断左右两边有没有路）
                    {
                        if (x - 1 >= 0 && BlockDatas[x - 1][y].Cross == 0 && x + 1 < levelData.boardSize.x && BlockDatas[x + 1][y].Cross == 0)//左右都没路（单条路）
                        {
                            if (BlockDatas[x][y].BlockType == 1)//如果是道路
                            {
                                if (y - 1 >= 0 && BlockDatas[x][y - 1].Cross == 0)//下边没路
                                {
                                    roadDataArr[x, y] = 6;
                                }
                                else if (y + 1 < levelData.boardSize.y && BlockDatas[x][y + 1].Cross == 0)//上边没路
                                {
                                    roadDataArr[x, y] = 4;
                                }
                                else//上下都有路
                                {
                                    roadDataArr[x, y] = 2;
                                }
                            }
                            else if (BlockDatas[x][y].BlockType == 3)//如果是人行横道
                            {
                                roadDataArr[x, y] = 33;
                            }
                        }
                        else if (x - 1 >= 0 && BlockDatas[x - 1][y].Cross == 0)//左边没路
                        {
                            if (BlockDatas[x][y].BlockType == 1)//如果是道路
                            {
                                if (y - 1 >= 0 && BlockDatas[x][y - 1].Cross == 0)//下边没路
                                {
                                    roadDataArr[x, y] = 18;
                                }
                                else if (y + 1 < levelData.boardSize.y && BlockDatas[x][y + 1].Cross == 0)//上边没路
                                {
                                    roadDataArr[x, y] = 22;
                                }
                                else//上下都有路
                                {
                                    roadDataArr[x, y] = 13;
                                }
                            }
                            else if (BlockDatas[x][y].BlockType == 3)//如果是人行横道
                            {
                                roadDataArr[x, y] = 28;
                            }
                        }
                        else if (x + 1 < levelData.boardSize.x && BlockDatas[x + 1][y].Cross == 0)//右边没路
                        {
                            if (BlockDatas[x][y].BlockType == 1)//如果是道路
                            {
                                if (y - 1 >= 0 && BlockDatas[x][y - 1].Cross == 0)//下边没路
                                {
                                    roadDataArr[x, y] = 19;
                                }
                                else if (y + 1 < levelData.boardSize.y && BlockDatas[x][y + 1].Cross == 0)//上边没路
                                {
                                    roadDataArr[x, y] = 23;
                                }
                                else//上下都有路
                                {
                                    roadDataArr[x, y] = 11;
                                }
                            }
                            else if (BlockDatas[x][y].BlockType == 3)//如果是人行横道
                            {
                                roadDataArr[x, y] = 29;
                            }
                        }
                        else//左右都有路
                        {
                            if (BlockDatas[x][y].BlockType == 1)//如果是道路
                            {
                                if (y - 1 >= 0 && BlockDatas[x][y - 1].Cross == 0)//下边没路
                                {
                                    roadDataArr[x, y] = 14;
                                }
                                else if (y + 1 < levelData.boardSize.y && BlockDatas[x][y + 1].Cross == 0)//上边没路
                                {
                                    roadDataArr[x, y] = 16;
                                }
                                else//上下都有路
                                {
                                    roadDataArr[x, y] = 9;
                                }
                            }
                            else if (BlockDatas[x][y].BlockType == 3)//如果是人行横道
                            {
                                roadDataArr[x, y] = 26;
                            }
                        }
                    }
                }
            }
            //把道路首尾各加四个路快
            int firstX = roadTemplate.RoadBlocks[0][0];
            int firstY = roadTemplate.RoadBlocks[0][1];
            int lastX = roadTemplate.RoadBlocks[roadTemplate.RoadBlocks.Length - 1][0];
            int lastY = roadTemplate.RoadBlocks[roadTemplate.RoadBlocks.Length - 1][1];
            int firstRoadId = roadDataArr[firstX, firstY];
            int lastRoadId = roadDataArr[lastX, lastY];
            int[] specifiedValues = new int[] { 3, 26, 27, 28, 29, 30, 31, 32, 33 };
            if (specifiedValues.Contains(firstRoadId))
            {
                var x = firstX;
                var y = firstY;
                if (Axis == 1)
                {
                    while (specifiedValues.Contains(firstRoadId) && x <= levelData.boardSize.x)
                    {
                        x += 1;
                        firstRoadId = roadDataArr[x, y];
                    }
                }
                else if (Axis == 2)
                {
                    while (specifiedValues.Contains(firstRoadId) && y <= levelData.boardSize.y)
                    {
                        y += 1;
                        firstRoadId = roadDataArr[x, y];
                    }
                }
            }
            if (specifiedValues.Contains(lastRoadId))
            {
                var x = lastX;
                var y = lastY;
                if (Axis == 1)
                {
                    while (specifiedValues.Contains(lastRoadId) && x >= 0)
                    {
                        x -= 1;
                        lastRoadId = roadDataArr[x, y];
                    }
                }
                else if (Axis == 2)
                {
                    while (specifiedValues.Contains(lastRoadId) && y >= 0)
                    {
                        y -= 1;
                        lastRoadId = roadDataArr[x, y];
                    }
                }
            }
            for (int j = 1; j <= 10; j++)
            {
                if (firstX == 0 || firstY == 0)
                {
                    var road1 = BornRoad(firstRoadId);
                    road1.transform.SetParent(transform, false);
                    road1.gameObject.SetActive(true);
                    extendRoadArr.Add(road1);
                    if (Axis == 1)//横
                    {
                        road1.transform.localPosition = ConvertPos(new Vector3(firstX - j, 0, firstY));
                    }
                    else if (Axis == 2)//竖
                    {
                        road1.transform.localPosition = ConvertPos(new Vector3(firstX, 0, firstY - j));
                    }
                }
                if (lastX == levelData.boardSize.x - 1 || lastY == levelData.boardSize.y - 1)
                {
                    var road2 = BornRoad(lastRoadId);
                    road2.transform.SetParent(transform, false);
                    road2.gameObject.SetActive(true);
                    extendRoadArr.Add(road2);
                    if (Axis == 1)//横
                    {
                        road2.transform.localPosition = ConvertPos(new Vector3(lastX + j, 0, lastY));
                    }
                    else if (Axis == 2)//竖
                    {
                        road2.transform.localPosition = ConvertPos(new Vector3(lastX, 0, lastY + j));
                    }
                }
            }
        }
        //设置车辆位置
        var carDatas = levelData.carDatas;
        var StoneForkliftDatas = levelData.StoneForkliftDatas;
        CarType type = CarType.None;
        for (int i = 0; i < carDatas.Count; i++)
        {
            for (int j = 0; j < StoneForkliftDatas.Count; j++)
            {
                if (StoneForkliftDatas[j].StoneLocation[0] == carDatas[i].Location[0] && StoneForkliftDatas[j].StoneLocation[1] == carDatas[i].Location[1])
                {
                    carDatas.RemoveAt(i);
                }
            }
        }
        carDataArr = new CarInfo[carDatas.Count];
        for (int i = 0; i < carDatas.Count; i++)
        {
            if (carDatas[i].Length == 2)
            {
                type = CarType.Small;
            }
            else if (carDatas[i].Length == 3)
            {
                type = CarType.Big;
            }
            for (int j = 0; j < StoneForkliftDatas.Count; j++)
            {
                if (StoneForkliftDatas[j].ForkliftLocation[0] == carDatas[i].Location[0] && StoneForkliftDatas[j].ForkliftLocation[1] == carDatas[i].Location[1])
                {
                    type = CarType.Bulldozer;
                }
                if (StoneForkliftDatas[j].StoneLocation[0] == carDatas[i].Location[0] && StoneForkliftDatas[j].StoneLocation[1] == carDatas[i].Location[1])
                {
                    type = CarType.None;
                }
            }
            if (type != CarType.None)
                carDataArr[i] = new CarInfo(type, carDatas[i].Location[0], carDatas[i].Location[1], carDatas[i].SelfForward, carDatas[i].MoveType);
        }
        //设置石头位置
        blockDataArr = new BlockInfo[StoneForkliftDatas.Count];
        for (int i = 0; i < StoneForkliftDatas.Count; i++)
        {
            blockDataArr[i] = new BlockInfo(new IntPos(StoneForkliftDatas[i].StoneLocation[0], StoneForkliftDatas[i].StoneLocation[1]));
        }
        //设置红绿灯位置
        var TrafficLightDatas = levelData.TrafficLightTemplates;
        trafficLightDataArr = new TrafficLightInfo[TrafficLightDatas.Count];
        for (int i = 0; i < TrafficLightDatas.Count; i++)
        {
            trafficLightDataArr[i] = new TrafficLightInfo(new IntPos(TrafficLightDatas[i].Location[0], TrafficLightDatas[i].Location[1]), TrafficLightDatas[i].SelfForward);
        }
        //设置小人位置
        var PeopleDatas = levelData.SideWalkTemplates;
        peopleDataArr = new PeoInfoItem[PeopleDatas.Count];
        for (int i = 0; i < PeopleDatas.Count; i++)
        {
            int firstX = PeopleDatas[i].Locations[0];
            int firstY = PeopleDatas[i].Locations[1];
            int lastX = PeopleDatas[i].Locations[PeopleDatas[i].Locations.Length - 2];
            int lastY = PeopleDatas[i].Locations[PeopleDatas[i].Locations.Length - 1];
            peopleDataArr[i] = new PeoInfoItem(new IntPos(firstX, firstY), new IntPos(lastX, lastY));
        }
        //判断游戏提示
        if (StoneForkliftDatas.Count > 0 && GlobalManager.Instance.IsBulldozerIntroduce)//推土机提示
        {
            GlobalManager.Instance.IsBulldozerIntroduce = false;
            m_UI.ShowGameIntroduce("Bulldozer");
        }
        else if (TrafficLightDatas.Count > 0 && GlobalManager.Instance.IsLightIntroduce)//红绿灯提示
        {
            GlobalManager.Instance.IsLightIntroduce = false;
            m_UI.ShowGameIntroduce("TrafficLight");
        }
        else if (PeopleDatas.Count > 0 && GlobalManager.Instance.IsPeoIntroduce)//行人提示
        {
            GlobalManager.Instance.IsPeoIntroduce = false;
            m_UI.ShowGameIntroduce("People");
        }
        GlobalManager.Instance.SaveGameData();
    }
    private void ResetCamera(BoardSize boardSize)
    {
        int m_Y = boardSize.y;
        int m_X = boardSize.x;
        int v7 = 1080;
        int v8 = 2160;
        float width = Screen.width;
        float height = Screen.height;
        float v11 = m_Y;
        float val2 = Math.Max((v7 * height) / (width * v8), (width * v8) / (v7 * height));//1.125
        float v12 = Math.Max(width / v7, height / v8);//1
        int v13 = 7;
        float v14 = 14;
        float v15 = v11 / v14;
        if (GlobalManager.Instance.CurrentLevel >= 3)
        {
            float v16 = (float)m_X / (float)v13;
            float v17 = Math.Min(Math.Abs(v8 / v7 - height / width), 0.1f);
            float v18 = v17 + 1.35f;
            float v19 = Math.Max(v16, v15);
            float v21 = Math.Max(v12, val2);
            v15 = 1.0f;
            float v22 = Math.Min(v21, 1.2f);
            float v23 = v19 * v22;
            if (v18 < 1.0f)
            {
                throw new Exception("System.Math::ThrowMinMaxException");
            }
            if (v23 >= 1.0f)
            {
                if (v23 <= v18)
                {
                    v15 = v19 * v22;
                }
                else
                {
                    v15 = v18;
                }
            }
        }
        Vector3 position = Camera.main.transform.position;
        float y = position.y;
        float z = position.z;
        Vector3 v34 = new Vector3(position.x, v15 * 20, v15 * z);
        Camera.main.transform.position = v34;
    }

    //引导检测
    public void CheckGuide()
    {
        if (GlobalManager.Instance.CurrentLevel == 0)
        {
            m_UI.ShowGuideFinger(Camera.main.WorldToScreenPoint(carArr[0].transform.position));
            m_UI.ShowGuideTip(GlobalManager.Instance.CurrentLevel);
        }
        else if (GlobalManager.Instance.CurrentLevel == 1)
        {
            m_UI.ShowGuideFinger(Camera.main.WorldToScreenPoint(carArr[1].transform.position));
            m_UI.ShowGuideTip(GlobalManager.Instance.CurrentLevel);
        }
        else if (GlobalManager.Instance.CurrentLevel == 7)
        {
            m_UI.ShowGuideFinger(m_UI.m_ItemBtn.transform.position);
        }

    }
    private int GetLevelFileIndex()
    {
        bool v9 = GlobalManager.Instance.CurrentLevel <= 2009;
        if (!v9)
        {
            if (SeedArray == null)
            {
                throw new Exception("SeedArray is null");
            }
            int max_length = SeedArray.Length;
            int v14 = GlobalManager.Instance.CurrentLevel;
            int v17 = (GlobalManager.Instance.CurrentLevel - 2010) / 1960;
            int v18 = (v17 + v14 - 2010) % max_length;
            if (v18 >= SeedArray.Length)
            {
                throw new Exception("v18 >= SeedArray.Length");
            }
            int v19 = SeedArray[v18];
            return (v17 + (GlobalManager.Instance.CurrentLevel - 2010) % 980 * v19) % 980;
        }
        if (GlobalManager.Instance.CurrentLevel < 50)
        {
            return GlobalManager.Instance.CurrentLevel;
        }
        int v20 = GlobalManager.Instance.CurrentLevel - 50;
        if (GlobalManager.Instance.CurrentLevel - 50 < 0)
        {
            v20 = GlobalManager.Instance.CurrentLevel - 49;
        }
        return v20 >> 1;
    }
    private string GetFolderName()
    {
        if (GlobalManager.Instance.CurrentLevel < 50)
        {
            return "default";
        }
        if ((GlobalManager.Instance.CurrentLevel & 1) == 0)
        {
            return "easy";
        }
        return "hard";
    }
    public void InitGame()
    {
        SetGameStatu(GameStatu.preparing);
        CleanGame();
        LoadLevelData();
        //初始化路
        for (var i = 0; i < boardX; i++)
            for (var j = 0; j < boardY; j++)
                if (roadDataArr[i, j] != 0)
                {
                    var road = BornRoad(roadDataArr[i, j]);
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
            // switch (carInfo.dir)
            // {
            //     case 0: //上
            //         car.transform.localPosition = ConvertPos(new Vector3(carInfo.posX, 0, carInfo.posY - 0.5f));
            //         break;
            //     case 1: //下
            //         car.transform.localPosition = ConvertPos(new Vector3(carInfo.posX, 0, carInfo.posY + 0.5f));
            //         break;
            //     case 2: //左
            //         car.transform.localPosition = ConvertPos(new Vector3(carInfo.posX + 0.5f, 0, carInfo.posY));
            //         break;
            //     case 3: //右
            //         car.transform.localPosition = ConvertPos(new Vector3(carInfo.posX - 0.5f, 0, carInfo.posY));
            //         break;
            //     default:
            //         break;
            // }
            car.transform.localPosition = ConvertPos(new Vector3(carInfo.posX, 0, carInfo.posY));
            car.CarOutOfBounds += m_UI.CreateCarOutOfBoundsAnim;
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
            var light = Instantiate(prefabTrafficLight).transform.GetChild(0).GetComponent<TrafficLight>();
            light.transform.parent.SetParent(transform, false);
            light.transform.parent.localPosition = ConvertPos(new Vector3(item.pos.x, 0, item.pos.y));
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
        if (GlobalManager.Instance.CurrentLevel < 7)
        {
            m_UI.m_ItemBtn.gameObject.SetActive(false);
        }
        //地形贴图
        m_Terrain.GetComponent<Renderer>().material.mainTexture = Resources.Load<Texture>("Image/Skin/" + GlobalManager.Instance.PlayerMapSkinName);
        StartCoroutine(PlayStartAni());
    }
    IEnumerator PlayStartAni()
    {
        foreach (var car in carArr)
        {
            car.transform.localScale = Vector3.zero;
            car.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).Play();
        }
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayGameStart();
        yield return null;
        //获取关卡步数
        StepCount = GetStepCount(GlobalManager.Instance.GameType, levelData);
        m_UI.ChangeLevelCount(GlobalManager.Instance.GameType);
        yield return new WaitForSeconds(0.5f);
        SetGameStatu(GameStatu.playing);
        foreach (var car in carArr)
        {
            car.StartCarScaleAnimation();
        }
        CheckGuide();
    }
    private void OnDestroy()
    {
        DOTween.KillAll();
        //GlobalManager.Instance.SaveGameData();
    }
    private void Update()
    {
        if (gameStatu == GameStatu.playing && Input.touchCount > 0 && StepCount > 0)
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
                        m_UI.HideGuideFinger();
                        if (IsUseItem) //使用道具
                            StartCoroutine(UseItem(hit.collider.transform.parent.parent.GetComponent<Car>().transform));
                        //LiftCarWithBalloon(hit.collider.transform.parent.parent.GetComponent<Car>().transform);
                        else
                        {
                            if (GlobalManager.Instance.CurrentLevel >= 10)
                            {
                                StepCount--;//行动次数减一
                            }
                            OnTouchCar(hit.collider.transform.parent.parent.GetComponent<Car>());
                        }
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
                    nextStep += dirction;
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
                        nextStep += dirction;
                    }
                    else
                    {
                        switch (roadDataArr[nextStep.x, nextStep.y])
                        {
                            case 3:
                            case 4:
                            case 5:
                            case 6:
                            case 7:
                                dirction = new Vector2Int(-dirction.y, dirction.x);
                                nextStep += dirction;
                                turnCount--;
                                break;
                            default:
                                nextStep += dirction;
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
                        nextStep += dirction;
                    }
                    else
                    {
                        switch (roadDataArr[nextStep.x, nextStep.y])
                        { //十字路口可以转
                            case 3:
                            case 4:
                            case 5:
                            case 6:
                            case 7:
                                dirction = new Vector2Int(dirction.y, -dirction.x);
                                nextStep += dirction;
                                turnCount--;
                                break;
                            default:
                                nextStep += dirction;
                                break;
                        }
                    }
                } while (outmap == false);
                break;
            case 4: //左掉头
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
                        nextStep += dirction;
                    }
                    else
                    {
                        switch (roadDataArr[nextStep.x, nextStep.y])
                        {
                            case 3:
                            case 4:
                            case 5:
                            case 6:
                            case 7:
                                if (tempStepp > 0)
                                {
                                    tempStepp--;
                                    nextStep += dirction;
                                }
                                else
                                {
                                    dirction = new Vector2Int(dirction.y, -dirction.x);
                                    nextStep += dirction;
                                    turnCount--;
                                    tempStepp = 1;
                                }

                                break;
                            default:
                                if (tempStepp > 0)
                                {
                                    tempStepp--;
                                }

                                nextStep += dirction;
                                break;
                        }
                    }
                } while (outmap == false);
                break;
            case 5: //右掉头
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
                        nextStep += dirction;
                    }
                    else
                    {
                        switch (roadDataArr[nextStep.x, nextStep.y])
                        {
                            case 3:
                            case 4:
                            case 5:
                            case 6:
                            case 7:
                                if (tempStep > 0)
                                {
                                    tempStep--;
                                    nextStep += dirction;
                                }
                                else
                                {
                                    dirction = new Vector2Int(-dirction.y, dirction.x);
                                    nextStep += dirction;
                                    turnCount--;
                                    tempStep = 1;
                                }

                                break;
                            default:
                                if (tempStep > 0)
                                {
                                    tempStep--;
                                }

                                nextStep += dirction;
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
        --GlobalManager.Instance.ItemCount;//更新道具数量UI
        IsUseItem = false;
        m_UI.OnHideItemIntroduceBtn();
        carArr.Remove(car.GetComponent<Car>());
        Action carAction = () =>
      {
          GlobalManager.Instance.SaveGameData();//保存数据
          DeleteCar(car.GetComponent<Car>());//删除车
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
    IEnumerator UseItem(Transform car)
    {
        --GlobalManager.Instance.ItemCount;//更新道具数量UI
        IsUseItem = false;
        m_UI.OnHideItemIntroduceBtn();
        carArr.Remove(car.GetComponent<Car>());
        car.GetComponent<Car>().MoveDroneToCarTop(() =>
        {
            Action carAction = () =>
            {
                CheckGameResult();
                GlobalManager.Instance.SaveGameData();//保存数据
                DeleteCar(car.GetComponent<Car>());//删除车
            };
            StartCoroutine(HelicopterFlyAnimation(car, carAction));
        });//显示气球
        //等待一秒
        yield return new WaitForSeconds(1f);
        if (car.GetComponent<Car>().type == CarType.Bulldozer)
        {
            var block = GetBulldozerPathBlock(car.GetComponent<Car>());
            block.MoveDroneToCarTop(() =>
            {
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
            });

        }
    }
    //直升飞机动画
    IEnumerator HelicopterFlyAnimation(Transform car, Action callback = null)
    {
        Vector3 startPosition = car.position; // 起始位置
        Vector3 endPosition = Camera.main.transform.position + Camera.main.transform.forward * 2 + Vector3.right * 4f; // 结束位置
        Vector3 controlPoint = startPosition + (endPosition - startPosition) / 2 + Vector3.left * 10f; // 控制点，用于定义弧线
        var tween = car.transform.DOMoveY(2, 1f).SetEase(Ease.OutQuad); // 将汽车移动到直升飞机上方
        //缓缓朝向下一个点
        Vector3 nextPos = Vector3.Lerp(Vector3.Lerp(startPosition, controlPoint, 0.01f), Vector3.Lerp(controlPoint, endPosition, 0.01f), 0.01f);
        car.transform.DOLookAt(nextPos, 2f);
        //等待汽车移动到飞机上方
        yield return tween.WaitForCompletion();
        startPosition = car.position;//更新起始位置
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
        if (car.dead) return;
        List<Vector2Int> posArr = new List<Vector2Int>();
        int PosArrIndex = 0;
        Car CurrentCar = car;
        if (CurrentCar.mTrail != null)
            CurrentCar.mTrail.SetActive(true);
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

                    nextStep += dirction;
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
                        nextStep += dirction;
                    }
                    else
                    {
                        int[] roadIds = null;
                        if (dirction == Vector2Int.up)
                        {
                            roadIds = new int[] { 3, 34, 37, 39 };
                        }
                        else if (dirction == Vector2Int.down)
                        {
                            roadIds = new int[] { 3, 36, 35, 40 };
                        }
                        else if (dirction == Vector2Int.left)
                        {
                            roadIds = new int[] { 3, 35, 34, 38 };
                        }
                        else if (dirction == Vector2Int.right)
                        {
                            roadIds = new int[] { 3, 37, 36, 41 };
                        }
                        var roadId = roadDataArr[nextStep.x, nextStep.y];
                        bool isRoad = false;
                        foreach (var item in roadIds)
                        {
                            if (roadId == item)
                            {
                                isRoad = true;
                                posArr.Add(nextStep);
                                dirction = new Vector2Int(-dirction.y, dirction.x);
                                nextStep += dirction;
                                turnCount--;
                                break;
                            }
                        }
                        if (!isRoad)
                        {
                            nextStep += dirction;
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
                        nextStep += dirction;
                    }
                    else
                    {
                        int[] roadIds = null;
                        if (dirction == Vector2Int.up)
                        {
                            roadIds = new int[] { 3, 34, 35, 38 };
                        }
                        else if (dirction == Vector2Int.down)
                        {
                            roadIds = new int[] { 3, 36, 37, 41 };
                        }
                        else if (dirction == Vector2Int.left)
                        {
                            roadIds = new int[] { 3, 36, 35, 40 };
                        }
                        else if (dirction == Vector2Int.right)
                        {
                            roadIds = new int[] { 3, 37, 34, 39 };
                        }
                        var roadId = roadDataArr[nextStep.x, nextStep.y];
                        bool isRoad = false;
                        foreach (var item in roadIds)
                        {
                            if (roadId == item)
                            {
                                isRoad = true;
                                posArr.Add(nextStep);
                                dirction = new Vector2Int(dirction.y, -dirction.x);
                                nextStep += dirction;
                                turnCount--;
                                break;
                            }
                        }
                        if (!isRoad)
                        {
                            nextStep += dirction;
                        }
                    }
                } while (hitcar == false && outmap == false && hitBlock == false);

                break;
            case 4: //右掉头
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
                        nextStep += dirction;
                    }
                    else
                    {
                        int[] roadIds = null;
                        if (dirction == Vector2Int.up)
                        {
                            roadIds = new int[] { 3, 34, 35, 38 };
                        }
                        else if (dirction == Vector2Int.down)
                        {
                            roadIds = new int[] { 3, 36, 37, 41 };
                        }
                        else if (dirction == Vector2Int.left)
                        {
                            roadIds = new int[] { 3, 36, 35, 40 };
                        }
                        else if (dirction == Vector2Int.right)
                        {
                            roadIds = new int[] { 3, 37, 34, 39 };
                        }
                        var roadId = roadDataArr[nextStep.x, nextStep.y];
                        bool isRoad = false;
                        foreach (var item in roadIds)
                        {
                            if (roadId == item)
                            {
                                isRoad = true;
                                if (tempStepp > 0)
                                {
                                    tempStepp--;
                                    nextStep += dirction;
                                }
                                else
                                {
                                    posArr.Add(nextStep);
                                    dirction = new Vector2Int(dirction.y, -dirction.x);
                                    nextStep += dirction;
                                    turnCount--;
                                    tempStepp = 1;
                                }

                                break;
                            }
                        }
                        if (!isRoad)
                        {
                            if (tempStepp > 0)
                            {
                                tempStepp--;
                            }

                            nextStep += dirction;
                        }
                    }
                } while (hitcar == false && outmap == false && hitBlock == false);

                break;
            case 5: //左掉头

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
                        nextStep += dirction;
                    }
                    else
                    {
                        int[] roadIds = null;
                        if (dirction == Vector2Int.up)
                        {
                            roadIds = new int[] { 3, 34, 37, 39 };
                        }
                        else if (dirction == Vector2Int.down)
                        {
                            roadIds = new int[] { 3, 36, 35, 40 };
                        }
                        else if (dirction == Vector2Int.left)
                        {
                            roadIds = new int[] { 3, 35, 34, 38 };
                        }
                        else if (dirction == Vector2Int.right)
                        {
                            roadIds = new int[] { 3, 37, 36, 41 };
                        }
                        var roadId = roadDataArr[nextStep.x, nextStep.y];
                        bool isRoad = false;
                        foreach (var item in roadIds)
                        {
                            if (roadId == item)
                            {
                                isRoad = true;
                                if (tempStep > 0)
                                {
                                    tempStep--;
                                    nextStep += dirction;
                                }
                                else
                                {
                                    posArr.Add(nextStep);
                                    dirction = new Vector2Int(-dirction.y, dirction.x);
                                    nextStep += dirction;
                                    turnCount--;
                                    tempStep = 1;
                                }

                                break;
                            }
                        }
                        if (!isRoad)
                        {
                            if (tempStep > 0)
                            {
                                tempStep--;
                            }

                            nextStep += dirction;
                        }
                    }
                } while (hitcar == false && outmap == false && hitBlock == false);
                break;
        }
        float speed = car.type == CarType.Small ? speedCarSmall : speedCarBig;//速度
        if (outmap)
        {
            carArr.Remove(car);
            if (CurrentCar.sprite != null)
                CurrentCar.sprite.gameObject.SetActive(false);
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
            car.moveAction = car.transform.DOLocalMove(ConvertPos(new Vector3(posArr[index].x, 0, posArr[index].y)), GetDuring(speed, posArr[index - 1], posArr[index])).SetEase(Ease.Linear).OnComplete(() =>
             {
                 index++;
                 if (index < posArr.Count)
                 {
                     car.transform.DOLocalRotateQuaternion(Quaternion.LookRotation(new Vector3(posArr[index].x, 0, posArr[index].y) - new Vector3(posArr[index - 1].x, 0, posArr[index - 1].y)), speedCarRotate).Play();
                     //car.transform.LookAt(ConvertPos(new Vector3(posArr[index].x, 0, posArr[index].y)));
                     car.moveAction = car.transform.DOLocalMove(ConvertPos(new Vector3(posArr[index].x, 0, posArr[index].y)), GetDuring(speed, posArr[index - 1], posArr[index])).SetEase(Ease.Linear).OnComplete(() =>
                     {
                         index++;
                         if (index < posArr.Count)
                         {
                             car.transform.DOLocalRotateQuaternion(Quaternion.LookRotation(new Vector3(posArr[index].x, 0, posArr[index].y) - new Vector3(posArr[index - 1].x, 0, posArr[index - 1].y)), speedCarRotate).Play();
                             //car.transform.LookAt(ConvertPos(new Vector3(posArr[index].x, 0, posArr[index].y)));
                             car.moveAction = car.transform.DOLocalMove(ConvertPos(new Vector3(posArr[index].x, 0, posArr[index].y)), GetDuring(speed, posArr[index - 1], posArr[index])).SetEase(Ease.Linear).OnComplete(() =>
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
            //StartCoroutine(hitCar());
            PosArrIndex = 1;
            car.moveAction = car.transform.DOLocalMove(ConvertPos(new Vector3(posArr[PosArrIndex].x, 0, posArr[PosArrIndex].y)), GetDuring(speed, posArr[PosArrIndex - 1], posArr[PosArrIndex])).OnComplete(() =>
            {
                PosArrIndex++;
                if (PosArrIndex < posArr.Count)
                {
                    if (new Vector3(posArr[PosArrIndex].x, 0, posArr[PosArrIndex].y) - new Vector3(posArr[PosArrIndex - 1].x, 0, posArr[PosArrIndex - 1].y) != Vector3.zero)
                        car.transform.DOLocalRotateQuaternion(Quaternion.LookRotation(new Vector3(posArr[PosArrIndex].x, 0, posArr[PosArrIndex].y) - new Vector3(posArr[PosArrIndex - 1].x, 0, posArr[PosArrIndex - 1].y)), speedCarRotate).Play();
                    // car.transform.LookAt(ConvertPos(new Vector3(posArr[index].x, 0, posArr[index].y)));
                    car.moveAction = car.transform.DOLocalMove(ConvertPos(new Vector3(posArr[PosArrIndex].x, 0, posArr[PosArrIndex].y)), GetDuring(speed, posArr[PosArrIndex - 1], posArr[PosArrIndex])).OnComplete(() =>
                    {
                        PosArrIndex++;
                        if (PosArrIndex < posArr.Count)
                        {
                            car.transform.DOLocalRotateQuaternion(Quaternion.LookRotation(new Vector3(posArr[PosArrIndex].x, 0, posArr[PosArrIndex].y) - new Vector3(posArr[PosArrIndex - 1].x, 0, posArr[PosArrIndex - 1].y)), speedCarRotate).Play();
                            //         car.transform.LookAt(ConvertPos(new Vector3(posArr[index].x, 0, posArr[index].y)));
                            car.moveAction = car.transform.DOLocalMove(ConvertPos(new Vector3(posArr[PosArrIndex].x, 0, posArr[PosArrIndex].y)), GetDuring(speed, posArr[PosArrIndex - 1], posArr[PosArrIndex])).OnComplete(() =>
                            {
                                PosArrIndex++;
                                if (hitcar)
                                {
                                    HitCar(carArr[hitCarIndex], dirction, backCar);
                                }
                            }).Play();
                        }
                        else
                        {
                            if (hitcar)
                            {
                                HitCar(carArr[hitCarIndex], dirction, backCar);
                            }
                        }
                    }).Play();
                }
                else
                {
                    if (hitcar)
                    {
                        HitCar(carArr[hitCarIndex], dirction, backCar);
                    }
                }
            }).Play();


        }
        IEnumerator hitCar()
        {
            for (int i = 1; i < posArr.Count; i++)
            {
                if (i < posArr.Count - 1)
                {
                    car.moveAction = CurrentCar.transform.DOLocalMove(ConvertPos(new Vector3(posArr[i].x, 0, posArr[i].y)), GetDuring(speed, posArr[i - 1], posArr[i])).OnComplete(() =>
                    {
                        var dir = new Vector3(posArr[i].x, 0, posArr[i].y) - new Vector3(posArr[i - 1].x, 0, posArr[i - 1].y);
                        car.moveAction = CurrentCar.transform.DOLocalRotateQuaternion(Quaternion.LookRotation(dir), speedCarRotate).OnComplete(() =>
                        {

                        }).Play();
                    }).Play();
                }
                if (i == posArr.Count - 1)
                {
                    car.moveAction = CurrentCar.transform.DOLocalMove(ConvertPos(new Vector3(posArr[i - 1].x, 0, posArr[i - 1].y)), GetDuring(speed, posArr[i - 1], posArr[i])).OnComplete(() =>
                    {
                        var dir = new Vector3(posArr[i].x, 0, posArr[i].y) - new Vector3(posArr[i - 1].x, 0, posArr[i - 1].y);
                        car.moveAction = CurrentCar.transform.DOLocalRotateQuaternion(Quaternion.LookRotation(dir), speedCarRotate).OnComplete(() =>
                        {

                        }).Play();
                    }).Play();
                    // //通过posArr[i]和posArr[i-1]计算车辆方向
                    // var dir = posArr[i] - posArr[i - 1];
                    // switch (dir.x)
                    // {
                    //     case 1:
                    //         car.dir = Vector2Int.right;
                    //         car.turn = 1;
                    //         break;
                    //     case -1:
                    //         car.dir = Vector2Int.left;
                    //         car.turn = 1;
                    //         break;
                    // }
                    // switch (dir.y)
                    // {
                    //     case 1:
                    //         car.dir = Vector2Int.up;
                    //         car.turn = 1;
                    //         break;
                    //     case -1:
                    //         car.dir = Vector2Int.down;
                    //         car.turn = 1;
                    //         break;
                    // }

                }
                yield return car.moveAction.WaitForCompletion();
            }
            if (hitcar)
            {
                HitCar(carArr[hitCarIndex], dirction, backCar);
            }
        }
        //IEnumerator backCar()
        void backCar()
        {
            // for (int i = posArr.Count - 2; i >= 0; i--)
            // {
            //     if (i > 0)
            //     {
            //         var durning = GetDuring(speed, posArr[i], posArr[i + 1]);//回退时间
            //         car.moveAction = CurrentCar.transform.DOLocalMove(ConvertPos(new Vector3(posArr[i].x, 0, posArr[i].y)), durning).OnComplete(() =>
            //         {
            //             car.moveAction = CurrentCar.transform.DOLocalRotateQuaternion(Quaternion.LookRotation(new Vector3(posArr[i].x, 0, posArr[i].y) - new Vector3(posArr[i - 1].x, 0, posArr[i - 1].y)), speedCarRotate).OnComplete(() =>
            //              {
            //                  //直线行驶
            //              }).Play();
            //         }).Play();
            //     }
            //     if (i == 0)
            //     {
            //         var durning = GetDuring(speed, posArr[i], posArr[i + 1]);//回退时间
            //         switch (car.carInfo.dir)
            //         {
            //             case 0: //上
            //                 car.moveAction = CurrentCar.transform.DOLocalMove(ConvertPos(new Vector3(posArr[i].x, 0, posArr[i].y - 0.5f)), durning).OnComplete(() =>
            //                 {
            //                     if (StepCount > 0)
            //                         SetGameStatu(GameStatu.playing);
            //                 }).Play();
            //                 break;
            //             case 1: //下
            //                 car.moveAction = CurrentCar.transform.DOLocalMove(ConvertPos(new Vector3(posArr[i].x, 0, posArr[i].y + 0.5f)), durning).OnComplete(() =>
            //                {
            //                    if (StepCount > 0)
            //                        SetGameStatu(GameStatu.playing);
            //                }).Play();
            //                 break;
            //             case 2: //左
            //                 car.moveAction = CurrentCar.transform.DOLocalMove(ConvertPos(new Vector3(posArr[i].x + 0.5f, 0, posArr[i].y)), durning).OnComplete(() =>
            //                {
            //                    if (StepCount > 0)
            //                        SetGameStatu(GameStatu.playing);
            //                }).Play();
            //                 break;
            //             case 3: //右
            //                 car.moveAction = CurrentCar.transform.DOLocalMove(ConvertPos(new Vector3(posArr[i].x - 0.5f, 0, posArr[i].y)), durning).OnComplete(() =>
            //               {
            //                   if (StepCount > 0)
            //                       SetGameStatu(GameStatu.playing);
            //               }).Play();
            //                 break;
            //             default:
            //                 break;
            //         }
            //         if (CurrentCar.mTrail != null)
            //             CurrentCar.mTrail.SetActive(false);
            //     }
            //     yield return car.moveAction.WaitForCompletion();
            // }
            if (CurrentCar.mTrail != null)
                CurrentCar.mTrail.SetActive(false);
            PosArrIndex -= 2;//回退两步
            var durning = GetDuring(speed, posArr[PosArrIndex], posArr[PosArrIndex + 1]);//回退时间

            if (PosArrIndex > 0)
            {

            }

            CurrentCar.transform.DOLocalMove(ConvertPos(new Vector3(posArr[PosArrIndex].x, 0, posArr[PosArrIndex].y)), durning).OnComplete(() =>
            {
                if (PosArrIndex == 0)
                {
                    if (StepCount > 0)
                        SetGameStatu(GameStatu.playing);
                }
                else
                {
                    CurrentCar.transform.DOLocalRotateQuaternion(Quaternion.LookRotation(new Vector3(posArr[PosArrIndex].x, 0, posArr[PosArrIndex].y) - new Vector3(posArr[PosArrIndex - 1].x, 0, posArr[PosArrIndex - 1].y)), speedCarRotate).Play();
                    PosArrIndex--;

                    durning = GetDuring(speed, posArr[PosArrIndex], posArr[PosArrIndex + 1]);
                    if (PosArrIndex > 0)
                    {
                    }

                    CurrentCar.transform.DOLocalMove(ConvertPos(new Vector3(posArr[PosArrIndex].x, 0, posArr[PosArrIndex].y)), durning).OnComplete(() =>
                    {
                        if (PosArrIndex == 0)
                        {
                            if (StepCount > 0)
                                SetGameStatu(GameStatu.playing);
                        }
                        else
                        {
                            CurrentCar.transform.DOLocalRotateQuaternion(Quaternion.LookRotation(new Vector3(posArr[PosArrIndex].x, 0, posArr[PosArrIndex].y) - new Vector3(posArr[PosArrIndex - 1].x, 0, posArr[PosArrIndex - 1].y)), speedCarRotate).Play();
                            PosArrIndex--;
                            CurrentCar.transform.DOLocalMove(ConvertPos(new Vector3(posArr[PosArrIndex].x, 0, posArr[PosArrIndex].y)), GetDuring(speed, posArr[PosArrIndex], posArr[PosArrIndex + 1])).OnComplete(() =>
                            {
                                if (StepCount > 0)
                                    SetGameStatu(GameStatu.playing);
                            }).Play();
                        }
                    }).Play();
                }
            }).Play();
        }
        CheckGameResult();
    }

    public float speedBackCar = 0.1f;
    //public float speedBackRotate = 0.3f;

    float speedCarSmall = 15;
    float speedCarBig = 10;

    float speedCarRotate = 0.1f;

    public void HitCar(Car car, Vector2Int dir, System.Action callback = null)
    {
        AudioManager.Instance.PlayCarCrash();
        var pos = car.transform.localPosition;
        car.transform.DOLocalMove(new Vector3(dir.x * 0.5f, 0, dir.y * 0.5f) + pos, 0.2f).OnComplete(() =>
        {
            car.transform.DOLocalMove(pos, 0.2f).OnComplete(() =>
            {
                if (callback != null)
                {
                    (callback as Action)?.Invoke();
                }
            }).Play();
        }).Play();
    }

    public void DeleteCar(Car car)
    {
        car.CarOutOfBounds -= m_UI.CreateCarOutOfBoundsAnim;
        car.isPlayCoinAnimation = false;
        carPool.Add(car);
        car.transform.SetParent(null, false);
        car.gameObject.SetActive(false);
    }

    public float GetDuring(float speed, Vector2Int p1, Vector2Int p2)
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
        foreach (var o in extendRoadArr)
        {
            o.transform.SetParent(null, false);
            o.gameObject.SetActive(false);
            roadPool.Add(o);
        }
        extendRoadArr.Clear();
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
            Destroy(block.transform.parent.parent.gameObject);
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
            Debug.Log("游戏失败");
        }
        else if (statu == GameStatu.finish)
        {
            //延迟显示胜利界面
            StartCoroutine(ShowFinishUIWithDelay());
            Debug.Log("游戏胜利");
        }
    }
    private IEnumerator ShowFailUIWithDelay()
    {
        // 设置延迟时间
        float delay = 1.5f;
        yield return new WaitForSeconds(delay);
        AudioManager.Instance.PlayFail();
        // 根据失败原因显示相应的失败界面
        if (failReason == FailReason.PeopleCrash)
        {
            m_UI.ShowFailedRoot(true);
        }
        else if (failReason == FailReason.ActionNotEnough)
        {
            m_UI.ShowFailedRoot(true);
        }
    }
    //胜利延时
    private IEnumerator ShowFinishUIWithDelay()
    {
        yield return new WaitForSeconds(1.5f);
        ApplovinSDKManager.Instance().interstitialAdsManager.ShowInterstitialAd(() =>
        {
        });
        // 设置延迟时间
        float delay = 0.8f;
        yield return new WaitForSeconds(delay);
        if (GlobalManager.Instance.GameType == GameType.ChallengeHard)
        {
            GlobalManager.Instance.CurrentHardLevel++;

        }
        else
        {
            GlobalManager.Instance.CurrentLevel++;
        }
        if (GlobalManager.Instance.CurrentLevel != 0 && GlobalManager.Instance.CurrentLevel != 1 && GlobalManager.Instance.CurrentLevel != 2)
        {
            //金币数量
            int coinCount = carDataArr.Length;
            //奖杯数量
            int trophyCount = 10;
            //如果排位赛开启(排位赛开启后困难模式才会开启)
            if (GlobalManager.Instance.mIsStartRankingMatch == true && GlobalManager.Instance.CurrentLevel > 15)
            {
                GlobalManager.Instance.TrophyCompleteLevel++;
                //如果是困难关卡
                if (GlobalManager.Instance.GameType == GameType.ChallengeHard)
                {
                    PlayerPrefs.SetInt("HardLevelStatus" + (GlobalManager.Instance.CurrentHardLevel + 1), 0);
                    trophyCount = 20;
                    coinCount = 50;
                }
                else
                {

                }
                //如果开启双倍奖励
                if (GlobalManager.Instance.IsDoubleReward)
                {
                    trophyCount *= 2;
                }
                m_UIGameVictoryPage.ShowAdvanceFinishRoot(coinCount, trophyCount);
            }
            else//16关之前结算
            {
                m_UIGameVictoryPage.ShowBeginnerFinishRoot(coinCount);
            }
            //如果竞速赛进行中
            if (GlobalManager.Instance.IsCompetition)
            {
                m_UIGameVictoryPage.ShowRacingFinishRoot();
            }
            GlobalManager.Instance.IsReward = GlobalManager.Instance.IsRewardLevel(GlobalManager.Instance.CurrentLevel);//判断是否有奖励
            GlobalManager.Instance.SaveGameData();
            AudioManager.Instance.PlayVictory();
        }
        else
        {
            InitGame();
        }
    }
    //左下角坐标 转 中心坐标
    public Vector3 ConvertPos(Vector3 p)
    {
        //如果boardX和boardY是偶数，需要加0.5
        if (boardX % 2 == 0)
        {
            p.x += 0.5f;
        }
        return new Vector3(p.x - boardX / 2, 0, p.z - boardY / 2);
    }

    public Car BornCar(CarType type)
    {
        Car result = null;
        for (int i = 0; i < carPool.Count; i++)
        {
            if (carPool[i].type == type)
            {
                result = carPool[i];
                result.sprite.gameObject.SetActive(true);
                if (result.mTrail != null)
                    result.mTrail.SetActive(false);
                carPool.RemoveAt(i);
                break;
            }
        }

        if (result == null)
        {
            switch (type)
            {
                case CarType.Small:
                    GameObject skinCar = Instantiate(Resources.Load<GameObject>("Prefabs/GameCar/" + GlobalManager.Instance.PlayerCarSkinName));
                    GameObject skinTrail = Instantiate(Resources.Load<GameObject>("Prefabs/" + GlobalManager.Instance.PlayerCarTrailName), skinCar.transform);
                    skinTrail.transform.localPosition = new Vector3(0, 0, -0.5f);
                    skinTrail.SetActive(false);
                    skinCar.GetComponent<Car>().mTrail = skinTrail;
                    return skinCar.GetComponent<Car>();
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
    //检测游戏结果
    public void CheckGameResult()
    {
        if (StepCount <= 0 && carArr.Count >= 1)
        {
            failReason = FailReason.ActionNotEnough;
            SetGameStatu(GameStatu.faled);
        }
        else if (carArr.Count == 0 && gameStatu == GameStatu.playing)
        {
            SetGameStatu(GameStatu.finish);
        }
    }
    //继续游戏
    public void ContinueGame()
    {
        if (failReason == FailReason.PeopleCrash)
        {
            ResetCar();
            m_UI.ShowFailedRoot(false);
        }
        else if (failReason == FailReason.ActionNotEnough)
        {
            StepCount += 5;
            m_UI.ShowFailedRoot(false);
        }
        SetGameStatu(GameStatu.playing);
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
                    carArr.Add(hitPeopleCar);
                    hitPeopleCar.Init(item);
                    hitPeopleCar.transform.localPosition = ConvertPos(new Vector3(item.posX, 0, item.posY));
                    hitPeopleCar.gameObject.SetActive(true);
                    if (hitPeopleCar.sprite != null)
                        hitPeopleCar.sprite.gameObject.SetActive(true);
                    break;
                }
            }
        }
        if (hitPeople != null)
        {
            hitPeople.Init(hitPeople.peoInfo);
        }
    }
}
