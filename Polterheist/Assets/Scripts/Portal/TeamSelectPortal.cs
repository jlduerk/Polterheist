using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TeamSelectPortal : MonoBehaviour {
    public TeamData neutralTeam;
    public TeamData team;
    public GameObject enterPortalFXPrefab;
    private ParticleSystem enterPortalFX;
    public GameObject exitPortalFXPrefab;
    private ParticleSystem exitPortalFX;
    private MeshRenderer renderer;
    private int playerCount;

    private void Start() {
        renderer = GetComponent<MeshRenderer>();
        if (renderer == null) {
            Debug.LogError("No MeshRenderer found on Portal object!", gameObject);
        }
        if (team == null) {
            Debug.LogError("No TeamData assigned for Portal object!", gameObject);
        }

        renderer.material = team.portalMaterial;
    }

    private void OnTriggerEnter(Collider other) {
        PlayerPossession player = other.GetComponent<PlayerPossession>();
        if (player == null) {
            return;
        }
        EnterPortal(player);
    }

    private void EnterPortal(PlayerPossession player) {
        PlayEnterPortalFX();
        player.gameObject.GetComponent<PlayerPossession>().TeamDataInit(team);
        player.GetPlayerMovement().TogglePlayerMovement(true);
        playerCount++;
    }

    public void ExitPortal(PlayerPossession player) {
        PlayExitPortalFX();
        player.gameObject.GetComponent<PlayerPossession>().TeamDataInit(neutralTeam);
        player.GetPlayerMovement().TogglePlayerMovement(false);
        playerCount--;
    }

    private void PlayEnterPortalFX() {
        if (enterPortalFXPrefab == null) {
            return;
        }
        if (enterPortalFX == null) {
            enterPortalFX = Instantiate(enterPortalFXPrefab, transform.position, enterPortalFXPrefab.transform.rotation, transform).GetComponent<ParticleSystem>();
        }

        enterPortalFX.Play();
    }

    private void PlayExitPortalFX() {
        if (exitPortalFXPrefab == null) {
            return;
        }
        if (exitPortalFX == null) {
            exitPortalFX = Instantiate(exitPortalFXPrefab, transform.position, exitPortalFXPrefab.transform.rotation, transform).GetComponent<ParticleSystem>();
        }

        exitPortalFX.Play();
    }

    public bool HasPlayer() {
        return playerCount > 0;
    }
}