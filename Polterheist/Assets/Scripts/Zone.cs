using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EZone { ZoneA = 1, ZoneB = 2, none = 0 }

public class Zone : MonoBehaviour
{ 
    public EZone thisZone = EZone.none;
    public LayerMask m_LayerMask;
    public int maxPossessablesInLevel = 100;

    public Collider[] possessableColliders;

    private void Start()
    {
        possessableColliders = new Collider[maxPossessablesInLevel];
    }

    void FixedUpdate()
    {
        GetCollidingPossessables();
    }

    // Gets all possessables and puts them in the possessableColliders array
    void GetCollidingPossessables()
    {
        Array.Clear(possessableColliders, 0, possessableColliders.Length);
        Physics.OverlapBoxNonAlloc(gameObject.transform.position, transform.localScale / 2, possessableColliders, Quaternion.identity, m_LayerMask);
    }

    // Gets score from all possessables in the zone
    public int GetScore()
    {
        int score = 0;

        int i = 0;
        while (possessableColliders[i] != null)
        {
            Possessable possessable = possessableColliders[i].GetComponentInParent<Possessable>();
            if (possessable != null && !possessable.WasScored)
            {
                score += possessable.ScoreValue;
                possessable.WasScored = true;
            }
            i++;
        }

        return score;
    }

    //Draw the Box Overlap as a gizmo to show where it currently is testing. Click the Gizmos button to see this
    void OnDrawGizmos()
    {
        switch (thisZone)
        {
            case EZone.ZoneA:
                {
                    Gizmos.color = Color.blue;
                    break;
                }
            case EZone.ZoneB:
                {
                    Gizmos.color = Color.red;
                    break;
                }
            default:
                {
                    Gizmos.color = Color.black;
                    break;
                }
        }

        //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
