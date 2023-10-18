using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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


    public Possessable currPossessable;
    private PlayerMovement playerMovement;
    private Ghost ghost;

    private PlayerInput playerInputComponent = null;
    private float ghostSpeed;

    private const string POSSESSION_INPUT_ID = "Possess";
    private const string HAUNT_INPUT_ID = "Haunt";
    public string possessSFX = "Possess";
    public string unPossessSFX = "Unpossess";
    public string hauntSFX = "Haunt";
    public string playerID = "";
    const string glyphs = "abcdefghijklmnopqrstuvwxyz0123456789";



    private void Start()
    {
        playerInputComponent = GetComponent<PlayerInput>();
        playerInputComponent.onActionTriggered += HandleInput;

        ghost = GetComponentInChildren<Ghost>();
        
        // Attach possession begin/end events to respective UnityActions
        OnPossessionBeginAction += OnPossessionBegin;
        OnPossessionEndAction += OnPossessionEnd;

        playerMovement = GetComponent<PlayerMovement>();
        ghostSpeed = playerMovement.speed;
        playerID = makeHash();
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
            Debug.Log("Nice try. that dont do shit");
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
        renderer.material.SetFloat("_Opacity", 1);
        currPossessable.Eject(this);
    }
    #endregion Possession Actions


    #region Possession Callbacks
    private void OnPossessionBegin(Possessable possessable, PlayerPossession possessor)
    {
        if (possessor == this)
        {
            currPossessable = possessable;
            playerMovement.speed /= possessable.gameObject.GetComponent<Rigidbody>().mass;
            renderer.material.SetFloat("_Opacity", .5f);
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

    #region Team Data
    private string makeHash()
    {
        int charAmount = UnityEngine.Random.Range(8, 14);
        string result = "";
        for (int i = 0; i < charAmount; i++)
        {
            result += glyphs[UnityEngine.Random.Range(0, glyphs.Length)];
        }
        return result;
    }

    static int redPlayerNum = 0;
    static int bluePlayerNum = 0;
    public void TeamDataInit(TeamData teamDataToAssign) {
        teamData = teamDataToAssign;
        Color randomPlayerColor = teamDataToAssign.playerColor;
        if (teamData.team == TeamData.Team.Blue) {
            bluePlayerNum++;
            randomPlayerColor.g += bluePlayerNum%2 * .4f;
            Debug.Log("BLUE TEAM");
        }
        else if(teamData.team == TeamData.Team.Red)
        {
            redPlayerNum++;
            randomPlayerColor.g += redPlayerNum%2 * .2f;
            Debug.Log("red TEAM");

        }
        Debug.Log("BLUE TEAM");

        renderer.material.SetColor("_Color", randomPlayerColor);
    }
    #endregion
}
