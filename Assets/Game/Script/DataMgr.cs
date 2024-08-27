using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using Newtonsoft.Json;
public class DataMgr : MonoBehaviour
{
    void Start()
    {

    }
    public void LoadLevelData(int level)
    {
        //加载json文件
        TextAsset json = Resources.Load<TextAsset>("LevelData/" + level);
        // 使用 Newtonsoft.Json 解析 json 数据
        LevelDataConfig levelData = JsonConvert.DeserializeObject<LevelDataConfig>(json.text);
    }
}