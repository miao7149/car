using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TrafficLight : MonoBehaviour {
    // Start is called before the first frame update
    //信号灯状态
    public int state = 0; //0红灯 1绿灯

    //红灯时间
    private float redTime; //红灯三秒

    //绿灯时间
    private float greenTime; //绿灯五秒

    public Light m_Light;

    //倒计时文字
    public TextMesh m_Text;
    private bool isTrigger = false;
    private float TriggerTime = 1;
    public GameObject m_Lv;
    public GameObject m_Hong;

    void Awake() {
        redTime = 3;
        greenTime = 5;
    }

    void Start() {
        //默认红灯
        m_Light.color = Color.green;
        m_Hong.SetActive(false);
        m_Lv.SetActive(true);
        m_Text.text = greenTime.ToString();
    }

    // Update is called once per frame
    void Update() {
        //红灯倒计时
        if (state == 0) {
            redTime -= Time.deltaTime;
            //文字倒计时去除小数
            m_Text.text = ((int)redTime + 1).ToString();
            if (redTime <= 0) {
                state = 1;
                redTime = 0;
                greenTime = 5;
                m_Light.color = Color.green;
                m_Lv.SetActive(true);
                m_Hong.SetActive(false);
            }
        }
        //绿灯倒计时
        else {
            greenTime -= Time.deltaTime;
            //文字倒计时去除小数
            m_Text.text = ((int)greenTime + 1).ToString();
            if (greenTime <= 0) {
                state = 0;
                greenTime = 0;
                redTime = 3;
                m_Light.color = Color.red;
                m_Lv.SetActive(false);
                m_Hong.SetActive(true);
            }
        }

        if (isTrigger) {
            TriggerTime -= Time.deltaTime;
            if (TriggerTime <= 0) {
                isTrigger = false;
                TriggerTime = 1;
            }
        }
    }

    public void Init(TrafficLightInfo lightInfo) {
        redTime = lightInfo.redTime;
        greenTime = lightInfo.greenTime;
        switch (lightInfo.dir) {
            case 0:
                transform.localEulerAngles = new Vector3(0, 0, 0);
                m_Text.transform.localEulerAngles = new Vector3(90, 0, 0);
                break;
            case 1:
                transform.localEulerAngles = new Vector3(0, 180, 0);
                m_Text.transform.localEulerAngles = new Vector3(90, 0, 180);
                break;
            case 2:
                transform.localEulerAngles = new Vector3(0, -90, 0);
                m_Text.transform.localEulerAngles = new Vector3(90, 0, -90);
                break;
            case 3:
                transform.localEulerAngles = new Vector3(0, 90, 0);
                m_Text.transform.localEulerAngles = new Vector3(90, 0, 90);
                break;
        }
    }

    //如果是红灯，开启碰撞检测
    void OnTriggerEnter(Collider other) {
        if (state == 0 && !isTrigger) {
            Debug.Log("信号灯碰撞检测");
            isTrigger = true;
            var car = other.transform.parent.parent.GetComponent<Car>();
            if (car.backing == false) {
                GameManager.Instance.StepCount -= 2; //减少行动力
                GameManager.Instance.CheckGameResult();
            }
        }
    }
}
