using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerPossession : MonoBehaviour
{
    [SerializeField]
    private Vector3 possessableAttachPoint;

    // Actions triggered by Possessables
    public UnityAction<Possessable, PlayerPossession> OnPossessionBeginAction;
    public UnityAction<Possessable, PlayerPossession> OnPossessionEndAction;

    private Possessable currentlyPossessing;

    private void Start()
    {
        // Attach possession begin/end events to respective UnityActions
        OnPossessionBeginAction += OnPossessionBegin;
        OnPossessionEndAction += OnPossessionEnd;
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

        currentlyPossessing.Eject();
    }

    public void PassMovementInput(Vector2 inputVector)
    {
        // Just a stub - use this to 
    }
    #endregion Possession Actions


    #region Possession Callbacks
    private void OnPossessionBegin(Possessable possessable, PlayerPossession possessor)
    {
        if (possessor == this)
        {
            currentlyPossessing = possessable;
        }
    }


    private void OnPossessionEnd(Possessable possessable, PlayerPossession possessor)
    {
        if (possessor == this)
        {
            currentlyPossessing = null;
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
}
