using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class RacingPlayerItemData
{
    //名称
    public string playerName;
    //过关数
    public int playerLevels;
    //关卡增长速度
    public float levelSpeed;
}
public class RacingCompetition : MonoBehaviour
{
    private const int TotalLevels = 30; // 总关卡数
    private const float TotalTime = 3600f; // 一小时倒计时
    private float remainingTime; // 剩余时间
    private RacingPlayerItemData mPlayerData; // 玩家数据
    private RacingPlayerItemData[] robotLevels = new RacingPlayerItemData[4]; // 机器人数据
    private bool competitionEnded; // 竞赛是否结束
    public List<RacingPlayerItem> m_PlayerItems = new List<RacingPlayerItem>();//排行榜Item
    //倒计时文本
    public TextMeshPro m_CountDownText;

    void Start()
    {
    }
    public void StartCompetition()
    {
        // 获取当前时间
        float currentTime = Time.realtimeSinceStartup;

        // 获取上次保存的开始时间
        float startTime = PlayerPrefs.GetFloat("StartTime", currentTime);
        remainingTime = TotalTime - (currentTime - startTime);

        // 如果剩余时间小于等于0，竞赛结束
        if (remainingTime <= 0)
        {
            remainingTime = 0;
            competitionEnded = true;
            Debug.Log("比赛结束，玩家未完成竞赛");
        }
        else
        {
            InitPlayerData();
            competitionEnded = false;
        }
        // 保存开始时间
        PlayerPrefs.SetFloat("StartTime", currentTime);
    }
    public void InitPlayerData()
    {
        mPlayerData = new RacingPlayerItemData();
        mPlayerData.playerName = PlayerPrefs.GetString("PlayerName", "Player");
        mPlayerData.playerLevels = PlayerPrefs.GetInt("PlayerLevels", 0);
        mPlayerData.levelSpeed = 0;
        for (int i = 0; i < robotLevels.Length; i++)
        {
            robotLevels[i].playerLevels = PlayerPrefs.GetInt("RobotLevels" + i, 0);
            var name = PlayerPrefs.GetString("RobotName" + i, "");
            if (string.IsNullOrEmpty(name))
            {
                PlayerPrefs.SetString("RobotName" + i, "Robot" + i);
                name = "Robot" + i;
            }
            robotLevels[i].playerName = name;
            robotLevels[i].levelSpeed = 1 - i * 0.25f; // 机器人关卡增长速度递减 1, 0.75, 0.5, 0.25
        }
    }
    void Update()
    {
        if (!competitionEnded)
        {
            remainingTime -= Time.deltaTime;

            if (remainingTime <= 0)
            {
                remainingTime = 0;
                competitionEnded = true;
                Debug.Log("比赛结束，玩家未完成竞赛");
            }
            else
            {
                //更新倒计时文本，以分钟为单位
                m_CountDownText.text = string.Format("{0:D2}:{1:D2}", (int)remainingTime / 60, (int)remainingTime % 60);
            }
        }
    }

    void UpdateRobotLevels()
    {
        for (int i = 0; i < robotLevels.Length; i++)
        {
            // 随机增加机器人的过关数
            if (Random.value < 0.01f * (1 + mPlayerData.playerLevels / 10f))
            {
                robotLevels[i].playerLevels = Mathf.Min(robotLevels[i].playerLevels + 1, TotalLevels);
                PlayerPrefs.SetInt("RobotLevels" + i, robotLevels[i].playerLevels);
            }
        }
    }

    int GetPlayerRank(List<RacingPlayerItemData> allLevels, int playerLevels)
    {
        int rank = 1;
        foreach (var item in allLevels)
        {
            if (item.playerLevels > playerLevels)
            {
                rank++;
            }
        }
        return rank;
    }

    public void PlayerPassLevel()
    {
        if (!competitionEnded)
        {
            mPlayerData.playerLevels = Mathf.Min(mPlayerData.playerLevels + 1, TotalLevels);
            PlayerPrefs.SetInt("PlayerLevels", mPlayerData.playerLevels);

            if (mPlayerData.playerLevels >= TotalLevels)
            {
                competitionEnded = true;
                Debug.Log("玩家完成竞赛，返回名次");
                // 计算玩家名次
                List<RacingPlayerItemData> allLevels = new List<RacingPlayerItemData>(robotLevels);
                allLevels.Add(mPlayerData);
                allLevels.Sort((a, b) => b.playerLevels.CompareTo(a.playerLevels)); // 降序排序
                int playerRank = GetPlayerRank(allLevels, mPlayerData.playerLevels);
                //取出玩家数据
                RacingPlayerItem playerItem = new RacingPlayerItem();

            }
        }
    }
}
