using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Selectable))]
public class MenuItem : MonoBehaviour {
    public Button button;
    public Vector3 highlightScaleUp = Vector3.one;
    public float scaleUpDuration = 1;
    public string hoverSFX;
    public string doActionSFX;

    public void Init() {
        button = GetComponent<Button>();
    }

    public void Highlight() {
        transform.DOScale(highlightScaleUp, scaleUpDuration).SetEase(Ease.OutQuart);
        AudioManager.Instance.Play(hoverSFX);
    }

    public void UnHighlight() {
        transform.DOScale(Vector3.one, scaleUpDuration).SetEase(Ease.OutQuart);
    }

    public void DoAction() {
        if (button == null) {
            Debug.LogWarning($"No Button component on game object!");
            return;
        }

        AudioManager.Instance.Play(doActionSFX);
        button.onClick.Invoke();
    }
}