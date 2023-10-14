using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MenuContainer : MonoBehaviour {
    public List<MenuItem> menuItems;

    private PlayerInputActions inputActions;
    EventSystem eventSystem;

    private void Start() {
        Init();
        InitInput();

    }

    private void OnDisable() {
        inputActions.UI.Disable();
    }

    private void Init() {
        foreach (Transform child in transform) {
            MenuItem menuItem = child.GetComponent<MenuItem>();
            if (menuItem == null) {
                continue;
            }

            menuItem.Init();
            menuItems.Add(menuItem);
        }
    }

    private void InitInput() {
        eventSystem = EventSystem.current;
        inputActions.UI.Enable();
        inputActions.UI.Navigation.performed += MenuNavigation;
        inputActions.UI.Confirm.performed += ConfirmAction;
    }

    //public void StartGame() {
    //    SceneManager.LoadScene(MAIN_SCENE_NAME);
    //}

    //public void QuitGame() {
    //    Application.Quit();
    //}

    public void MenuNavigation(InputAction.CallbackContext context) {

    }

    public void ConfirmAction(InputAction.CallbackContext context) {
        eventSystem.currentSelectedGameObject.GetComponent<MenuItem>().DoAction();
    }

    //public void Pause(InputAction.CallbackContext context) {
    //    if (!inGameScene) {
    //        return;
    //    }

    //    isPaused = !isPaused;

    //    int timeScale = isPaused ? 0 : 1;
    //    Time.timeScale = timeScale;
    //}
}
