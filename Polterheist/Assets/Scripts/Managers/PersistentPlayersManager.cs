using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


// Utility to reuse the same player ID for each game controller, across levels
public class PersistentPlayersManager : MonoBehaviour
{
    public class CurrentLevelData {
        public List<PlayerInput> playerInputs = new List<PlayerInput>();
        public List<PlayerPossession> playerPosessions = new List<PlayerPossession>();
        public int numPlayersReady;
        public GameObject[] spawnPoints;

        public void ResetData() {
            numPlayersReady = 0;
            playerInputs.Clear();
            playerPosessions.Clear();
            spawnPoints = null;
        }
    }

    private static PersistentPlayersManager instance;
    public static PersistentPlayersManager Instance
    {
        get { return instance; }
        private set { }
    }

    private const string SPAWN_TAG = "SpawnPoint";
    
    private Dictionary<int, string> devicePlayerIdMap = new Dictionary<int, string>();
    private const string playerIdGlyphs = "abcdefghijklmnopqrstuvwxyz0123456789";
    private Dictionary<string, PlayerData> playerDataDictionary = new Dictionary<string, PlayerData>();
    public HatData[] hats;

    [SerializeField]
    private TeamData[] teamDatas;
    public Dictionary<TeamData.Team, TeamData> teamDataDictionary = new Dictionary<TeamData.Team, TeamData>();
    public const TeamData.Team DEFAULT_TEAM = TeamData.Team.White;

    public CurrentLevelData currentLevelData {
        get {
            if (_currentLevelData == null) {
                InitData();
            }
            return _currentLevelData;
        }
        private set { }
    }
    private CurrentLevelData _currentLevelData;

    private void Awake()
    {
        // Check if the instance already exists and destroy the new instance if it does
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        // Set the instance to this object if it doesn't exist
        instance = this;

        SceneManager.sceneLoaded += OnSceneLoaded;

        // TODO: This only works on root gameobjects! Has no effect if this gameobject is a child of GameManager, for instance
        // Prevent the instance from being destroyed when loading new scenes
        DontDestroyOnLoad(this.gameObject);

        // Load the team data objects into a dictionary, using the team enum as the key
        foreach(TeamData teamData in teamDatas)
        {
            teamDataDictionary.Add(teamData.team, teamData);
        }
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// Set up a new player, given its "player index' from the Input system
    /// </summary>
    /// <param name="playerInputIndex">The PlayerInput controller index</param>
    /// <returns>The playerID</returns>
    public string TryAddPlayer(int playerInputIndex)
    {
        if (devicePlayerIdMap.ContainsKey(playerInputIndex))
        {
            return devicePlayerIdMap[playerInputIndex];
        }

        // Make new player ID if this is a new device
        string playerId = PlayerIdHash();
        devicePlayerIdMap.Add(playerInputIndex, playerId);
        PlayerData newPlayerData = new PlayerData();
        newPlayerData.team = DEFAULT_TEAM;
        newPlayerData.playerInputIndex = playerInputIndex;
        playerDataDictionary.Add(playerId, newPlayerData);

        return playerId;
    }

    public bool RemovePlayer(int inputPlayerIndex) {
        if (devicePlayerIdMap == null) {
            return false;
        }
        return devicePlayerIdMap.Remove(inputPlayerIndex);
    }

    public int GetActivePlayerCount() {
        return currentLevelData.numPlayersReady;
    }

    public PlayerData GetPlayerData(string playerID) {
        return playerDataDictionary[playerID];
    }

    public TeamData.Team GetPlayerTeam(string playerID) {
        return GetPlayerData(playerID).team;
    }

    public void SetPlayerTeam(string playerID, TeamData.Team newTeam) {
        playerDataDictionary[playerID].team = newTeam;
    }

    public void SetPlayerHat(string playerID, HatData newHatData) {
        playerDataDictionary[playerID].hatData = newHatData;
    }

    public TeamData GetTeamData(TeamData.Team team) {
        return teamDataDictionary[team];
    }

    public TeamData GetPlayerTeamData(string playerID) {
        return GetTeamData(GetPlayerTeam(playerID));
    }

    // Get the player ID using the Input system's "player index" (one per player)
    public string GetDevicePlayerId(int inputPlayerIndex)
    {
        string playerId;
        devicePlayerIdMap.TryGetValue(inputPlayerIndex, out playerId);
        return playerId;
    }

    private string PlayerIdHash()
    {
        int charAmount = UnityEngine.Random.Range(8, 14);
        string result = "";
        for (int i = 0; i < charAmount; i++)
        {
            result += playerIdGlyphs[UnityEngine.Random.Range(0, playerIdGlyphs.Length)];
        }
        return result;
    }
    
    /// <summary>
    /// Assigns the the player their cosmetics
    /// </summary>
    /// <param name="playerInput">The player</param>
    public void RegisterPlayer(PlayerInput playerInput) {
        // Keep each "player" in the Input system (each given a player index) tied to one player ID
        // This is intended to keep player-specific data the same between levels
        int inputPlayerIndex = playerInput.playerIndex;
        string playerID = TryAddPlayer(inputPlayerIndex);

        PlayerPossession playerPossession = playerInput.GetComponent<PlayerPossession>();
        playerPossession.SetPlayerID(playerID);
        playerPossession.DressPlayer();
        _currentLevelData.playerInputs.Add(playerInput);
        _currentLevelData.playerPosessions.Add(playerPossession);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        InitData();
    }

    private void InitData() {
        _currentLevelData = new CurrentLevelData();
        _currentLevelData.ResetData();
        _currentLevelData.spawnPoints = GameObject.FindGameObjectsWithTag(SPAWN_TAG);
        PlayerInput[] playerInputs = FindObjectsOfType<PlayerInput>();
        foreach (PlayerInput playerInput in playerInputs) {
            RegisterPlayer(playerInput);
        }
    }
}