using System;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

namespace MM {
    public class DialogBase : MonoBehaviour {
        public RectTransform baseDialog;

        private void OnEnable() {
            baseDialog.localScale = Vector3.zero;
            baseDialog.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack).Play();
        }
    }
}
