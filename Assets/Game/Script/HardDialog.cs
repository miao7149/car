using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HardDialog : MonoBehaviour {
    public Image ani;

    public Sprite[] sprites;

    int index = 0;

    bool play = false;

    float cd = 0;

    public GameObject Root;

    public Animation aniShow;

    // Start is called before the first frame update
    void Start() {
        if (isHardLevel()) {
            DOVirtual.DelayedCall(2f, () => {
                Root.SetActive(true);
                aniShow.Play("ani_HardDialog");
            }).Play();
        }
    }

    public void AniCallBack() {
        aniShow.Play("ani_HardDialog_2");
    }

    private int hardlevel;

    bool isHardLevel() {
        int level = GlobalManager.Instance.CurrentLevel;
        if (level >= 25) {
            bool a = (level - 5) % 10 == 0;
            if (a) {
                hardlevel = (level - 5) / 10 + 1;

                if (PlayerPrefs.GetInt("mhlid" + hardlevel, 0) == 1) {
                    return false;
                }


                PlayerPrefs.SetInt("mhlid" + hardlevel, 1);
                PlayerPrefs.Save();
                return true;
            }
            else {
                return false;
            }
        }
        else {
            if (level == 16) hardlevel = 1;
            else if (level == 20) hardlevel = 2;

            if (PlayerPrefs.GetInt("mhlid" + hardlevel, 0) == 1) {
                return false;
            }


            var a = level == 16 || level == 20;
            if (a) {
                PlayerPrefs.SetInt("mhlid" + hardlevel, 1);
                PlayerPrefs.Save();
            }

            return a;
        }
    }

    private void OnEnable() {
        play = true;
    }

    private void OnDisable() {
        play = false;
    }


    private void Update() {
        if (play) {
            cd -= Time.deltaTime;
            if (cd <= 0) {
                index++;
                if (index >= sprites.Length) {
                    index = 0;
                }

                ani.sprite = sprites[index];
                if (index == 0) {
                    cd = 2f;
                }
                else {
                    cd = 0.05f;
                }
            }
        }
    }

    public void OnTouchPlay() {
        if (GlobalManager.Instance.PlayerCoin >= 2) {
            GlobalManager.Instance.PlayerCoin -= 2;
            //mUIHardMode.m_UICoin.GetComponent<UICoin>().UpdateCoin();
            PlayerPrefs.SetInt("HardLevelStatus" + hardlevel, 1);
            GlobalManager.Instance.SaveGameData();

            GlobalManager.Instance.GameType = GameType.ChallengeHard;
            GlobalManager.Instance.CurrentHardLevel = hardlevel - 1;
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
            Root.SetActive(false);
        }
        else {
            TipsManager.Instance.ShowTips(GlobalManager.Instance.GetLanguageValue("CoinNotEnough"));
        }
        ///////////////////
    }

    public void OnTouchLater() {
        Root.SetActive(false);
    }
}
