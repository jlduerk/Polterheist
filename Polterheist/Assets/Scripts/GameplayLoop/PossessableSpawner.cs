using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossessableSpawner : MonoBehaviour {
    public Possessable[] possessablePool;
    public List<Transform> spawnPoints;
    private const string SPAWN_POINT_TAG = "SpawnPoint";

    private void Start() {
        spawnPoints = new List<Transform>();
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