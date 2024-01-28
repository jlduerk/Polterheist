using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TeamSelectPortal : MonoBehaviour {
    public TeamData.Team teamToAssign;
    public GameObject enterPortalFXPrefab;
    private ParticleSystem enterPortalFX;
    public GameObject exitPortalFXPrefab;
    private ParticleSystem exitPortalFX;
    private MeshRenderer meshRenderer;
    private int playerCount;

    private void Start() {
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer == null) {
            Debug.LogError("No MeshRenderer found on Portal object!", gameObject);
        }

        meshRenderer.material = PersistentPlayersManager.Instance.GetTeamData(teamToAssign).portalMaterial;
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
        player.gameObject.GetComponent<PlayerPossession>().TeamDataInit(teamToAssign);
        player.GetPlayerMovement().TogglePlayerMovement(true);
        playerCount++;
    }

    public void ExitPortal(PlayerPossession player) {
        PlayExitPortalFX();
        player.gameObject.GetComponent<PlayerPossession>().TeamDataInit(PersistentPlayersManager.DEFAULT_TEAM);
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