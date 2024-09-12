using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using System;
using System.Text.RegularExpressions;
public class GlobalManager : MonoBehaviour
{
    // 单例实例
    private static GlobalManager instance;
    // 公共访问点
    public static GlobalManager Instance
    {
        get
        {
            if (instance == null)
            {
                // 在场景中查找现有实例
                instance = FindObjectOfType<GlobalManager>();

                // 如果没有找到，创建一个新的GameObject并添加GlobalManager组件
                if (instance == null)
                {
                    GameObject singleton = new GameObject("GlobalManager");
                    instance = singleton.AddComponent<GlobalManager>();
                    DontDestroyOnLoad(singleton);
                }
            }
            return instance;
        }
    }
    //玩家名称
    [HideInInspector]
    public string PlayerName;
    //玩家金币
    [HideInInspector]
    public int PlayerCoin;
    //玩家道具数量
    [HideInInspector]
    public int ItemCount;
    //当前关卡
    [HideInInspector]
    public int CurrentLevel;
    //当前困难关卡
    [HideInInspector]
    public int CurrentHardLevel;
    //当前奖杯完成关卡
    [HideInInspector]
    public int TrophyCompleteLevel;
    //当前段位
    [HideInInspector]
    public int CurrentRank;
    //当前语言
    [HideInInspector]
    public Language CurrentLanguage;
    //震动
    [HideInInspector]
    public bool IsVibrate = true;
    //音效
    [HideInInspector]
    public bool IsSound = true;
    //多语言字典
    [HideInInspector]
    public Dictionary<string, Dictionary<Language, string>> mLanguageDict = new Dictionary<string, Dictionary<Language, string>>();
    //是否第一次游戏
    [HideInInspector]
    public bool IsFirstGame = true;
    //行人游戏介绍是否显示过
    [HideInInspector]
    public bool IsPeoIntroduce = true;
    //信号灯游戏介绍是否显示过
    [HideInInspector]
    public bool IsLightIntroduce = true;
    //推土机游戏介绍是否显示过
    [HideInInspector]
    public bool IsBulldozerIntroduce = true;
    //游戏类型
    [HideInInspector]
    public GameType GameType = GameType.Main;
    public Dictionary<string, LevelstepsTemplate> lstLevelstepsTemplate = new();
    public Dictionary<string, TimeraceTemplate> lstTimeraceTemplate = new();
    public Dictionary<string, PlayernameTemplate> lstPlayernameTemplate = new();
    public Dictionary<string, RankrewardTemplate> lstRankrewardTemplate = new();
    public Dictionary<string, RanksectionTemplate> lstRanksectionTemplate = new();
    public Dictionary<string, RobotTemplate> lstRobotTemplate = new();
    public const int StartBossLevel = 20;//开始boss关卡
    public const int BossLevelInterval = 10;//boss关卡间隔
    public const int StartGiftLevel = 14;//开始礼物关卡
    public const int GiftLevelInterval = 10;//礼物关卡间隔
    public Sprite[] m_CoinSprites;//金币序列帧图片
    //是否是奖励关
    public bool IsRewardLevel(int level)
    {
        if (level >= StartGiftLevel && (level - StartGiftLevel) % GiftLevelInterval == 0)
        {
            return true;
        }
        return false;
    }
    //是否开启双倍奖励
    public bool IsDoubleReward;
    //是否弹出奖励
    [HideInInspector]
    public bool IsReward = false;
    //排位赛相关////////////////////////////////////////////////
    public PlayerInfo _selfPlayerInfo; // 0x10
    public List<PlayerInfo> _trophyRankingList; // 0x18
    //private TrophyInfo _selfTrophyInfo; // 0x20
    private List<RobotTemplate> _curRobotData; // 0x38
    //开始时间
    public DateTime StartDate;
    //是否已经开始排位赛
    [HideInInspector]
    public bool mIsStartRankingMatch = false;
    //竞速赛相关///////////////////////////////////////////////////
    public RacingPlayerItemData _RacingSelfPlayerInfo; // 玩家数据
    public List<RacingPlayerItemData> _playerInfoList; // 机器人数据
    //开始时间
    public DateTime RacingStartDate;
    //开始关卡数
    public int RacingStartLevel;
    //比赛是否进行中
    public bool IsCompetition = false;
    private void Awake()
    {
        // 确保单例模式
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        LoadGameData();
        LoadXmlFile("Xml/TrafficLanguage");
        LoadLevelStepsTemplat();
        LoadTimeraceTemplate();
        LoadPlayernameTemplate();
        LoadRankrewardTemplate();
        LoadRanksectionTemplate();
        LoadRobotTemplate();
        CurrentLanguage = GetSystemLanguage();
    }

