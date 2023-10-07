using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour {
    private static GameManager instance;
    public GameManager Instance {
        get { return instance; }
        private set { }
    }
    public TeamData[] teamDatas;

    private PlayerInputManager playerInputManager;
    private int playerCount;

    #region Monobehavior
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
        playerInputManager = FindObjectOfType<PlayerInputManager>();
    }

    private void OnEnable() {
        playerInputManager.onPlayerJoined += OnPlayerJoined;
    }

    private void OnDisable() {
        playerInputManager.onPlayerJoined -= OnPlayerJoined;
    }
    #endregion

    public TeamData GetTeamData(TeamData.Team teamToRequest) {
        foreach (TeamData teamData in teamDatas) {
            if (teamData.team == teamToRequest) {
                return teamData;
            }
        }
        Debug.LogError($"{teamToRequest} is not in GameManager TeamData array!");
        return null;
    }

    public void OnPlayerJoined(PlayerInput playerInput) {
        int counterModulo = playerCount % teamDatas.Length;
        playerInput.gameObject.GetComponent<PlayerPossession>().TeamDataInit(teamDatas[counterModulo]);
        playerCount++;
    }
}