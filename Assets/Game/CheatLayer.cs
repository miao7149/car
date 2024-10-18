using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheatLayer : MonoBehaviour {
    public GameObject cheatPanel;

    public Text msg;

    public void OnTouchButton(string tag) {
        switch (tag) {
            case "cheat":
                cheatPanel.SetActive(!cheatPanel.activeSelf);
                break;
            case "checkCar":
                ShowMessage(GameManager.Instance.DebugGetCarString());

                break;
            case "win":
                GameManager.Instance.SetGameStatu(GameStatu.finish);
                break;

            case "up":
                GlobalManager.Instance.CurrentLevel += 100;
                PlayerPrefs.SetInt("CurrentLevel", GlobalManager.Instance.CurrentLevel);
                MenuManager.Instance().SetLevelList();
                break;
            case "down":

                GlobalManager.Instance.CurrentLevel -= 100;
                PlayerPrefs.SetInt("CurrentLevel", GlobalManager.Instance.CurrentLevel);
                MenuManager.instance.SetLevelList();
                break;
            case "coin":
                GlobalManager.Instance.PlayerCoin = 99999;

                PlayerPrefs.SetInt("PlayerCoin", 99999);
                PlayerPrefs.Save();
                break;
            case "reward":

                GlobalManager.Instance._selfPlayerInfo.TrophyProp.Trophy.Count += 1000;
                GlobalManager.Instance._selfTrophyCount += 1000;
                PlayerPrefs.SetInt("TrophyCount", GlobalManager.Instance._selfTrophyCount);
                PlayerPrefs.Save();
                break;

            case "sign":
                var signCount = PlayerPrefs.GetInt("SignCount", 0);
                signCount++;
                PlayerPrefs.SetInt("SignCount", signCount);
                PlayerPrefs.Save();
                break;
        }
    }

    public void ShowMessage(string message) {
        msg.text = message;
    }
}
