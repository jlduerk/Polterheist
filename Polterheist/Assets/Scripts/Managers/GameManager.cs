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
    public bool allowSinglePlayer;
    public TeamData[] teamDatas;

    private PlayerInputManager playerInputManager;
    public int playerCount;
    public Transform[] playerSpawnPoints;

    public ScoreManager scoreManager;
    public GameFlowManager gameFlowManager;

    public bool GameInProgress = false;
    public bool Paused = false;

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

        playerInputManager = FindObjectOfType<PlayerInputManager>();
    }

    private void Start()
    {
        if (GameStartEvent == null) GameStartEvent = new UnityEvent();
        if (GameEndEvent == null) GameEndEvent = new UnityEvent();

        GameStartEvent.AddListener(OnGameStarted);
        GameEndEvent.AddListener(OnGameEnded);
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

    public void OnLevelOpened()
    {
        scoreManager.Init();
        gameFlowManager.Init();
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

    public void StartGame() {
        GameStartEvent.Invoke();
    }

    List<PlayerInput> players = new List<PlayerInput>();
    public void RegisterPlayer(PlayerInput playerInput) {
        int counterModulo = playerCount % teamDatas.Length;
        playerInput.gameObject.GetComponent<PlayerPossession>().TeamDataInit(teamDatas[counterModulo]);
        playerCount++;

        players.Add(playerInput);
        playerInput.gameObject.transform.position = playerSpawnPoints[players.Count - 1].position;

        if (allowSinglePlayer) {
            gameFlowManager.StartCountdown();
            return;
        }
        if (playerCount >= 2) {
            gameFlowManager.StartCountdown();
        }
    }
}