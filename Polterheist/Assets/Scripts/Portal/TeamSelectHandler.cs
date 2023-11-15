using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TeamSelectHandler : MonoBehaviour {
    public Transform playerTeleportLocation;
    public Transform[] playerSpawns;
    public GameObject waitingForPlayersToJoinPrompt;
    public GameObject pressToStartPrompt;
    private int totalPlayerCount;
    private int currentPlayerCount;
    public TeamSelectPortal[] portals;

    private void Update() {
        StartGamePrompt();
    }

    private void StartGamePrompt() {
        totalPlayerCount = PersistentPlayersManager.Instance.GetActivePlayerCount();
        if (CheckIfAllPlayersHaveTeam()) {
            pressToStartPrompt.SetActive(true);
            waitingForPlayersToJoinPrompt.SetActive(false);
        }
        else {
            pressToStartPrompt.SetActive(false);
            waitingForPlayersToJoinPrompt.SetActive(true);
        }
    }
    
    private bool CheckIfAllPlayersHaveTeam() {
        foreach (TeamSelectPortal portal in portals) {
            if (!portal.HasPlayer()) {
                return false;
            }
        }

        return totalPlayerCount >= currentPlayerCount;
    }
}