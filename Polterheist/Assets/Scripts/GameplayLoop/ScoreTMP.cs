using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class ScoreTMP : MonoBehaviour {
    public TMP_Text text;
    public float pointDuration = 1;
    public float moveAmount = 1f;
    public float scaleAmount = 1.5f;
    public Ease ease;

    private void OnEnable() {
        text.material = Instantiate(text.material);
        transform.localScale = Vector3.one;
    }

    public void EarnPoint(int pointValue) {
        text.text = "+" + pointValue;
        Animate();
    }

    public void LosePoint(int pointValue) {
        text.text = "-" + pointValue;
        Animate();
    }

    private void Animate() {
        text.alpha = 1;
        transform.DOMoveY(moveAmount, pointDuration);
        transform.DOScale(scaleAmount, pointDuration);
        text.DOFade(0, pointDuration).OnComplete(DestroyMe);
    }

    private void DestroyMe() {
        // This script is placed on the child of the prefab
        Destroy(transform.parent.gameObject);
    }
}