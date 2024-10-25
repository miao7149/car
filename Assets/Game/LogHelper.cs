using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

public class GameLog {
    public int EnvFlag {
        get;
        set;
    }

    public string GameVersion {
        get;
        set;
    }

    public string DataVersion {
        get;
        set;
    }

    public MyUser MyUser {
        get;
        set;
    }

    public MyDevice MyDevice {
        get;
        set;
    }

    public string Remark {
        get;
        set;
    }

    public MyGameEvent MyGameEvent {
        get;
        set;
    }

    public string MyEventTime;
}

public class MyUser {
    public MyUser() {
        TheDateTime = System.DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
        UserId = SystemInfo.deviceUniqueIdentifier;
    }

    public string UserId {
        get;
        set;
    }

    public string TheDateTime {
        get;
        set;
    }
}

public class MyDevice {
    public MyDevice() {
        TheType = SystemInfo.deviceType.ToString();
        Unild = SystemInfo.deviceUniqueIdentifier;
        Name = SystemInfo.deviceName;
    }

    public string TheType {
        get;
        set;
    }

    public string Unild {
        get;
        set;
    }

    public string Name {
        get;
        set;
    }
}

public class MyGameEvent {
    public string SceneName {
        get;
        set;
    }

    public string ComponentPage {
        get;
        set;
    }

    public Dictionary<string, object> ArgDict {
        get;
        set;
    }

    public string MyEventType {
        get;
        set;
    }
}

// public enum GameEventType {
//     Login,
//     Logout,
//     StartGame,
//     EndGame,
//     ClickButton,
//     ClickItem,
//     ClickMap,
//     ClickNpc,
//     ClickShop,
//     ClickBack,
//     ClickMenu,
//     ClickClose
// }

public class LogHelper {
    public static IEnumerator LogToServer(string eventType, Dictionary<string, object> argDict, string remark = "", string componentPage = "") {
        GameLog log = new() {
            EnvFlag = 1,
            GameVersion = Application.version,
            DataVersion = "1.0.6",
            MyUser = new(),
            MyDevice = new(),
            Remark = remark,
            MyGameEvent = new() {
                SceneName = SceneManager.GetActiveScene().name,
                MyEventType = eventType,
                ComponentPage = componentPage,
                ArgDict = argDict
            },
            MyEventTime = System.DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
        };
        string url = "https://console.gamestar6688.com/log/traffic";
        string json = JsonConvert.SerializeObject(log);


        var req = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        req.uploadHandler = new UploadHandlerRaw(jsonToSend);
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");
        yield return req.SendWebRequest();
        if (req.result != UnityWebRequest.Result.Success) {
            Debug.LogError("日志上报异常: " + req.error);
        }
        else if (true) {
            Debug.Log("日志上报成功: " + json);
        }

        req.Dispose();
    }
}
