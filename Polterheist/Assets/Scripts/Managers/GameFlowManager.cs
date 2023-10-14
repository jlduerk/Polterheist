using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : MonoBehaviour {
    private List<Possessable> spawnedPossessables = new List<Possessable>();

    [Header("Rounds Attributes")]
    public int numRounds = 3;
    private int roundsCounter = 0;

    public void BeginRound() {
        //DOTWEEN CALL THIS!
        GameManager.Instance.StartGame();
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
