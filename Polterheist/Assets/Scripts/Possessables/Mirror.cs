using MEC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour {
    private const string PLAYER_TAG = "Player";
    private const string POSSESSABLE_TAG = "Possessable";
    public string teleportSFX = "Teleport";
    public float spitOutForce = 2;
    public Mirror matchingMirror;
    [HideInInspector] public bool mirrorLinked;
    public GameObject teleportVFXPrefab;
    private ParticleSystem teleportVFX;

    private void Start() {
        FindMirrorToLinkWith();
    }

    private void FindMirrorToLinkWith() {
        Mirror[] mirrorsInScene = FindObjectsOfType<Mirror>();
        if (mirrorsInScene == null || mirrorsInScene.Length == 0) {
            return;
        }
        foreach (Mirror mirror in mirrorsInScene) {
            if (mirror == this) {
                continue;
            }
            if (!mirror.mirrorLinked) {
                matchingMirror = mirror;
                mirror.matchingMirror = this;
                mirrorLinked = true;
                mirror.mirrorLinked = true;
                break;
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (!mirrorLinked) {
            return;
        }
        if (other.CompareTag(PLAYER_TAG)) {
            TeleportPlayer(other.transform);
        }
        //TODO add criteria to only pass thru if the mirror is haunted
        // if (other.CompareTag(POSSESSABLE_TAG) && this.IsHaunted?) {
        //     TeleportPossessable(other.transform);
        // }
    }

    //the get components here are not ideal but done due to time crunch and avoiding merge conflicts
    private void TeleportPlayer(Transform objectToTeleport) {
        //if player has a possessable, they cannot enter the mirror
        if (objectToTeleport.GetComponentInParent<PlayerPossession>().currPossessable) {
            return;
        }
        TeleportFX(objectToTeleport.GetComponentInParent<PlayerMovement>());
        objectToTeleport.parent.transform.position =
            matchingMirror.transform.position + matchingMirror.transform.forward * spitOutForce;
    }

    private void TeleportFX(PlayerMovement player) {
        AudioManager.Instance.Play(teleportSFX);
        if (!teleportVFX) {
            teleportVFX = Instantiate(teleportVFXPrefab, transform).GetComponent<ParticleSystem>();
        }
        teleportVFX.Play();
        player.SpawnEffect(player.transform);
    }
}