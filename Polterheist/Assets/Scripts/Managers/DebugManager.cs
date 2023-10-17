using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct DebugKey {
    public enum FunctionType {
        Reset = 0,
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
        }

        Debug.Log(EventSystem.current.currentSelectedGameObject);
    }

    
    #region Debug Commands
    private void ResetGame() {
        SceneManager.LoadScene(MAIN_MENU_SCENE_NAME);
    }

    private void ShowCursor() {
        Cursor.visible = showCursor;
    }
    //ADD MORE DEBUG COMMANDS HERE
    #endregion
}
