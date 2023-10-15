using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : MonoBehaviour {
    private List<Possessable> spawnedPossessables = new List<Possessable>();

    [Header("Rounds Attributes")]
    public int numRounds = 3;
    private int roundsCounter = 0;

    private Countdown countdown;

    public void BeginRound() {
        //DOTWEEN CALL THIS!
        GameManager.Instance.StartGame();
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        countdown = FindObjectOfType<Countdown>();
    }

    public void StartCountdown()
    {
        if (countdown == null)
        {
            Debug.Log("GameFlowManager: countdown is null");
        } else
        {
            countdown.AnimatedCountdown();
        }
    }

    private void NextRound() {
        roundsCounter++;
        if (roundsCounter >= numRounds) {
            GameOver();
            return;
        }

        BeginRound();
    }

    private void GameOver() {
        //TODO
    }
}
