using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerPossession : MonoBehaviour {
    public TeamData teamData;
    public MeshRenderer renderer;

    [SerializeField] private Vector3 possessableAttachPoint;
    [SerializeField] private PossessableDetector possessableDetector;


    // Actions triggered by Possessables
    public UnityAction<Possessable, PlayerPossession> OnPossessionBeginAction;
    public UnityAction<Possessable, PlayerPossession> OnPossessionEndAction;


    private Possessable currentlyPossessing;
    private PlayerMovement playerMovement;

    private PlayerInput playerInputComponent = null;
    private float ghostSpeed;

    private const string POSSESSION_INPUT_ID = "Possess";
    private const string HAUNT_INPUT_ID = "Haunt";


    private void Start()
    {
        playerInputComponent = GetComponent<PlayerInput>();
        playerInputComponent.onActionTriggered += HandleInput;

        // Attach possession begin/end events to respective UnityActions
        OnPossessionBeginAction += OnPossessionBegin;
        OnPossessionEndAction += OnPossessionEnd;

        playerMovement = GetComponent<PlayerMovement>();
        ghostSpeed = playerMovement.speed;
    }

    private void OnDisable() {

        OnPossessionBeginAction -= OnPossessionBegin;
        OnPossessionEndAction -= OnPossessionEnd;
    }

    /*
     * Handle the general onActionTriggered event from PlayerInput component
     * This is a little cumbersome in code, but seems more flexible than hooking up functions
     * to UnityEvents created by the PlayerInput component (since we can make/control our own UnityEvents)
     */
    public void HandleInput(InputAction.CallbackContext context)
    {
        if (context.action.name.Equals(POSSESSION_INPUT_ID) && context.performed)
        {
            OnPossessButtonPressed();
        }
        if (context.action.name.Equals(HAUNT_INPUT_ID) && context.performed)
        {
            OnHauntButtonPressed();
        }
    }


    public void OnPossessButtonPressed()
    {
        if (currentlyPossessing)
        {
            Unpossess();
            return;
        }
        
        Possessable nextPossessable = possessableDetector.GetAvailablePossessable();
        if (!nextPossessable)
            return;

        TryPossess(nextPossessable);
    }

    public void OnHauntButtonPressed()
    {
        if (currentlyPossessing)
        {
           currentlyPossessing.Haunt();
        }
        else
        {
            Debug.Log("Nice try. that dont do shit");
        }
    }


    #region Possession Actions
    public void TryPossess(Possessable possessable)
    {
        // Just try to possess - if it succeeds, OnPossessionBegin should fire
        possessable.TryPossess(this);
    }

    public void Unpossess()
    {
        if (!currentlyPossessing)
            return;

        currentlyPossessing.SetColor(null);
        currentlyPossessing.Eject();
    }
    #endregion Possession Actions


    #region Possession Callbacks
    private void OnPossessionBegin(Possessable possessable, PlayerPossession possessor)
    {
        if (possessor == this)
        {
            currentlyPossessing = possessable;
            playerMovement.speed /= possessable.gameObject.GetComponent<Rigidbody>().mass;
            currentlyPossessing.SetColor(teamData.teamMaterial);
        }
    }


    private void OnPossessionEnd(Possessable possessable, PlayerPossession possessor)
    {
        if (possessor == this)
        {
            currentlyPossessing = null;
            playerMovement.speed = ghostSpeed;
        }
    }
    #endregion Possession Callbacks


    #region Getters
    public Vector3 PossessableAttachPoint => possessableAttachPoint;

    public Rigidbody GetPlayerRB()
    {
        // TODO: Rigidbody may ultimately not be on same object
        return gameObject.GetComponent<Rigidbody>();
    }
    #endregion Getters

    #region Team Data
    public void TeamDataInit(TeamData teamDataToAssign) {
        teamData = teamDataToAssign;

        renderer.material = teamData.teamMaterial;
    }
    #endregion
}
