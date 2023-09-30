using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossessableDetector : MonoBehaviour
{
    private List<Possessable> possessables = new List<Possessable>();

    //NOTE: This assumes that this object's collider can only collide with the Possessable physics layer
    private void OnTriggerEnter(Collider other)
    {
        Possessable possessable = other.gameObject.GetComponentInParent<Possessable>();
        if (!possessable || possessables.Contains(possessable))
            return;

        possessables.Add(possessable);

        Debug.Log("Touching Possessable: " + possessable.gameObject.name);
    }

    private void OnTriggerExit(Collider other)
    {
        Possessable possessable = other.gameObject.GetComponentInParent<Possessable>();
        if (!possessable || !possessables.Contains(possessable))
            return;

        possessables.Remove(possessable);
        Debug.Log("Stopped touching Possessable: " + possessable.gameObject.name);
    }

    public Possessable GetAvailablePossessable()
    {
        Possessable possessable = null;
        for (int i = possessables.Count - 1; i >= 0; i--)
        {
            if (possessables[i].CanBePossessed())
            {
                possessable = possessables[i];
                break;
            }
        }

        return possessable;
    }
}
