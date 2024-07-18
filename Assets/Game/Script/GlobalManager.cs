using UnityEngine;
using System.Collections.Generic;

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
    public int PlayerCoin;
    //玩家道具数量
    public int ItemCount;
    //当前关卡
    public int CurrentLevel;

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
    }

    private void OnDestroy()
    {
        SaveGameData();
    }
    // 可以添加更多的全局管理方法和逻辑

    // Start is called before the first frame update
    void Start()
    {

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
    }
    // 加载游戏数据
    public void LoadGameData()
    {
        PlayerCoin = PlayerPrefs.GetInt("PlayerCoin", 10000);
        ItemCount = PlayerPrefs.GetInt("ItemCount", 100);
        CurrentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
    }
}
