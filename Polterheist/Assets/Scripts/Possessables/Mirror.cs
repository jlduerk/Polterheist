using MEC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour {
    private const string PLAYER_TAG = "Player";
    private const string POSSESSABLE_TAG = "Possessable";
    public string teleportSFX = "Teleport";
    public float spitOutForce = 3;
    public Mirror matchingMirror;
    [HideInInspector] public bool mirrorLinked;
    
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

    private void TeleportPlayer(Transform objectToTeleport) {
        objectToTeleport.parent.transform.position =
            matchingMirror.transform.position + matchingMirror.transform.forward * spitOutForce; //forward;
        //objectToTeleport.parent.GetComponent<Rigidbody>().AddForce(matchingMirror.transform.forward * spitOutForce, ForceMode.Impulse);
    }
}