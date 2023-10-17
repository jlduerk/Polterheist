using MEC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Mirror : MonoBehaviour {
    private const string PLAYER_TAG = "Player";
    private const string POSSESSABLE_TAG = "Possessable";
    public string teleportSFX = "Teleport";
    public float spitOutForce = 2;
    public Mirror matchingMirror;
    [HideInInspector] public bool mirrorLinked;
    public GameObject teleportVFXPrefab;
    private ParticleSystem teleportVFX;

    private CoroutineHandle hauntCoroutine;
    public float hauntDuration = 5;
    private bool isHaunted;
    public Renderer renderer;
    private Material startingMaterial;
    public Material hauntedMaterial;
    public GameObject hauntVFXPrefab;
    private ParticleSystem hauntVFX;
    
    public Rigidbody possessableRidgidBody;
    public float pushBackIntensity = 100;
    public float HauntLaunchForce = 300;
    public Vector3 HauntEffectExtents = new Vector3(4,4,4);
    private CapsuleCollider collider;
    private float starterColliderRadius;
    private float starterOffset;
    public float hauntedColliderRadius;
    public float hauntedColliderOffset;

    private void Start() {
        FindMirrorToLinkWith();
        startingMaterial = renderer.materials[0];
        collider = GetComponent<CapsuleCollider>();
        starterColliderRadius = collider.radius;
        starterOffset = collider.center.z;
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

        if (other.CompareTag(POSSESSABLE_TAG) && isHaunted) {
            TeleportPossessable(other.transform);
        }
    }

    //the get components here are not ideal but done due to time crunch and avoiding merge conflicts
    private void TeleportPlayer(Transform objectToTeleport) {
        //if player has a possessable, they cannot enter the mirror
        if (objectToTeleport.GetComponentInParent<PlayerPossession>().currPossessable) {
            return;
        }
        TeleportPlayerFX(objectToTeleport.GetComponentInParent<PlayerMovement>());
        objectToTeleport.parent.transform.position =
            matchingMirror.transform.position + matchingMirror.transform.forward * spitOutForce;
    }

    private void TeleportPossessable(Transform possessableTransform) {
        possessableTransform.parent.transform.position =
            matchingMirror.transform.position + matchingMirror.transform.forward * spitOutForce;
        possessableTransform.GetComponent<Rigidbody>().AddForce(matchingMirror.transform.forward * spitOutForce, ForceMode.VelocityChange);
    }

    private void TeleportPlayerFX(PlayerMovement player) {
        AudioManager.Instance.Play(teleportSFX);
        if (!teleportVFX) {
            teleportVFX = Instantiate(teleportVFXPrefab, transform).GetComponent<ParticleSystem>();
        }
        teleportVFX.Play();
        player.SpawnEffect(player.transform);
    }

    public void Haunt(Possessable possessable, PlayerPossession possessor) {
        if (isHaunted || hauntCoroutine.IsRunning) {
            return;
        }

        //perhaps just spawn a pull sphere?
        Vector3 upForce = new Vector3(1, HauntLaunchForce, 1);
        possessableRidgidBody.AddForce(upForce);
        Collider[] hitColliders = Physics.OverlapBox(transform.position + (transform.forward * spitOutForce), HauntEffectExtents);
        foreach (var hitCollider in hitColliders) {
            if (LayerMask.LayerToName(hitCollider.gameObject.layer) == "Possessable") {
                Vector3 towardsVec = -1 * (hitCollider.transform.position - possessable.transform.position).normalized;
                Vector3 forceVec = new Vector3(towardsVec.x, 1, towardsVec.z);
                hitCollider.attachedRigidbody.AddForce(forceVec * pushBackIntensity * possessableRidgidBody.mass);
            }
        }
        
        hauntCoroutine = Timing.RunCoroutine(_Haunt());
    }
    private IEnumerator<float> _Haunt() {
        isHaunted = true;
        HauntEffect();
        renderer.material = hauntedMaterial;
        yield return Timing.WaitForSeconds(hauntDuration);
        collider.radius = starterColliderRadius;
        Vector3 starterCenter = new Vector3(0, 0, starterOffset);
        collider.center = starterCenter;
        renderer.material = startingMaterial;
        isHaunted = false;
    }

    public void HauntEffect() {
        if (hauntVFX) {
            hauntVFX.Play();
            return;
        }
        collider.radius = hauntedColliderRadius;
        Vector3 returnedCenter = new Vector3(0, 0, hauntedColliderOffset);
        collider.center = returnedCenter;
        hauntVFX = Instantiate(hauntVFXPrefab, transform.position, Quaternion.identity, transform).GetComponent<ParticleSystem>();
        hauntVFX.Play();
    }
}