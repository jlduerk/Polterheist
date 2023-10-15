using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using DG.Tweening;

public class MenuContainer : MonoBehaviour {
    private PlayerInputActions inputActions;
    protected EventSystem eventSystem;

    [Header("MenuItem Attributes")]
    public Transform menuItemsContainer;
    public List<MenuItem> menuItems;

    private CanvasGroup canvasGroup;
    [Header("Fade Attributes")]
    public float fadeInDuration;
    public Ease fadeInEase;
    public float fadeOutDuration;
    public Ease fadeOutEase;

    private void Start() {
        Init();
        InitInput();
    }

    private void OnDisable() {
        if (inputActions == null) {
            return;
        }
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

        canvasGroup = GetComponent<CanvasGroup>();
        FadeMenu(IsEnabled());
    }

    private void InitInput() {
        inputActions = new PlayerInputActions();
        inputActions.UI.Enable();
        inputActions.UI.Confirm.performed += ConfirmAction;
        inputActions.UI.StartButton.performed += StartButton;

        eventSystem = EventSystem.current;
        GameObject defaultSelectable = gameObject;
        if (menuItems.Count == 0 || menuItems[0] == null) {
            Debug.LogWarning("No Menu Items in the menuItems list!");
            return;
        }
        
        eventSystem.SetSelectedGameObject(menuItems[0].gameObject);
    }
    
    public void ConfirmAction(InputAction.CallbackContext context) {
        if (!IsEnabled()) {
            return;
        }
        if (eventSystem.currentSelectedGameObject == null) {
            Debug.LogError($"No selected MenuItem to click!");
            return;
        }
        eventSystem.currentSelectedGameObject.GetComponent<MenuItem>().DoAction();
    }

    public virtual void StartButton(InputAction.CallbackContext context) { }

    public void FadeMenu(bool enable) {
        if (enable) {
            DOTween.To(()=> canvasGroup.alpha, x=> canvasGroup.alpha = x, 1, fadeInDuration).SetEase(fadeInEase).SetUpdate(true);
        }
        else {
            DOTween.To(()=> canvasGroup.alpha, x=> canvasGroup.alpha = x, 0, fadeOutDuration).SetEase(fadeOutEase).SetUpdate(true);
        }

        ToggleMenuItems(enable);
    }

    public bool IsEnabled() {
        if (canvasGroup == null) {
            return false;
        }
        return canvasGroup.alpha > 0;
    }

    public void ToggleMenuItems(bool enabled) {
        foreach (MenuItem menuItem in menuItems) {
            menuItem.EnableToggle(enabled);
        }
    }
}