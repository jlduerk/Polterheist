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
    public TeamData[] teamDatas;
    private bool singlePlayerAllowed;

    private PlayerInputManager playerInputManager;
    public int playerCount;
    public Transform[] playerSpawnPoints;
    [HideInInspector] public List<PlayerInput> players = new List<PlayerInput>();

    public ScoreManager scoreManager;
    public GameFlowManager gameFlowManager;

    public bool GameInProgress = false;
    public bool Paused = false;

    public UnityEvent GameStartEvent;
    public UnityEvent GameEndEvent;

    public GameObject[] hats;
    private List<GameObject> availableHats = new List<GameObject>();
    private CoroutineHandle howToPlayCoroutine;
    public float howToPlayDuration = 5;
    public GameObject howToPlayPrompt;
    private bool howToPlayDone;
    private bool countdownStarted;

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
        howToPlayPrompt.SetActive(true);

        GameStartEvent.AddListener(OnGameStarted);
        GameEndEvent.AddListener(OnGameEnded);

        availableHats.AddRange(hats);

        howToPlayCoroutine = Timing.RunCoroutine(_HowToPlay());
    }

    private void Update() {
        if (!howToPlayDone || countdownStarted) {
            return;
        }
        if (singlePlayerAllowed && playerCount > 0) {
            gameFlowManager.StartCountdown();
            countdownStarted = true;
            return;
        }
        if (playerCount >= 2) {
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
    
    public void RegisterPlayer(PlayerInput playerInput) {
        playerCount++;
        int counterModulo = playerInput.playerIndex % teamDatas.Length;
        playerInput.gameObject.GetComponent<PlayerPossession>().TeamDataInit(teamDatas[counterModulo]);

        players.Add(playerInput);
        playerInput.gameObject.transform.position = playerSpawnPoints[players.Count - 1].position;
        PlayerPossession playerPossession = playerInput.GetComponent<PlayerPossession>();
        RandomizeHat(playerPossession);
    }

    private void RandomizeHat(PlayerPossession playerPossession) {
        int rand = Random.Range(0, availableHats.Count);
        GameObject hatToSpawn = availableHats[rand];
        availableHats.Remove(hatToSpawn);
        GameObject spawnedHat = Instantiate(hatToSpawn, playerPossession.GetHatAttachPoint());
        Ghost ghost = playerPossession.GetComponentInChildren<Ghost>();
        ghost.hatRenderer = spawnedHat.GetComponent<Renderer>(); 
    }

    private IEnumerator<float> _HowToPlay() {
        yield return Timing.WaitForSeconds(howToPlayDuration);
        howToPlayPrompt.SetActive(false);
        howToPlayDone = true;
    }
}