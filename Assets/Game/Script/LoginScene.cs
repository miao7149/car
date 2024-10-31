using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginScene : MonoBehaviour {
    // Start is called before the first frame update


    void Start() {
        StartCoroutine(LogHelper.LogToServer("LoadGame", new Dictionary<string, object>()));
        int firstLogin = PlayerPrefs.GetInt("FirstLogin", 1);
        if (firstLogin == 1) {
            PlayerPrefs.SetInt("FirstLogin", 0);
            PlayerPrefs.Save();
            SceneManager.LoadScene(2, LoadSceneMode.Single);

            return;
        }


        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
