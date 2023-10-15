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

    // Start is called before the first frame update
    void Start()
    {
        CountdownText = GetComponent<TextMeshProUGUI>();
        //AnimatedCountdown();
    }

    public void AnimatedCountdown()
    {
        if (countdownNum > 0)
        {
            CountdownText.text = countdownNum.ToString();
            GameManager.Instance.audioManager.Play("Countdown");
            NumberAnimation();
        }
        else if (countdownNum == 0)
        {
            CountdownText.text = "O";
            GameManager.Instance.audioManager.Play("CountdownGo");
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
