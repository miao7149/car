using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class People : MonoBehaviour {
    public GameObject main;
    public Sequence action = null;
    private bool isTrigger = false;
    private float TriggerTime = 1;

    public Animator m_Animator;

    //晕倒后的角色
    public GameObject m_Faint;
    public PeoInfoItem peoInfo;

    void OnDestroy() {
        if (action != null) {
            action.Kill();
        }
    }

    float Speed = 1f;

    public void Init(PeoInfoItem peoInfoItem) {
        peoInfo = peoInfoItem;
        gameObject.SetActive(true);
        transform.SetParent(transform, false);
        main.transform.localEulerAngles = new Vector3(0, 0, 0);
        m_Animator.gameObject.SetActive(true);
        m_Faint.SetActive(false);
        var p1 = GameManager.Instance.ConvertPos(new Vector3(peoInfoItem.pos1.x, 0, peoInfoItem.pos1.y));
        var p2 = GameManager.Instance.ConvertPos(new Vector3(peoInfoItem.pos2.x, 0, peoInfoItem.pos2.y));
        Vector3 direction = (p2 - p1).normalized;
        var dis = Vector3.Distance(p1, p2);
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.localRotation = lookRotation;
        transform.localPosition = p1;

        // 创建一个DOTween序列
        action = DOTween.Sequence();

        // 移动到p2
        action.AppendCallback(() => m_Animator.CrossFade("walk", 0.1f)); // 开始行走动画
        action.Append(transform.DOLocalMove(p2, dis / Speed).SetEase(Ease.Linear).OnComplete(() => TurnAround(gameObject, p2, p1, m_Animator)));
        action.AppendCallback(() => m_Animator.CrossFade("normal", 0.1f)); // 停止行走动画
        action.AppendInterval(1); // 停顿1秒

        // 移动到p1
        action.AppendCallback(() => m_Animator.CrossFade("walk", 0.1f)); // 开始行走动画
        action.Append(transform.DOLocalMove(p1, dis / Speed).SetEase(Ease.Linear).OnComplete(() => TurnAround(gameObject, p1, p2, m_Animator)));
        action.AppendCallback(() => m_Animator.CrossFade("normal", 0.1f)); // 停止行走动画
        action.AppendInterval(1); // 停顿1秒

        // 无限循环
        action.SetLoops(-1, LoopType.Restart);
        action.Play();
    }

    public void Replay() {
        if (action != null) {
            action.Kill();
        }

        var p1 = GameManager.Instance.ConvertPos(new Vector3(peoInfo.pos1.x, 0, peoInfo.pos1.y));
        var p2 = GameManager.Instance.ConvertPos(new Vector3(peoInfo.pos2.x, 0, peoInfo.pos2.y));
        var dis = Vector3.Distance(p1, p2);

        // 创建一个DOTween序列
        action = DOTween.Sequence();

        // 移动到p2
        action.AppendCallback(() => m_Animator.CrossFade("walk", 0.1f)); // 开始行走动画
        action.Append(transform.DOLocalMove(p2, dis / Speed).SetEase(Ease.Linear).OnComplete(() => TurnAround(gameObject, p2, p1, m_Animator)));
        action.AppendCallback(() => m_Animator.CrossFade("normal", 0.1f)); // 停止行走动画
        action.AppendInterval(1); // 停顿1秒

        // 移动到p1
        action.AppendCallback(() => m_Animator.CrossFade("walk", 0.1f)); // 开始行走动画
        action.Append(transform.DOLocalMove(p1, dis / Speed).SetEase(Ease.Linear).OnComplete(() => TurnAround(gameObject, p1, p2, m_Animator)));
        action.AppendCallback(() => m_Animator.CrossFade("normal", 0.1f)); // 停止行走动画
        action.AppendInterval(1); // 停顿1秒

        // 无限循环
        action.SetLoops(-1, LoopType.Restart);
        action.Play();
    }

    public void Hit(Vector3 dir) {
        m_Animator.gameObject.SetActive(false);
        m_Faint.SetActive(true);
        action.Kill();
        main.transform.localEulerAngles = new Vector3(0, 0, 90);
        transform.DOLocalRotateQuaternion(Quaternion.LookRotation(dir), 1f).SetEase(Ease.OutCirc).Play();
        transform.DOLocalMove(transform.localPosition + dir.normalized * 1.5f, 1f).SetEase(Ease.OutCirc).Play();
    }

    void Update() {
        if (isTrigger) {
            TriggerTime -= Time.deltaTime;
            if (TriggerTime <= 0) {
                isTrigger = false;
                TriggerTime = 1;
            }
        }
    }

    void TurnAround(GameObject p, Vector3 from, Vector3 to, Animator animator) {
        // 计算转身方向
        Vector3 direction = (to - from).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        p.transform.DORotateQuaternion(lookRotation, 0.5f); // 转身动画
    }

    void OnTriggerEnter(Collider other) {
        var car = other.transform.parent.parent.GetComponent<Car>();
        if (car.backing) return;
        if (isTrigger) return;
        isTrigger = true;

        GameManager.Instance.failReason = FailReason.PeopleCrash;

        GameManager.Instance.SetGameStatu(GameStatu.faled);
        GameManager.Instance.StepCount += 1;


        GameManager.Instance.hitPeopleCar = car;
        GameManager.Instance.hitPeople = this;
        car.moveAction.Kill();
        if (car.mTrail != null)
            car.mTrail.SetActive(false);
        AudioManager.Instance.PlayPedestrianHit();
        // DOVirtual.DelayedCall(2f, () => {
        //     GameManager.Instance.DeleteCar(car);
        // });
        Hit(car.transform.localRotation * Vector3.forward);
    }
}
