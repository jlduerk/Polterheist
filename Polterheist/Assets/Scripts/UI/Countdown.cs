using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class Countdown : MonoBehaviour
{
    private TextMeshProUGUI CountdownText;
    [SerializeField] private float scaleBig = 50;
    [SerializeField] private float scaleGoFade = 10;
    [SerializeField] private float tweenTimePerNumber = 0.5f;
    [SerializeField] private float TimePerNumberDisplay = 0.5f;
    [SerializeField] private float GhostScale = 2f;

    private int countdownNum = 3;
    private bool countdownStarted;

    // Start is called before the first frame update
    void Start()
    {
        CountdownText = GetComponent<TextMeshProUGUI>();
        //AnimatedCountdown();
    }

    private void Update() {
        if (countdownStarted) {
            return;
        }

        int soulCount = 2 - GameManager.Instance.playerCount;
        string soulString = soulCount > 1 ? "s0uls" : "s0ul";
        CountdownText.text = $"Waiting f0r {soulCount.ToString()} {soulString}...";
    }

    public void AnimatedCountdown()
    {
        countdownStarted = true;
        if (countdownNum > 0)
        {
            CountdownText.text = countdownNum.ToString();
            CountdownText.alpha = 0;
            AudioManager.Instance.Play("Countdown");
            NumberAnimation();
        }
        else if (countdownNum == 0)
        {
            CountdownText.text = "O";
            AudioManager.Instance.Play("CountdownGo");
            GoAnimation();
        }
        countdownNum--;
    }

    private void NumberAnimation()
    {
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(transform.DOScale(new Vector3(scaleBig, scaleBig, scaleBig), 0f))
            .Append(transform.DOScale(new Vector3(1, 1, 1), tweenTimePerNumber).SetEase(Ease.OutFlash))
            .Insert(0, CountdownText.DOFade(0f, 0f))
            .Insert(0.1f, CountdownText.DOFade(1f, tweenTimePerNumber).SetEase(Ease.OutFlash))
            .AppendInterval(TimePerNumberDisplay)
            .AppendCallback(AnimatedCountdown);
    }

    private void GoAnimation()
    {
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(transform.DOScale(new Vector3(scaleBig, scaleBig, scaleBig), 0f))
            .Append(transform.DOScale(new Vector3(GhostScale, GhostScale, GhostScale), tweenTimePerNumber).SetEase(Ease.OutFlash))
            .Insert(0, CountdownText.DOFade(0f, 0f))
            .Insert(0.1f, CountdownText.DOFade(1f, tweenTimePerNumber).SetEase(Ease.OutFlash))
            .AppendInterval(TimePerNumberDisplay * 1.5f)
            .Append(transform.DOScale(new Vector3(scaleGoFade, scaleGoFade, scaleGoFade), 0.5f))
            .Insert(TimePerNumberDisplay * 1.5f + tweenTimePerNumber, CountdownText.DOFade(0f, 0.5f).SetEase(Ease.InFlash))
            .AppendCallback(StartGame);
    }

    private void StartGame()
    {
        GameManager.Instance.gameFlowManager.BeginRound();
    }

}
