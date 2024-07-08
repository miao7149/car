using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class People : MonoBehaviour {
    public GameObject main;
    public Sequence action = null;


    public void Init() {
        main.transform.localEulerAngles = new Vector3(0, 0, 0);
    }

    public void Hit(Vector3 dir) {
        action.Kill();
        main.transform.localEulerAngles = new Vector3(0, 0, 90);
        transform.DOLocalRotateQuaternion(Quaternion.LookRotation(dir), 1f).SetEase(Ease.OutCirc).Play();
        transform.DOLocalMove(transform.localPosition + dir.normalized * 3, 1f).SetEase(Ease.OutCirc).Play();
    }

    public void TriggerEnter(Collider other) {
        var car = other.transform.parent.parent.GetComponent<Car>();
        if (car.dead) {
            car.moveAction.Kill();
            Hit(car.transform.localRotation * Vector3.forward);
            GameManager.Instance.SetGameStatu(GameStatu.faled);
        }
    }
}
