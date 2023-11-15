using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Utility to reuse the same player ID for each game controller, across levels

public class PersistentPlayersManager : MonoBehaviour
{
    private static PersistentPlayersManager instance;
    public static PersistentPlayersManager Instance
    {
        get { return instance; }
        private set { }
    }
    
    private Dictionary<int, string> devicePlayerIdMap = new Dictionary<int, string>();

    private const string playerIdGlyphs = "abcdefghijklmnopqrstuvwxyz0123456789";

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
        // Prevent the instance from being destroyed when loading new scenes
        DontDestroyOnLoad(this.gameObject);
    }

    // Set up a new player, given its "player index' from the Input system
    public bool AddPlayer(int inputPlayerIndex)
    {
        if (devicePlayerIdMap.ContainsKey(inputPlayerIndex))
        {
            return false;
        }

        // Make new player ID if this is a new device
        string playerId = PlayerIdHash();
        devicePlayerIdMap.Add(inputPlayerIndex, playerId);

        return true;
    }

    public bool RemovePlayer(int inputPlayerIndex) {
        if (devicePlayerIdMap == null) {
            return false;
        }
        return devicePlayerIdMap.Remove(inputPlayerIndex);
    }

    public int GetActivePlayerCount() {
        if (devicePlayerIdMap == null) {
            return 0;
        }

        return devicePlayerIdMap.Count;
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
}
