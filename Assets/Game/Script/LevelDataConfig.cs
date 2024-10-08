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
public class TimeraceTemplate // TypeDefIndex: 3607
{
    // Fields
    public int id; // 0x10
    public int interval; // 0x14
    public int berlin; // 0x18

    // Methods
    // RVA: 0x9C2BF4 Offset: 0x9C2BF4 VA: 0x9C2BF4
    public void ReadData(string _line)
    {
        char[] v5 = new char[] { '|' };
        string[] v6 = _line.Split(v5);
        if (v6 == null || v6.Length <= 0)
        {
            throw new Exception("v6.Length <= 0");
        }
        this.id = int.Parse(v6[0]);
        this.interval = int.Parse(v6[1]);
        this.berlin = int.Parse(v6[2]);
    }

    // RVA: 0x9C2CD4 Offset: 0x9C2CD4 VA: 0x9C2CD4
    public TimeraceTemplate() { }
}
public class PlayernameTemplate // TypeDefIndex: 3602
{
    // Fields
    public int id; // 0x10
    public string name; // 0x18

    // Methods
    // RVA: 0x9C2758 Offset: 0x9C2758 VA: 0x9C2758
    public void ReadData(string _line)
    {
        char[] v5 = new char[] { '|' };
        string[] v6 = _line.Split(v5);
        if (v6 == null || v6.Length <= 0)
        {
            throw new Exception("v6.Length <= 0");
        }
        this.id = int.Parse(v6[0]);
        this.name = v6[1];
    }

    // RVA: 0x9C2818 Offset: 0x9C2818 VA: 0x9C2818
    public PlayernameTemplate() { }
}
public class RankrewardTemplate // TypeDefIndex: 3604
{
    // Fields
    public string id; // 0x10
    public int reward1; // 0x18
    public int reward1count; // 0x1C
    public int reward2; // 0x20
    public int reward2count; // 0x24

    // Methods

    // RVA: 0x9C2904 Offset: 0x9C2904 VA: 0x9C2904
    public void ReadData(string _line)
    {
        char[] v5 = new char[] { '|' };
        string[] v6 = _line.Split(v5);
        if (v6 == null || v6.Length <= 0)
        {
            throw new Exception("v6.Length <= 0");
        }
        this.id = v6[0];
        this.reward1 = int.Parse(v6[1]);
        this.reward1count = int.Parse(v6[2]);
        this.reward2 = int.Parse(v6[3]);
        this.reward2count = int.Parse(v6[4]);
    }

    // RVA: 0x9C2A1C Offset: 0x9C2A1C VA: 0x9C2A1C
    public RankrewardTemplate() { }
}
public class RanksectionTemplate // TypeDefIndex: 3605
{
    // Fields
    public int id; // 0x10
    public int improve; // 0x14
    public int decline; // 0x18

    // Methods
    // RVA: 0x9C2A24 Offset: 0x9C2A24 VA: 0x9C2A24
    public void ReadData(string _line)
    {
        char[] v5 = new char[] { '|' };
        string[] v6 = _line.Split(v5);
        if (v6 == null || v6.Length <= 0)
        {
            throw new Exception("v6.Length <= 0");
        }
        this.id = int.Parse(v6[0]);
        this.improve = int.Parse(v6[1]);
        this.decline = int.Parse(v6[2]);
    }

    // RVA: 0x9C2B04 Offset: 0x9C2B04 VA: 0x9C2B04
    public RanksectionTemplate() { }
}
public class RobotTemplate // TypeDefIndex: 3606
{
    // Fields
    public int id; // 0x10
    public int section; // 0x14
    public int result; // 0x18

    // Methods
    // RVA: 0x9C2B0C Offset: 0x9C2B0C VA: 0x9C2B0C
    public void ReadData(string _line)
    {
        char[] v5 = new char[] { '|' };
        string[] v6 = _line.Split(v5);
        if (v6 == null || v6.Length <= 0)
        {
            throw new Exception("v6.Length <= 0");
        }
        this.id = int.Parse(v6[0]);
        this.section = int.Parse(v6[1]);
        this.result = int.Parse(v6[2]);
    }

    // RVA: 0x9C2BEC Offset: 0x9C2BEC VA: 0x9C2BEC
    public RobotTemplate() { }
}
