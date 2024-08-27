using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDataConfig
{
    public BoardSize boardSize;
    public List<List<BlockData>> BlockDatas;
    public List<CarData> carDatas;
    public List<RoadBlock> RoadTemplates;
    public List<StoneForkliftData> StoneForkliftDatas;
    public List<TrafficLightData> TrafficLightTemplates;
    public List<SideWalkData> SideWalkTemplates;
}
public class BlockData//道路块
{
    public int[] Location;//位置
    public int BlockType;//类型
    public int Cross;//是否是十字路口
}
public class BoardSize//棋盘大小
{
    public int x;//棋盘大小
    public int y;//棋盘大小
    public float magnitude;//平方根
    public float sqrMagnitude;//平方
}
public class RoadBlock//道路
{
    public int[][] RoadBlocks;//道路
    public bool IsDoubleRoad;//是否是双向道路
    public int LineIndex;//道路索引
    public int Axis;//道路方向
}
public class CarData//车辆
{
    public int[] Location;//位置
    public float speed;//速度
    public int SelfForward;//自身方向
    public int Length;//车长
    public int MoveType;//移动类型
}
public class TrafficLightData//红绿灯
{
    public int[] Location;//位置
    public int SelfForward;//自身方向
}
public class SideWalkData//人行道
{
    public int[] Locations;//位置
}
public class StoneForkliftData //石头和叉车
{
    public int[] ForkliftLocation;//位置
    public int[] StoneLocation;//位置
}
public class LevelstepsTemplate
{
    public int id; // 0x10
    public int stepsadd; // 0x14

    // Methods
    // RVA: 0x9C268C Offset: 0x9C268C VA: 0x9C268C
    public void ReadData(string _line)
    {
        char[] v5 = new char[] { '|' };
        string[] v6 = _line.Split(v5);
        if (v6 == null || v6.Length <= 0)
        {
            throw new Exception("v6.Length <= 0");
        }
        this.id = int.Parse(v6[0]);
        this.stepsadd = int.Parse(v6[1]);
    }

    // RVA: 0x9C2750 Offset: 0x9C2750 VA: 0x9C2750
    public LevelstepsTemplate() { }
}
