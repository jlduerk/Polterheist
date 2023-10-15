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
    private bool isEnabled = true;
    
    public void Init() {
        button = GetComponent<Button>();
    }

    public void Highlight() {
        if (!IsEnabled()) {
            return;
        }
        transform.DOScale(highlightScaleUp, scaleUpDuration).SetEase(Ease.OutQuart).SetUpdate(true);
        AudioManager.Instance.Play(hoverSFX);
    }

    public void UnHighlight() {
        if (!IsEnabled()) {
            return;
        }
        transform.DOScale(Vector3.one, scaleUpDuration).SetEase(Ease.OutQuart).SetUpdate(true);
    }

    public void DoAction() {
        if (!IsEnabled()) {
            return;
        }
        if (button == null) {
            Debug.LogWarning($"No Button component on game object!");
            return;
        }

        AudioManager.Instance.Play(doActionSFX);
        button.onClick.Invoke();
    }

    public void EnableToggle(bool enable) {
        isEnabled = enable;
    }

    public bool IsEnabled() {
        return isEnabled;
    }
}