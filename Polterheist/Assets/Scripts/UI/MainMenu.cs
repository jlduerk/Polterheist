using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    private PlayerInputActions inputActions;
    EventSystem eventSystem;
    private const string MAIN_SCENE_NAME = "Main";
    private bool isPaused;
    public bool inGameScene;

    private void Start() {
        InitInput();
        if (SceneManager.GetActiveScene().name == MAIN_SCENE_NAME) {
            inGameScene = true;
        }
    }

    private void OnDisable() {
        inputActions.UI.Disable();
    }

    private void InitInput() {
        eventSystem = EventSystem.current;
        inputActions.UI.Enable();
        inputActions.UI.Navigation.performed += MenuNavigation;
        inputActions.UI.Confirm.performed += ConfirmAction;
    }

    public void StartGame() {
        SceneManager.LoadScene(MAIN_SCENE_NAME);
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void MenuNavigation(InputAction.CallbackContext context) {

    }

    public void ConfirmAction(InputAction.CallbackContext context) {
        eventSystem.currentSelectedGameObject.GetComponent<Button>();
    }

    public void Pause(InputAction.CallbackContext context) {
        if (!inGameScene) {
            return;
        }

        isPaused = !isPaused;
        
        int timeScale = isPaused ? 0 : 1;
        Time.timeScale = timeScale;
    }
}