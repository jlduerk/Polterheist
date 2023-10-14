using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 10;
    private bool timerIsRunning = false;
    public TextMeshProUGUI timerText;

    private void Start()
    {
        timerText.gameObject.SetActive(false);
        // Starts the timer automatically

        GameManager.Instance.GameStartEvent.AddListener(StartTimer);
    }

    void StartTimer()
    {
        timerText.gameObject.SetActive(true);
        timerIsRunning = true;
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                GameManager.Instance.GameEndEvent.Invoke();
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }

    void DisplayTime(float timeRemaining)
    {
        timeRemaining += 1;
        float minutes = Mathf.FloorToInt(timeRemaining / 60);
        float seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
