using System;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;
using MEC;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {
    private static GameManager instance;
    public static GameManager Instance {
        get { return instance; }
        private set { }
    }
    private const int MIN_PLAYERS = 2;
    private const int DEBUG_MIN_PLAYERS = 1;


    public TeamData[] teamDatas;
    private bool singlePlayerAllowed;

    private PlayerInputManager playerInputManager;
    public Transform[] playerSpawnPoints;

    public ScoreManager scoreManager;
    public GameFlowManager gameFlowManager;

    public bool GameInProgress = false;

    public UnityEvent GameStartEvent;
    public UnityEvent GameEndEvent;

    public GameObject[] hats;
    private CoroutineHandle howToPlayCoroutine;
    public float howToPlayDuration = 5;
    private bool howToPlayDone;
    private bool countdownStarted;

    //this part is hard-coded and more one off-y. if we work on this again this will def need changes lol
    private const int NUM_LEVELS = 3;
    private int currentLevelIndex;
    private string currentLevelName;

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

#if UNITY_EDITOR
        singlePlayerAllowed = true;
#endif
    }

    private void Start()
    {
        if (GameStartEvent == null) GameStartEvent = new UnityEvent();
        if (GameEndEvent == null) GameEndEvent = new UnityEvent();

        scoreManager.Init();
        gameFlowManager.Init();

        GameStartEvent.AddListener(OnGameStarted);
        GameEndEvent.AddListener(OnGameEnded);

        //howToPlayCoroutine = Timing.RunCoroutine(_HowToPlay());
    }

    private void Update() {
        if (!howToPlayDone || countdownStarted) {
            return;
        }
        if (singlePlayerAllowed && PersistentPlayersManager.Instance.currentLevelData.numPlayersReady >= DEBUG_MIN_PLAYERS) {
            gameFlowManager.StartCountdown();
            countdownStarted = true;
            return;
        }
        if (PersistentPlayersManager.Instance.currentLevelData.numPlayersReady >= MIN_PLAYERS) {
            gameFlowManager.StartCountdown();
            countdownStarted = true;
            return;
        }
    }

    private void OnDestroy() {
        Timing.KillCoroutines(howToPlayCoroutine);
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

    private void OnGameStarted()
    {
        Debug.Log("Game Started");
        GameInProgress = true;
    }
    private void OnGameEnded()
    {
        Debug.Log("Game Ended");
        GameInProgress = false;
        foreach (PlayerInput player in PersistentPlayersManager.Instance.currentLevelData.playerInputs) {
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            playerMovement?.TogglePlayerMovement(false);
        }
    }

    public void StartGame() {
        GameStartEvent.Invoke();
    }

    private IEnumerator<float> _HowToPlay() {
        yield return Timing.WaitForSeconds(howToPlayDuration);
        howToPlayDone = true;
    }
}