    private void OnDestroy()
    {
        SaveGameData();
    }

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.numerator;
    }

    // Update is called once per frame
    void Update()
    {

    }
    // 保存游戏数据
    public void SaveGameData()
    {
        PlayerPrefs.SetInt("PlayerCoin", PlayerCoin);
        PlayerPrefs.SetInt("ItemCount", ItemCount);
        PlayerPrefs.SetInt("CurrentLevel", CurrentLevel);
        PlayerPrefs.SetInt("CurrentHardLevel", CurrentHardLevel);
        PlayerPrefs.SetInt("TrophyCompleteLevel", TrophyCompleteLevel);
        PlayerPrefs.SetInt("CurrentRank", CurrentRank);
        PlayerPrefs.SetInt("IsVibrate", IsVibrate ? 1 : 0);
        PlayerPrefs.SetInt("IsSound", IsSound ? 1 : 0);
        PlayerPrefs.SetInt("IsFirstGame", IsFirstGame ? 1 : 0);
        PlayerPrefs.SetInt("IsPeoIntroduce", IsPeoIntroduce ? 1 : 0);
        PlayerPrefs.SetInt("IsLightIntroduce", IsLightIntroduce ? 1 : 0);
        PlayerPrefs.SetInt("IsBulldozerIntroduce", IsBulldozerIntroduce ? 1 : 0);
        PlayerPrefs.SetString("PlayerName", PlayerName);
        PlayerPrefs.SetInt("IsStartRankingMatch", mIsStartRankingMatch ? 1 : 0);
        PlayerPrefs.Save();
    }
    // 加载游戏数据
    public void LoadGameData()
    {
        PlayerCoin = PlayerPrefs.GetInt("PlayerCoin", 0);
        ItemCount = PlayerPrefs.GetInt("ItemCount", 4);
        CurrentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
        CurrentHardLevel = PlayerPrefs.GetInt("CurrentHardLevel", 0);
        TrophyCompleteLevel = PlayerPrefs.GetInt("TrophyCompleteLevel", 0);
        CurrentRank = PlayerPrefs.GetInt("CurrentRank", 1);
        IsVibrate = PlayerPrefs.GetInt("IsVibrate", 1) == 1;
        IsSound = PlayerPrefs.GetInt("IsSound", 1) == 1;
        IsFirstGame = PlayerPrefs.GetInt("IsFirstGame", 1) == 1;
        IsPeoIntroduce = PlayerPrefs.GetInt("IsPeoIntroduce", 1) == 1;
        IsLightIntroduce = PlayerPrefs.GetInt("IsLightIntroduce", 1) == 1;
        IsBulldozerIntroduce = PlayerPrefs.GetInt("IsBulldozerIntroduce", 1) == 1;
        PlayerName = PlayerPrefs.GetString("PlayerName", "Player_108");
        mIsStartRankingMatch = PlayerPrefs.GetInt("IsStartRankingMatch") == 1;
    }
    //获取系统语言
    public Language GetSystemLanguage()
    {
        SystemLanguage lang = Application.systemLanguage;
        switch (lang)
        {
            case SystemLanguage.Chinese: //中文
                return Language.Chinese;
            case SystemLanguage.ChineseSimplified: //简体中文
                return Language.Chinese;
            case SystemLanguage.ChineseTraditional: //繁体中文
                return Language.Chinese;
            case SystemLanguage.English: //英文
                return Language.English;
            // case SystemLanguage.Japanese: //日文
            //     return Language.Japanese;
            // case SystemLanguage.Portuguese: //葡萄牙语
            //     return Language.Portuguese;
            // case SystemLanguage.Spanish: //西班牙语
            //     return Language.Spanish;
            // case SystemLanguage.German: //德语
            //     return Language.German;
            // case SystemLanguage.French: //法语
            //     return Language.French;
            // case SystemLanguage.Korean: //韩语
            //     return Language.Korean;
            // case SystemLanguage.Arabic: //阿拉伯语
            //     return Language.Arabic;
            // case SystemLanguage.Russian: //俄语
            //     return Language.Russian;
            default:
                return Language.English;
        }
    }
    void LoadXmlFile(string fileName)
    {
        // 加载 XML 文件
        TextAsset xmlAsset = Resources.Load<TextAsset>(fileName);
        if (xmlAsset == null)
        {
            Debug.LogError("无法加载 XML 文件: " + fileName);
            return;
        }

        // 解析 XML 文件
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlAsset.text);

        // 处理 XML 数据
        XmlNodeList worksheetList = xmlDoc.GetElementsByTagName("Worksheet");
        foreach (XmlNode worksheet in worksheetList)
        {
            string sheetName = worksheet.Attributes["Name"].Value;
            Debug.Log("Worksheet: " + sheetName);

            XmlNodeList rowList = worksheet.SelectNodes("Row");
            foreach (XmlNode row in rowList)
            {
                XmlNodeList cellList = row.SelectNodes("Cell");
                var key = cellList[0].InnerText;
                //如果已经存在key，则跳过
                if (mLanguageDict.ContainsKey(key))
                {
                    continue;
                }
                mLanguageDict.Add(key, new Dictionary<Language, string>());
                foreach (XmlNode cell in cellList)
                {
                    string column = cell.Attributes["Column"].Value;
                    string value = cell.InnerText;
                    if (column != "Id:key")
                    {
                        try
                        {
                            Language language = (Language)Enum.Parse(typeof(Language), column, true);
                            mLanguageDict[key][language] = value;
                        }
                        catch (ArgumentException)
                        {
                            Debug.LogError("无效的语言枚举值: " + column);
                        }
                    }
                    //Debug.Log("Column: " + column + ", Value: " + value);
                }
            }
        }
    }
    public void LoadLevelStepsTemplat()
    {
        var obj = Resources.Load<TextAsset>("Template/LevelstepsTemplate");
        string v108 = obj.text;
        Dictionary<int, int> stepsData = new Dictionary<int, int>();
        string[] lines = obj.text.Split('\n');
        // 从第三行开始解析数据
        for (int i = 3; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;
            LevelstepsTemplate levelstepsTemplate = new LevelstepsTemplate();
            levelstepsTemplate.ReadData(line);
            var id = levelstepsTemplate.id;
            lstLevelstepsTemplate.Add(id.ToString(), levelstepsTemplate);
        }
    }
    public void LoadTimeraceTemplate()
    {
        var obj = Resources.Load<TextAsset>("Template/TimeraceTemplate");
        string v108 = obj.text;
        Dictionary<int, int> stepsData = new Dictionary<int, int>();
        string[] lines = obj.text.Split('\n');
        // 从第三行开始解析数据
        for (int i = 3; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;
            TimeraceTemplate timeraceTemplate = new TimeraceTemplate();
            timeraceTemplate.ReadData(line);
            var id = timeraceTemplate.id;
            lstTimeraceTemplate.Add(id.ToString(), timeraceTemplate);
        }
    }
    public void LoadPlayernameTemplate()
    {
        var obj = Resources.Load<TextAsset>("Template/PlayernameTemplate");
        string v108 = obj.text;
        Dictionary<int, int> stepsData = new Dictionary<int, int>();
        string[] lines = obj.text.Split('\n');
        // 从第三行开始解析数据
        for (int i = 3; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;
            PlayernameTemplate playernameTemplate = new PlayernameTemplate();
            playernameTemplate.ReadData(line);
            var id = playernameTemplate.id;
            lstPlayernameTemplate.Add(id.ToString(), playernameTemplate);
        }
    }
    public void LoadRankrewardTemplate()
    {
        var obj = Resources.Load<TextAsset>("Template/RankrewardTemplate");
        string v108 = obj.text;
        Dictionary<int, int> stepsData = new Dictionary<int, int>();
        string[] lines = obj.text.Split('\n');
        // 从第三行开始解析数据
        for (int i = 3; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;
            RankrewardTemplate rankrewardTemplate = new RankrewardTemplate();
            rankrewardTemplate.ReadData(line);
            var id = rankrewardTemplate.id;
            lstRankrewardTemplate.Add(id.ToString(), rankrewardTemplate);
        }
    }
    public void LoadRanksectionTemplate()
    {
        var obj = Resources.Load<TextAsset>("Template/RanksectionTemplate");
        string v108 = obj.text;
        Dictionary<int, int> stepsData = new Dictionary<int, int>();
        string[] lines = obj.text.Split('\n');
        // 从第三行开始解析数据
        for (int i = 3; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;
            RanksectionTemplate ranksectionTemplate = new RanksectionTemplate();
            ranksectionTemplate.ReadData(line);
            var id = ranksectionTemplate.id;
            lstRanksectionTemplate.Add(id.ToString(), ranksectionTemplate);
        }
    }
    public void LoadRobotTemplate()
    {
        var obj = Resources.Load<TextAsset>("Template/RobotTemplate");
        string v108 = obj.text;
        Dictionary<int, int> stepsData = new Dictionary<int, int>();
        string[] lines = obj.text.Split('\n');
        // 从第三行开始解析数据
        for (int i = 3; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;
            RobotTemplate robotTemplate = new RobotTemplate();
            robotTemplate.ReadData(line);
            var id = robotTemplate.id;
            lstRobotTemplate.Add(id.ToString(), robotTemplate);
        }
    }
    public string GetLanguageValue(string key)
    {
        if (mLanguageDict.ContainsKey(key))
        {
            if (mLanguageDict[key].ContainsKey(CurrentLanguage))
            {
                return mLanguageDict[key][CurrentLanguage];
            }
        }
        return key;
    }
    //排位赛相关，需要移植到GlobalManager，设置为全局
    public void RefreshTrophyRankingList(bool isUpdateRanking = true)
    {
        InitRobotData();
        UpdateTrophyRanking(isUpdateRanking);
        List<PlayerInfo> trophyRankingList = _trophyRankingList;
        if (trophyRankingList == null)
        {
            throw new NullReferenceException("trophyRankingList == null");
        }
        trophyRankingList.Sort();
        int index = 0;
        foreach (PlayerInfo player in trophyRankingList)
        {
            player.RankIndex = index;
            SetRewardPropsByRank(index, player);
            index++;
        }
    }

    // RVA: 0x9D78C0 Offset: 0x9D78C0 VA: 0x9D78C0
    public void UpdateTrophyRanking(bool isUpdateRanking)
    {
        if (_trophyRankingList != null)
        {
            if (!isUpdateRanking)
            {
                return;
            }
        }
        else
        {
            List<PlayerInfo> v7 = new List<PlayerInfo>(99);
            _trophyRankingList = v7;
            for (int i = 1; i < 99; i++)
            {
                PlayerInfo v13 = new PlayerInfo();
                v13.Id = i;
                TrophyProp v16 = new TrophyProp();
                v16.Trophy = new PropData(Prop.Trophy, 0);
                v13.TrophyProp = v16;
                _trophyRankingList.Add(v13);
            }
            _selfPlayerInfo = new PlayerInfo()
            {
                Id = -1,
                TrophyProp = new TrophyProp()
                {
                    Trophy = new PropData(Prop.Trophy, 0)
                },
                Name = PlayerName
            };
            _trophyRankingList.Add(_selfPlayerInfo);
        }
        int v18 = 0;
        while (v18 < _trophyRankingList.Count)
        {
            PlayerInfo playerInfo = _trophyRankingList[v18];
            if (playerInfo == null)
            {
                throw new NullReferenceException("playerInfo == null");
            }
            if (playerInfo.Id >= 0)
            {
                string robotName = GetRobotName(playerInfo.Id);
                playerInfo.Name = robotName;
                playerInfo.TrophyProp.Trophy.Count = GetScore(playerInfo);
            }
            ++v18;
        }
    }
    public void SetRewardPropsByRank(int rankIndex, PlayerInfo player)
    {
        int v8 = GlobalManager.Instance.CurrentRank + 1;
        int v9 = 0;
        if (v8 >= 6)
        {
            v9 = 6;
        }
        else
        {
            v9 = v8;
        }
        if (v9 <= 1)
        {
            v9 = 1;
        }
        Dictionary<string, RankrewardTemplate> lstRankrewardTemplate = GlobalManager.Instance.lstRankrewardTemplate;
        if (lstRankrewardTemplate == null)
        {
            throw new NullReferenceException("lstRankrewardTemplate == null");
        }
        Dictionary<string, RanksectionTemplate> lstRanksectionTemplate = GlobalManager.Instance.lstRanksectionTemplate;
        if (lstRanksectionTemplate == null)
        {
            throw new NullReferenceException("lstRanksectionTemplate == null");
        }
        string TrophyIndex = v9.ToString();
        RanksectionTemplate curTemp = lstRanksectionTemplate[TrophyIndex];
        if (curTemp == null)
        {
            throw new NullReferenceException("curTemp == null");
        }
        int v16 = 0;
        if (rankIndex >= 3)
        {
            if (curTemp.improve <= rankIndex)
            {
                v16 = curTemp.decline >= rankIndex ? 5 : 6;
            }
            else
            {
                v16 = 4;
            }
        }
        else
        {
            v16 = rankIndex + 1;
        }
        object[] v17 = new object[2];
        v17[0] = v9;
        v17[1] = v16;
        string v22 = string.Format("{0}_{1}", v17);
        RankrewardTemplate v23 = lstRankrewardTemplate[v22];
        if (v23 == null || player == null)
        {
            throw new NullReferenceException("v23 == null || player == null");
        }
        int reward1count = v23.reward1count;
        TrophyProp TrophyProp = player.TrophyProp;
        int v26 = reward1count > 0 ? 1 : 0;
        int v27 = reward1count <= 0 ? 1 : 2;
        int v28 = v23.reward2count >= 1 ? v27 : v26;
        PropData[] v29 = new PropData[v28];
        if (TrophyProp == null)
        {
            throw new NullReferenceException("TrophyProp == null");
        }
        TrophyProp.PropArray = v29;
        TrophyProp.PropArray[0].Type = (Prop)v23.reward1;
        TrophyProp.PropArray[0].Count = reward1count;
        int reward2count = v23.reward2count;
        if (reward2count < 1)
        {
            return;
        }
        if (TrophyProp == null)
        {
            throw new NullReferenceException("TrophyProp == null");
        }
        if (TrophyProp.PropArray == null)
        {
            throw new NullReferenceException("TrophyProp.PropArray == null");
        }
        if (TrophyProp.PropArray.Length <= 1)
        {
            throw new NullReferenceException("TrophyProp.PropArray.Length <= 1");
        }
        TrophyProp.PropArray[1].Type = (Prop)v23.reward2;
        TrophyProp.PropArray[1].Count = reward2count;
    }
    private string GetRobotName(int index)
    {
        int Day = StartDate.Day;
        int Month = StartDate.Month;
        int Year = StartDate.Year;
        int v8 = StartDate.Day;
        int v9 = v8 + 3;
        if (v8 >= 0)
        {
            v9 = v8;
        }
        int v10 = v8 - (int)(v9 & 0xFFFFFFFC);
        int v11 = 0;
        if (v10 <= 1)
        {
            v11 = 1;
        }
        else
        {
            v11 = v10;
        }
        Dictionary<string, PlayernameTemplate> lstPlayernameTemplate = GlobalManager.Instance.lstPlayernameTemplate;
        if (lstPlayernameTemplate == null)
        {
            throw new NullReferenceException("lstPlayernameTemplate == null");
        }
        int v17 = (Month + Day + Year + v11 * index) % (lstPlayernameTemplate.Count - 2) + 1;
        string v23 = v17.ToString();
        PlayernameTemplate v18 = lstPlayernameTemplate[v23];
        if (v18 == null)
        {
            throw new NullReferenceException("v18 == null");
        }
        return v18.name;
    }
    private int GetScore(PlayerInfo player)
    {
        if (player == null)
        {
            throw new NullReferenceException("player == null");
        }
        int Id = player.Id;
        if (Id < 0)
        {
            return -1;
        }
        DateTime StartDate = this.StartDate;
        int Day = StartDate.Day;
        int Month = StartDate.Month;
        List<RobotTemplate> curRobotData = _curRobotData;
        if (curRobotData == null)
        {
            throw new NullReferenceException("curRobotData == null");
        }
        int v9 = player.Id;
        int v10 = (Month + Day * Id) % 3;
        int v11 = 0;
        foreach (RobotTemplate v12 in curRobotData)
        {
            if (v12 == null)
            {
                throw new NullReferenceException("v12 == null");
            }
            if (v12.id % 100 == GlobalManager.Instance.CurrentRank)
            {
                ++v11;
                v9 -= v12.section;
                if (v9 < 0)
                {
                    break;
                }
            }
        }
        RobotTemplate robot = curRobotData[v11];
        if (robot == null)
        {
            throw new NullReferenceException("robot == null");
        }
        int v15 = robot.result;
        int v16 = 0;
        if (v9 >= 0)
        {
            v16 = v9;
        }
        else
        {
            v16 = v9 + 1;
        }
        int v17 = v9 - (int)(v16 & 0xFFFFFFFE);
        DateTime v49 = StartDate;
        int v18 = v49.Month;
        int v19 = 0;
        if (v17 == 1)
        {
            v19 = 10;
        }
        else
        {
            v19 = -10;
        }
        int v20 = v49.Day * v19;
        int v21 = v15 + 10 * v9 + v18 * v19;
        int v22 = player.Id;
        int v23 = 0;
        if (v20 >= 0)
        {
            v23 = v20;
        }
        else
        {
            v23 = v20 | 1;
        }
        int v24 = v49.Day;
        int Hour = v49.Hour;
        int v26 = (int)(v49.Month * (v24 + v22 + Hour));
        float v27 = (float)Math.IEEERemainder((double)v26, 5.3f) * 0.01f;
        float v29 = (float)Math.IEEERemainder((double)v26, 100.0f) / 100.0f;
        float v30 = Math.Min(v29, 0.9f);
        float v31 = 0.0f;
        if (v29 < 0.2f)
        {
            v31 = 0.2f;
        }
        else
        {
            v31 = v30;
        }
        float ScoreByFuncIndex = GetScoreByFuncIndex(v10);
        if (ScoreByFuncIndex < 0.0f)
        {
            throw new NullReferenceException("ScoreByFuncIndex < 0.0f");
        }
        int v33 = v21 - (v23 >> 1);
        float v34 = ScoreByFuncIndex + v27;
        float v35 = Math.Min(v34, 1.0f);
        float v36 = 0.0f;
        if (v34 >= 0.0f)
        {
            v36 = v35;
        }
        else
        {
            v36 = 0.0f;
        }
        int TrophyCompleteLevel = GlobalManager.Instance.TrophyCompleteLevel;//开启段位怕行榜后的过关数
        float v39 = v36 * (float)v33;
        int v40 = (int)v39;
        bool v41 = v39 == float.PositiveInfinity;
        float v42 = (float)(v31 * (float)TrophyCompleteLevel) * 10.0f;
        int v43 = v41 ? int.MinValue : v40;
        int v44 = v42 == float.PositiveInfinity ? int.MinValue : (int)v42;
        int v45 = v44 + v43;
        if (v33 < 0)
        {
            throw new Exception("v33 < 0");
        }
        int v46 = v45 <= v33 ? v45 : v21 - (v23 >> 1);
        if (v45 >= 0)
        {
            return 10 * (v46 / 10);
        }
        else
        {
            return 0;
        }
    }
    private float GetScoreByFuncIndex(int funcIndex)
    {
        DateTime NowDateTime = GetNowDateTime();
        DateTime StartDate = this.StartDate;
        TimeSpan TimerSpan = NowDateTime - StartDate;
        float TotalSeconds = (float)TimerSpan.TotalSeconds;
        float v11 = TotalSeconds / 604800.0f;
        if (funcIndex == 2)
        {
            v11 = v11 * v11;
        }
        else if (funcIndex == 1)
        {
            v11 = SinScore(v11);
        }
        float result = Math.Min(v11, 1.0f);
        if (v11 < 0.0f)
        {
            return 0.0f;
        }
        return result;
    }
    private float SinScore(float delta)
    {
        return (float)Math.Sin(delta * 3.14159265 * 0.5);
    }
    private void InitRobotData()
    {
        _curRobotData = new List<RobotTemplate>();
        List<RobotTemplate> curRobotData = _curRobotData;
        if (curRobotData == null)
        {
            throw new NullReferenceException("curRobotData == null");
        }
        curRobotData.Clear();
        Dictionary<string, RobotTemplate> lstRobotTemplate = GlobalManager.Instance.lstRobotTemplate;
        if (lstRobotTemplate == null)
        {
            throw new NullReferenceException("lstRobotTemplate == null");
        }
        foreach (RobotTemplate v10 in lstRobotTemplate.Values)
        {
            if (v10 == null)
            {
                throw new NullReferenceException("v10 == null");
            }
            int id = v10.id;
            if (id / 100 == GlobalManager.Instance.CurrentRank)//当前段位
            {
                List<RobotTemplate> v12 = _curRobotData;
                if (v12 == null)
                {
                    throw new NullReferenceException("v12 == null");
                }
                v12.Add(v10);
            }
        }
    }
    private DateTime GetNowDateTime()
    {
        return DateTime.Now;
    }
    //获取当前排名
    public int GetRankIndex()
    {
        if (_trophyRankingList == null)
        {
            throw new NullReferenceException("_trophyRankingList == null");
        }
        for (int i = 0; i < _trophyRankingList.Count; i++)
        {
            PlayerInfo playerInfo = _trophyRankingList[i];
            if (playerInfo == null)
            {
                throw new NullReferenceException("playerInfo == null");
            }
            if (playerInfo.Id == -1)
            {
                return i;
            }
        }
        return -1;
    }
    /////////////////////////////竞速赛相关
    public void RefreshPlayerInfoList()
    {
        if (_playerInfoList == null)
        {
            List<RacingPlayerItemData> v6 = new List<RacingPlayerItemData>();
            this._playerInfoList = v6;
            for (int i = 0; i < 4; i++)
            {
                RacingPlayerItemData v10 = new();
                v10.Id = i;
                v10.Name = GetRacingRobotName(i);
                v10.CompleteTime = GetCompleteTime(i);
                _playerInfoList.Add(v10);
            }
            _RacingSelfPlayerInfo = new RacingPlayerItemData
            {
                Id = -1,
                Name = GlobalManager.Instance.PlayerName,
                CompleteLevel = GlobalManager.Instance.CurrentLevel - RacingStartLevel
            };
            _playerInfoList.Add(_RacingSelfPlayerInfo);
        }
        int v15 = 0;
        while (v15 < _playerInfoList.Count)
        {
            RacingPlayerItemData obj = _playerInfoList[v15];
            if (obj.Id >= 0)
            {
                RacingPlayerItemData obj2 = _playerInfoList[v15];
                obj2.CompleteLevel = GetLevelScore(obj.Id);
            }
            else
            {
                RacingPlayerItemData obj3 = _playerInfoList[v15];
                obj3.CompleteLevel = GlobalManager.Instance.CurrentLevel - RacingStartLevel;
            }
            ++v15;
        }
        _playerInfoList.Sort();
    }
    private string GetRacingRobotName(int index)
    {
        int Hour = RacingStartDate.Hour;
        int Second = RacingStartDate.Second;
        int Minute = RacingStartDate.Minute;
        Dictionary<string, PlayernameTemplate> lstPlayernameTemplate = GlobalManager.Instance.lstPlayernameTemplate;
        if (lstPlayernameTemplate == null)
        {
            throw new NullReferenceException("lstPlayernameTemplate == null");
        }
        int v13 = (index + Hour * index + Minute * Second) % (lstPlayernameTemplate.Count - 2) + 1;
        string v18 = v13.ToString();
        PlayernameTemplate v14 = lstPlayernameTemplate[v18];
        if (v14 == null)
        {
            throw new NullReferenceException("v14 == null");
        }
        return v14.name;
    }
    private int GetCompleteTime(int index)
    {
        Dictionary<string, TimeraceTemplate> lstTimeraceTemplate = GlobalManager.Instance.lstTimeraceTemplate;
        if (lstTimeraceTemplate == null)
        {
            throw new NullReferenceException("lstTimeraceTemplate == null");
        }
        string indexStr = index.ToString();
        TimeraceTemplate curTemp = lstTimeraceTemplate[indexStr];
        if (curTemp == null)
        {
            throw new NullReferenceException("curTemp == null");
        }
        return 30 * this.GetRealInter(curTemp, index);
    }
    private int GetLevelScore(int index)
    {
        Dictionary<string, TimeraceTemplate> lstTimeraceTemplate = GlobalManager.Instance.lstTimeraceTemplate;
        if (lstTimeraceTemplate == null)
        {
            throw new NullReferenceException("lstTimeraceTemplate == null");
        }
        string indexStr = index.ToString();
        TimeraceTemplate curTemp = lstTimeraceTemplate[indexStr];
        if (curTemp == null)
        {
            throw new NullReferenceException("curTemp == null");
        }
        TimeSpan TimerSpan = DateTime.Now - RacingStartDate;
        double TotalSeconds = TimerSpan.TotalSeconds;
        int RealInter = this.GetRealInter(curTemp, index);
        int v14 = TotalSeconds == double.PositiveInfinity ? int.MinValue : (int)TotalSeconds;
        return v14 / RealInter;
    }
    private int GetRealInter(TimeraceTemplate curTemp, int index)
    {
        int interval = curTemp.interval;
        DateTime RacingStartDate = this.RacingStartDate;
        int Second = RacingStartDate.Second;
        int Minute = RacingStartDate.Minute;
        int Hour = RacingStartDate.Hour;
        int v8 = (index & 1) != 0 ? -1 : 1;
        return interval + (Second * index + Hour * Minute) % curTemp.berlin * v8;
    }
    public int GetPlayerRankIndex()
    {
        int v3 = 0;
        while (true)
        {
            List<RacingPlayerItemData> playerInfoList = this._playerInfoList;
            if (playerInfoList == null)
            {
                throw new NullReferenceException("playerInfoList == null");
            }
            if (v3 >= playerInfoList.Count)
            {
                return -1;
            }
            RacingPlayerItemData playerInfo = playerInfoList[v3];
            if (playerInfo == null)
            {
                throw new NullReferenceException("playerInfo == null");
            }
            ++v3;
            if (playerInfo.Id < 0)
            {
                return v3;
            }
        }
    }
}
