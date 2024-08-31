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

    //玩家金币
    [HideInInspector]
    public int PlayerCoin;
    //玩家道具数量
    [HideInInspector]
    public int ItemCount;
    //当前关卡
    [HideInInspector]
    public int CurrentLevel;
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
    //是否弹出奖励
    [HideInInspector]
    public bool IsReward = false;
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
        PlayerPrefs.SetInt("IsVibrate", IsVibrate ? 1 : 0);
        PlayerPrefs.SetInt("IsSound", IsSound ? 1 : 0);
        PlayerPrefs.SetInt("IsFirstGame", IsFirstGame ? 1 : 0);
        PlayerPrefs.SetInt("IsPeoIntroduce", IsPeoIntroduce ? 1 : 0);
        PlayerPrefs.SetInt("IsLightIntroduce", IsLightIntroduce ? 1 : 0);
        PlayerPrefs.SetInt("IsBulldozerIntroduce", IsBulldozerIntroduce ? 1 : 0);
        PlayerPrefs.Save();
    }
    // 加载游戏数据
    public void LoadGameData()
    {
        PlayerCoin = PlayerPrefs.GetInt("PlayerCoin", 0);
        ItemCount = PlayerPrefs.GetInt("ItemCount", 4);
        CurrentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
        IsVibrate = PlayerPrefs.GetInt("IsVibrate", 1) == 1;
        IsSound = PlayerPrefs.GetInt("IsSound", 1) == 1;
        IsFirstGame = PlayerPrefs.GetInt("IsFirstGame", 1) == 1;
        IsPeoIntroduce = PlayerPrefs.GetInt("IsPeoIntroduce", 1) == 1;
        IsLightIntroduce = PlayerPrefs.GetInt("IsLightIntroduce", 1) == 1;
        IsBulldozerIntroduce = PlayerPrefs.GetInt("IsBulldozerIntroduce", 1) == 1;
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
            this.lstLevelstepsTemplate.Add(id.ToString(), levelstepsTemplate);
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
}
