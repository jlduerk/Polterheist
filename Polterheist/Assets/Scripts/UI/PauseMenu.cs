using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class PauseMenu : MenuContainer {
    [HideInInspector] public bool paused;
    public float pauseDuration;
    public Ease pauseEase;
    public float unPauseDuration;
    public Ease unPauseEase;

    public override void StartAction(InputAction.CallbackContext context) {
        PauseToggle();
        base.StartAction(context);
    }

    public void PauseToggle() {
        paused = !paused;
        Settings.PauseGame(paused);

        if (paused) {
            eventSystem.SetSelectedGameObject(menuItems[0].gameObject);
            DOTween.To(()=> Time.timeScale, x=> Time.timeScale = x, 0, pauseDuration).SetEase(pauseEase);
        }
        else {
            DOTween.To(()=> Time.timeScale, x=> Time.timeScale = x, 1, unPauseDuration).SetEase(unPauseEase);
        }
        FadeMenu(paused);
    }
}