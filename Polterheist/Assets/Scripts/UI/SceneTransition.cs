using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour {
    public float duration = 3;
    public const float DISSOLVE_TRANSITION_DURATION = 0.2f;
    public const float DELAY = 0;
    private const float DISSOLVE_AMOUNT_CLOSED = 1.15f;
    private const float DISSOLVE_AMOUNT_OPEN = 0.2f;
    private const string SHADER_APATURE_REFERENCE = "_Apature";
    private const string SHADER_DISSOLVE_AMOUNT_REFERENCE = "_Dissolve_Amount";
    private static readonly int Apature = Shader.PropertyToID(SHADER_APATURE_REFERENCE);
    private static readonly int DissolveAmount = Shader.PropertyToID(SHADER_DISSOLVE_AMOUNT_REFERENCE);
    private Material material;
    private float startingApatureValue;

    private void Start() {
        material = GetComponent<Image>().material;
        if (material == null) {
            Debug.LogError("No assigned material! What're you doing?!");
            return;
        }

        startingApatureValue = material.GetFloat(Apature);
        material.SetFloat(DissolveAmount, DISSOLVE_AMOUNT_CLOSED);
        material.SetFloat(Apature, 1.5f);
        Invoke("SceneStartTransition", 0);
    }

    private void SceneStartTransition() {
        material.DOFloat(DISSOLVE_AMOUNT_OPEN, DissolveAmount, duration).SetEase(Ease.InCubic);
        material.DOFloat(-0.1f, Apature, duration).SetEase(Ease.InCubic);
    }

    public void GoToSceneTransition(string sceneToLoad = "") {
        material.DOFloat(DISSOLVE_AMOUNT_CLOSED, DissolveAmount, duration).SetEase(Ease.InOutCubic);
        material.DOFloat(1.4f, Apature, duration).SetEase(Ease.InOutCubic).OnComplete( ()=> LoadScene(sceneToLoad));
    }

    public void QuitGameTransition() {
        material.DOFloat(DISSOLVE_AMOUNT_CLOSED, DissolveAmount, duration).SetEase(Ease.InOutCubic);
        material.DOFloat(1.15f, Apature, duration).SetEase(Ease.InOutCubic).OnComplete(() => QuitGame());
    }

    private void LoadScene(string sceneToLoad) {
        if (sceneToLoad == "") {
            return;
        }
        Timing.RunCoroutine(_SceneChangeDelay(sceneToLoad));
    }

    private IEnumerator<float> _SceneChangeDelay(string sceneChange) {
        yield return Timing.WaitForSeconds(DELAY);
        DOTween.Clear();
        SceneManager.LoadScene(sceneChange);

    }

    private void QuitGame() {
        Timing.RunCoroutine(_QuitSceneChangeDelay());
    }

    private IEnumerator<float> _QuitSceneChangeDelay() {
        yield return Timing.WaitForSeconds(DELAY);
        Application.Quit();
    }


    //private void OnDisable() {
    //    material.SetFloat(DissolveAmount, DISSOLVE_AMOUNT_CLOSED);
    //    material.SetFloat(Apature, startingApatureValue);
    //}
}