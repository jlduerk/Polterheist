using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MenuContainer : MonoBehaviour {
    public Transform menuItemsContainer;
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
        menuItems = new List<MenuItem>();
        foreach (Transform child in menuItemsContainer) {
            MenuItem menuItem = child.GetComponent<MenuItem>();
            if (menuItem == null) {
                continue;
            }

            menuItem.Init();
            menuItems.Add(menuItem);
        }
    }

    private void InitInput() {
        inputActions = new PlayerInputActions();
        inputActions.UI.Enable();
        inputActions.UI.Confirm.performed += ConfirmAction;

        eventSystem = EventSystem.current;
        GameObject defaultSelectable = gameObject;
        if (menuItems.Count == 0 || menuItems[0] == null) {
            Debug.LogWarning("No Menu Items in the menuItems list!");
            return;
        }
        eventSystem.SetSelectedGameObject(menuItems[0].gameObject);
    }


    public void ConfirmAction(InputAction.CallbackContext context) {
        eventSystem.currentSelectedGameObject.GetComponent<MenuItem>().DoAction();
    }
}