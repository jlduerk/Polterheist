using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using DG.Tweening;

public class MenuContainer : MonoBehaviour {
    private PlayerInputActions inputActions;
    protected EventSystem eventSystem;
    private bool initialized;

    [Header("MenuItem Attributes")]
    public Transform menuItemsContainer;
    public List<MenuItem> menuItems;

    private CanvasGroup canvasGroup;
    [Header("Fade Attributes")] 
    public float fadeInDuration;
    public Ease fadeInEase;
    public float fadeOutDuration;
    public Ease fadeOutEase;
    public bool cancelCanClose;
    public MenuContainer openContainerOnCancel;

    private void Start() {
        Init();
    }

    private void OnDisable() {
        if (inputActions == null) {
            return;
        }
        inputActions.UI.Disable();
    }

    private void Init() {
        //menu items
        if (menuItemsContainer != null) {
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

        //fades
        canvasGroup = GetComponent<CanvasGroup>();
        FadeMenu(IsEnabled());

        //input
        inputActions = new PlayerInputActions();
        inputActions.UI.Enable();
        inputActions.UI.Confirm.performed += ConfirmAction;
        inputActions.UI.StartButton.performed += StartAction;
        inputActions.UI.Cancel.performed += CancelAction;
        eventSystem = EventSystem.current;
        if (menuItems.Count == 0 || menuItems[0] == null) {
            Debug.LogWarning("No Menu Items in the menuItems list!");
            return;
        }
        
        //ready
        SelectFirstMenuItem();
        initialized = true;
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

    public virtual void StartAction(InputAction.CallbackContext context) { }

    public virtual void CancelAction(InputAction.CallbackContext context) {
        if (cancelCanClose) {
            FadeMenu(false);
            if (openContainerOnCancel) {
                openContainerOnCancel.FadeMenu(true);
            }
        }
    }

    public void FadeMenu(bool enable) {
        ToggleMenuItems(enable);
        if (enable) {
            DOTween.To(()=> canvasGroup.alpha, x=> canvasGroup.alpha = x, 1, fadeInDuration).SetEase(fadeInEase).SetUpdate(true);
        }
        else {
            DOTween.To(()=> canvasGroup.alpha, x=> canvasGroup.alpha = x, 0, fadeOutDuration).SetEase(fadeOutEase).SetUpdate(true);
        }
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
            if (menuItem.IsEnabled()) {
                menuItem.UnHighlight();
            }
        }

        if (enabled) {
            SelectFirstMenuItem();
        }
    }

    protected void SelectFirstMenuItem() {
        if (eventSystem == null) {
            eventSystem = EventSystem.current;
        }
        if (menuItems.Count != 0) {
            menuItems[0].Highlight();
            eventSystem.SetSelectedGameObject(menuItems[0].gameObject);
        }
    }
}