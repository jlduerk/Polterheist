using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour {
    private static GameManager instance;
    public static GameManager Instance {
        get { return instance; }
        private set { }
    }
    public TeamData[] teamDatas;

    private PlayerInputManager playerInputManager;
    private int playerCount;

    public ScoreManager scoreManager;

    public bool GameInProgress = false;

    public UnityEvent GameStartEvent;
    public UnityEvent GameEndEvent;

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

    private void Start()
    {
        if (GameStartEvent == null) GameStartEvent = new UnityEvent();
        if (GameEndEvent == null) GameEndEvent = new UnityEvent();

        GameStartEvent.AddListener(OnGameStarted);
        GameEndEvent.AddListener(OnGameEnded);

        GameStartEvent.Invoke();
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

    private void OnGameStarted()
    {
        Debug.Log("Game Started");
        GameInProgress = true;
    }
    private void OnGameEnded()
    {
        Debug.Log("Game Ended");
        GameInProgress = false;
    }
}