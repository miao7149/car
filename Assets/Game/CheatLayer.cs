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
                GlobalManager.Instance.CurrentLevel++;
                PlayerPrefs.SetInt("CurrentLevel", GlobalManager.Instance.CurrentLevel);
                MenuManager.Instance().SetLevelList();
                break;
            case "down":

                GlobalManager.Instance.CurrentLevel--;
                PlayerPrefs.SetInt("CurrentLevel", GlobalManager.Instance.CurrentLevel);
                MenuManager.instance.SetLevelList();
                break;
        }
    }

    public void ShowMessage(string message) {
        msg.text = message;
    }
}
