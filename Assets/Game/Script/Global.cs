using System;
public class Global
{
}

[Serializable]
public class CarInfo
{
    public CarType type; //车型 1-2m小车   2-3m大车
    public int posX; //车头位置
    public int posY; //车头位置
    public int dir; //方向 1上 2下  3左  4右
    public int turn; //转向 1直行 2左转 3右转 4左掉头 5右掉头

    public CarInfo(CarType type, int px, int py, int dir, int turn)
    {
        this.type = type;
        posX = px;
        posY = py;
        this.dir = dir;
        this.turn = turn;
    }
}

[Serializable]
public class PeoInfoItem
{//行人信息
    public PeoInfoItem(IntPos p1, IntPos p2)
    {
        pos2 = p2;
        pos1 = p1;
    }

    public IntPos pos1;
    public IntPos pos2;
}
[Serializable]
public class TrafficLightInfo
{//信号灯信息
    public IntPos pos;
    public int dir;//方向 1上 2右  3下  4左
    //红灯时间
    public float redTime = 3;
    //绿灯时间
    public float greenTime = 5;
    public TrafficLightInfo(IntPos p, int d)
    {
        pos = p;
        dir = d;
    }
}
//路障信息
[Serializable]
public class BlockInfo
{
    public IntPos pos;
    public BlockInfo(IntPos p)
    {
        pos = p;
    }
}
public enum GameStatu
{ //游戏状态
    preparing,//准备中
    playing,//进行中
    waiting,//等待
    finish,// 结束
    faled//失败
}
public enum CarType
{
    Small = 1, //车型 1-2m小车
    Big = 2, //车型 2-3m大车
    Bulldozer = 3, //推土机 1-2m
}
//语言
public enum Language
{
    English = 1,//英语
    Chinese = 2,//中文
    Japanese = 3,//日语
    Portuguese = 4,//葡萄牙语
    Spanish = 5,//西班牙语
    German = 6,//德语
    French = 7,//法语
    Korean = 8,//韩语
    Arabic = 9,//阿拉伯语
    Russian = 10,//俄语
}
//游戏介绍类型
public enum GameIntroType
{
    //行人
    People = 1,
    //信号灯
    TrafficLight = 2,
    //推土机
    Bulldozer = 3,
}
//失败原因
public enum FailReason
{
    PeopleCrash = 1, //行人碰撞
    ActionNotEnough = 2,//行动力不足
}
public class IntPos
{
    public IntPos(int x_, int y_)
    {
        x = x_;
        y = y_;
    }

    public int x;
    public int y;
}
public enum GameType
{
    Main = 10,//主游戏
    ChallengeHard = 11,//挑战模式
    ChallengeSuperHard = 12,//超级挑战
    Other = 13//其他
}
public struct PropData
{
    // Fields
    public static PropData DefaultPropData;
    public Prop Type;
    public int Count;


    public PropData(Prop type, int count)
    {
        this.Type = type;
        this.Count = count;
    }


    public PropData(Prop type)
    {
        this.Type = type;
        this.Count = 1;
    }


    public PropData Clone()
    {
        return this;
    }

    static PropData()
    {
        DefaultPropData.Type = Prop.Coin;//1
        DefaultPropData.Count = 0xA;//10
    }

}
public enum Prop
{
    Coin = 1,//金币
    Hint = 2,//提示
    RemoveAd = 3,//去广告
    Ballon = 4,//气球
    Trophy = 5//奖杯
}
