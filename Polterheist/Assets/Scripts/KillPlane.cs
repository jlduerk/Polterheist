using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KillPlane : MonoBehaviour {
    private PossessableSpawner possessableSpawner;
    private const string PLAYER_TAG = "Player";
    private const string POSSESSABLE_TAG = "Possessable";

    private void Start() {
        possessableSpawner = FindObjectOfType<PossessableSpawner>();
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag(POSSESSABLE_TAG)) {
            if (!possessableSpawner) {
                return;
            }
            possessableSpawner.RespawnPossessable(other.transform);
        }

        if (other.CompareTag(PLAYER_TAG)) {
            RespawnPlayer(other.GetComponent<PlayerInput>());
        }
    }
    
    public void RespawnPlayer(PlayerInput player) {
        for (int i = 0; i < PersistentPlayersManager.Instance.GetActivePlayerCount(); i++) {
            if (player == PersistentPlayersManager.Instance.currentLevelData.playerInputs[i]) {
                player.transform.position = GameManager.Instance.playerSpawnPoints[i].position;
                return;
            }
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.green;
        //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}