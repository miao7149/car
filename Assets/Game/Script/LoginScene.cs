using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginScene : MonoBehaviour {
    // Start is called before the first frame update


    void Start() {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
