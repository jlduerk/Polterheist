using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    private static GameManager instance;
    public static TeamData[] teamDatas;

    private void Awake() {
        // check if the instance already exists and destroy the new instance if it does
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        }
        // set the instance to this object if it doesn't exist
        instance = this;
        // prevent the instance from being destroyed when loading new scenes
        DontDestroyOnLoad(this.gameObject);
    }

    public static TeamData GetTeamData(TeamData.Team teamToRequest) {
        foreach (TeamData teamData in teamDatas) {
            if (teamData.team == teamToRequest) {
                return teamData;
            }
        }
        Debug.LogError($"{teamToRequest} is not in GameManager TeamData array!");
        return null;
    }
}