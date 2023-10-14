using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : MonoBehaviour {
    public int numRounds = 3;
    public float roundStartDelay = 2;
    private int roundsCounter = 0;
    private CoroutineHandle beginRoundCoroutine;

    public void BeginRound() {
        if (!beginRoundCoroutine.IsRunning) {
            beginRoundCoroutine = Timing.RunCoroutine(_BeginRound());
        }
    }

    private IEnumerator<float> _BeginRound() {
        //TODO send a message for screen transitions OR wait time prior to game start
        
        yield return Timing.WaitForSeconds(roundStartDelay);
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
