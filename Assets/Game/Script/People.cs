using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class People : MonoBehaviour
{
    public GameObject main;
    public Sequence action = null;
    private bool isTrigger = false;
    private float TriggerTime = 1;

    public void Init()
    {
        main.transform.localEulerAngles = new Vector3(0, 0, 0);
    }

    public void Hit(Vector3 dir)
    {
        action.Kill();
        main.transform.localEulerAngles = new Vector3(0, 0, 90);
        transform.DOLocalRotateQuaternion(Quaternion.LookRotation(dir), 1f).SetEase(Ease.OutCirc).Play();
        transform.DOLocalMove(transform.localPosition + dir.normalized * 3, 1f).SetEase(Ease.OutCirc).Play();
    }
    void Update()
    {
        if (isTrigger)
        {
            TriggerTime -= Time.deltaTime;
            if (TriggerTime <= 0)
            {
                isTrigger = false;
                TriggerTime = 1;
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (isTrigger) return;
        isTrigger = true;
        var car = other.transform.parent.parent.GetComponent<Car>();
        GameManager.Instance.failReason = FailReason.PeopleCrash;
        GameManager.Instance.SetGameStatu(GameStatu.faled);
        GameManager.Instance.hitPeopleCar = car;
        car.moveAction.Kill();
        //Hit(car.transform.localRotation * Vector3.forward);
    }
}
