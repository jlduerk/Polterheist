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
        for (int i = 0; i < spawnPoints.Count; i++) {
            int randomIndex = Random.Range(0, possessablePool.Length - 1);
            Instantiate(possessablePool[randomIndex], spawnPoints[i]);
        }
    }
}