using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossessableDetector : MonoBehaviour
{
    private List<Possessable> possiblePossessables = new List<Possessable>();
    private PlayerPossession playerPossession;

    private void Start() {
        playerPossession = GetComponentInParent<PlayerPossession>();
    }

    //NOTE: This assumes that this object's collider can only collide with the Possessable physics layer
    private void OnTriggerEnter(Collider other)
    {
        Possessable possessable = other.gameObject.GetComponentInParent<Possessable>();
        if (playerPossession.currentlyPossessing || !possessable || possessable.IsPossessed || possiblePossessables.Contains(possessable))
            return;

        possiblePossessables.Add(possessable);
        OnAddPossiblePossessable(possessable);
        Debug.Log("Touching Possessable: " + possessable.gameObject.name);
    }

    private void OnTriggerExit(Collider other)
    {
        Possessable possessable = other.gameObject.GetComponentInParent<Possessable>();
        if (playerPossession.currentlyPossessing || !possessable || !possiblePossessables.Contains(possessable))
            return;

        possiblePossessables.Remove(possessable);
        OnRemovePossiblePossessable(possessable);
        Debug.Log("Stopped touching Possessable: " + possessable.gameObject.name);
    }

    public Possessable GetAvailablePossessable()
    {
        Possessable possessable = null;
        for (int i = possiblePossessables.Count - 1; i >= 0; i--)
        {
            if (possiblePossessables[i].CanBePossessed())
            {
                possessable = possiblePossessables[i];
                break;
            }
        }

        return possessable;
    }

    // Clear all possible Possessables in range, and revert them to their original visual state
    public void ClearPossiblePossessables()
    {
        for (int i = 0;  i < possiblePossessables.Count; i++)
        {
            if (possiblePossessables[i] == playerPossession.currentlyPossessing)
                continue;

            OnRemovePossiblePossessable(possiblePossessables[i]);
        }

        possiblePossessables.Clear();
    }

    private void OnAddPossiblePossessable(Possessable possessable)
    {
        possessable.SetColor(playerPossession.teamData.teamMaterialHover);
    }

    private void OnRemovePossiblePossessable(Possessable possessable)
    {
        possessable.SetColor(null);
    }
}