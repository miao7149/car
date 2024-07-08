using System;

public class Global {
}

[Serializable]
public class CarInfo {
    public int type; //车型 1-2m小车   2-3m大车
    public int posX; //车头位置
    public int posY; //车头位置
    public int dir; //方向 1上 2右  3下  4左
    public int turn; //转向 1直行 2左转 3右转 4左掉头 5右掉头

    public CarInfo(int type, int px, int py, int dir, int turn) {
        this.type = type;
        posX = px;
        posY = py;
        this.dir = dir;
        this.turn = turn;
    }
}

[Serializable]
public class PeoInfoItem {
    public PeoInfoItem(IntPos p1, IntPos p2) {
        pos2 = p2;
        pos1 = p1;
    }

    public IntPos pos1;
    public IntPos pos2;
}

public enum GameStatu {
    preparing,
    playing,
    waiting,
    finish,
    faled
}

public class IntPos {
    public IntPos(int x_, int y_) {
        x = x_;
        y = y_;
    }

    public int x;
    public int y;
}
