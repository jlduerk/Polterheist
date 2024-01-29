using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

[System.Serializable]
public struct DebugKey {
    public enum FunctionType {
        Reset = 0,
        CycleKeyboardPlayer,
    }
    
    public KeyCode keyCode;
    public FunctionType functionType;
}
public class DebugManager : MonoBehaviour {
    private static DebugManager instance;
    public static DebugManager Instance {
        get { return instance; }
        private set { }
    }

    public bool enableDebugManager;
    public bool showCursor;
    public DebugKey[] debugKeys;
    private const string MAIN_MENU_SCENE_NAME = "MainMenu";
    EventSystem eventSystem;
    
    void Awake()  {
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

    private void Start() {
        ShowCursor();
    }

    private void Update() {
        if (!enableDebugManager) {
            return;
        }
        foreach (DebugKey debugKey in debugKeys) {
            if (Input.GetKeyDown(debugKey.keyCode)) {
                DebugCommand(debugKey.functionType);
            }
        }
    }
    
    private void DebugCommand(DebugKey.FunctionType functionType) {
        switch (functionType) {
            case DebugKey.FunctionType.Reset:
                ResetGame();
                break;
            case DebugKey.FunctionType.CycleKeyboardPlayer:
                CycleKeyboardPlayer();
                break;
        }

        Debug.Log(EventSystem.current.currentSelectedGameObject);
    }

    
    #region Debug Commands
    private void ResetGame() {
        SceneManager.LoadScene(MAIN_MENU_SCENE_NAME);
    }

    // Index of the player to control with the keyboard; -1 means no players are controlled with keyboard
    private int controlledPlayer = -1;
    // Switches keyboard input to the next player
    private void CycleKeyboardPlayer() {
        //PlayerInput[] players = FindObjectsByType<PlayerInput>(FindObjectsSortMode.InstanceID);
        int playerCount = PlayerInput.all.Count;
        if (playerCount == 0)
        {
            controlledPlayer = -1;
            return;
        }

        InputDevice keyboardDevice = InputSystem.GetDevice("Keyboard");
        if (keyboardDevice == null)
            return;

        if (controlledPlayer >= playerCount)
            controlledPlayer = -1;

        Func<PlayerInput, InputDevice> FindGamepad = player => 
        {
            foreach(InputDevice device in player.devices)
            {
                if (device is Gamepad)
                    return device;
            }
            return null;
        };

        if (controlledPlayer > -1)
        {
            PlayerInput player = PlayerInput.GetPlayerByIndex(controlledPlayer);
            player.user.UnpairDevice(keyboardDevice);
            InputDevice gamepadDevice = FindGamepad(player);
            if (gamepadDevice == null)
                player.SwitchCurrentControlScheme("Gamepad");
            else
                player.SwitchCurrentControlScheme("Gamepad", gamepadDevice);
        }

        controlledPlayer++;
        if (controlledPlayer >= playerCount)
            controlledPlayer = -1;

        if (controlledPlayer > -1)
        {
            PlayerInput player = PlayerInput.GetPlayerByIndex(controlledPlayer);
            InputUser.PerformPairingWithDevice(keyboardDevice, user: player.user);
            InputDevice gamepadDevice = FindGamepad(player);
            if (gamepadDevice == null)
                player.SwitchCurrentControlScheme("Keyboard", keyboardDevice);
            else
                player.SwitchCurrentControlScheme("Keyboard", keyboardDevice, gamepadDevice);
            Debug.Log($"DEBUG: Player {controlledPlayer} is now controlled by the keyboard.");
        }
        else
        {
            Debug.Log($"DEBUG: No players are being controlled by the keyboard.");
        }
    }

    private void ShowCursor() {
        Cursor.visible = showCursor;
    }
    //ADD MORE DEBUG COMMANDS HERE
    #endregion
}
