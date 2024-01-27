using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerPossession : MonoBehaviour {
    public TeamData teamData;
    public MeshRenderer meshRenderer;

    [SerializeField] private Transform hatAttachPoint;
    [SerializeField] private Vector3 possessableAttachPoint;
    [SerializeField] private PossessableDetector possessableDetector;


    // Actions triggered by Possessables
    public UnityAction<Possessable, PlayerPossession> OnPossessionBeginAction;
    public UnityAction<Possessable, PlayerPossession> OnPossessionEndAction;


    public Possessable currPossessable;
    private PlayerMovement playerMovement;

    private PlayerInput playerInputComponent = null;
    private float ghostSpeed;

    private const string POSSESSION_INPUT_ID = "Possess";
    private const string HAUNT_INPUT_ID = "Haunt";
    public string possessSFX = "Possess";
    public string unPossessSFX = "Unpossess";
    public string hauntSFX = "Haunt";
    private const int MAX_MASS = 5;

    private string playerID;
    public string PlayerID => playerID;
    public PlayerData PlayerData => PersistentPlayersManager.Instance.GetPlayerData(PlayerID);


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
        if (currPossessable)
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
        if (currPossessable)
        { 
            currPossessable.Haunt(this);
        }
        else
        {
            playerMovement.Dash();
            //Debug.Log("Nice try. that dont do shit");
        }
        
        //AudioManager.Instance.Play(possessSFX);
    }


    #region Possession Actions
    public void TryPossess(Possessable possessable)
    {
        // Just try to possess - if it succeeds, OnPossessionBegin should fire
        possessable.TryPossess(this);
    }

    public void Unpossess()
    {
        if (!currPossessable)
            return;
        meshRenderer.material.SetFloat("_Opacity", 1);
        currPossessable.Eject(this);
    }
    #endregion Possession Actions


    #region Possession Callbacks
    private void OnPossessionBegin(Possessable possessable, PlayerPossession possessor)
    {
        if (possessor == this)
        {
            currPossessable = possessable;
            playerMovement.speed /= Mathf.Clamp(possessable.gameObject.GetComponent<Rigidbody>().mass, 1, MAX_MASS);
            meshRenderer.material.SetFloat("_Opacity", .5f);
            possessableDetector.ClearPossiblePossessables();
            //ghost.dropShadow.gameObject.SetActive(false);
        }
        
        AudioManager.Instance.Play(possessSFX);
    }


    private void OnPossessionEnd(Possessable possessable, PlayerPossession possessor)
    {
        if (possessor == this)
        {
            currPossessable = null;
            playerMovement.speed = ghostSpeed;
            //ghost.dropShadow.gameObject.SetActive(true);
        }
        
        AudioManager.Instance.Play(unPossessSFX);
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

    public void DressPlayer() {
        TeamDataInit(PlayerData.teamData);
        HatData hatData = PlayerData.hatData;
        if (hatData) {
            GameObject spawnedHat = Instantiate(hatData.hatPrefab, GetHatAttachPoint());
            Ghost ghost = GetComponentInChildren<Ghost>();
            ghost.hatRenderer = spawnedHat.GetComponent<Renderer>();
        }
    }

    public Transform GetHatAttachPoint() {
        return hatAttachPoint;
    }

    public void AssignTeamData(TeamData teamDataToAssign) {
        teamData = teamDataToAssign;
    }

    public void TeamDataInit(TeamData teamDataToAssign) {
        teamData = teamDataToAssign;
        Color randomPlayerColor = teamDataToAssign.playerColor;
        if (teamData.team == TeamData.Team.Blue) {
            randomPlayerColor.g += PlayerData.playerInputIndex % 2 * .4f;
            randomPlayerColor.r -= PlayerData.playerInputIndex % 2 * .2f;
        }
        else if(teamData.team == TeamData.Team.Red)
        {
            randomPlayerColor.g += PlayerData.playerInputIndex % 2 * .2f;

        }

        meshRenderer.material.SetColor("_Color", randomPlayerColor);
    }

    public PlayerMovement GetPlayerMovement() {
        return playerMovement;
    }

    public void SetPlayerID(string id) {
        playerID = id;
    }
}