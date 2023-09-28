using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossessionTest : MonoBehaviour
{
    public PlayerPossession playerA;
    public PlayerPossession playerB;
    public Possessable possessable;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            playerB.Unpossess();
            playerA.TryPossess(possessable);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            playerA.Unpossess();
            playerB.TryPossess(possessable);
        }
    }
}
