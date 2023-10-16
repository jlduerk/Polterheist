using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class PossessableSpawner : MonoBehaviour {
    public PossessableSet possessableSet;
    private Possessable[] possessablePool;
    private List<Transform> spawnPoints = new List<Transform>();
    private const string SPAWN_POINT_TAG = "SpawnPoint";
    public Vector3 respawnOffset = new Vector3(0, 1, 0);
    public GameObject spawnEffectPrefab;
    private ParticleSystem spawnEffect;

    private void Start() {
        possessableSet = Instantiate(possessableSet);
        possessablePool = new Possessable[possessableSet.possessables.Length];
        Array.Copy(possessableSet.possessables, possessablePool, possessableSet.possessables.Length);
        
        foreach (Transform child in transform) {
            if (child.tag != SPAWN_POINT_TAG) {
                continue;
            }
            spawnPoints.Add(child);
        }
        
        SpawnPossessables();
    }

    public void SpawnPossessables() {
        GameObject possessablesContainer = new GameObject();
        possessablesContainer.transform.SetParent(transform);
        possessablesContainer.name = "PossessablesContainer";
        
        for (int i = 0; i < spawnPoints.Count; i++) {
            int randomIndex = Random.Range(0, possessablePool.Length - 1);
            Instantiate(possessablePool[randomIndex], spawnPoints[i].position, Quaternion.identity, possessablesContainer.transform);
        }
    }

    public void RespawnPossessable(Transform possessableToRespawn) {
        int randomIndex = Random.Range(0, spawnPoints.Count - 1);
        possessableToRespawn.GetComponent<Rigidbody>().velocity = Vector3.zero;
        possessableToRespawn.position = spawnPoints[randomIndex].position + respawnOffset;
        SpawnEffect(possessableToRespawn);
    }
    
    public void SpawnEffect(Transform spawnTransform) {
        if (!spawnEffectPrefab) {
            Debug.LogError($"No spawnEffectPrefab assigned!");
            return;
        }
        if (spawnEffect) {
            spawnEffect.transform.position = spawnTransform.position;
            spawnEffect.Play();
            return;
        }

        spawnEffect = Instantiate(spawnEffectPrefab, spawnTransform.position, Quaternion.identity, spawnTransform).GetComponent<ParticleSystem>();
        spawnEffect.Play();
    }
}