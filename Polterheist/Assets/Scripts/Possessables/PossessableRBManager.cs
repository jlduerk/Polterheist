using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This is placed on all Possessable objects that can be moved.
 */

public class PossessableRBManager : MonoBehaviour
{
    private SpringJoint springJoint = null;

    private Rigidbody GetSpringAttachedRB()
    {
        if (!springJoint)
            return null;

        return springJoint.connectedBody;
    }


    // Create and attach spring joint to given rigidbody
    public void AttachSpringTo(Rigidbody rb, Vector3 anchorPoint)
    {
        // Don't attach something else if already attached
        if (GetSpringAttachedRB())
            return;

        springJoint = gameObject.AddComponent<SpringJoint>();
        springJoint.connectedBody = rb;
        springJoint.autoConfigureConnectedAnchor = false;
        springJoint.connectedAnchor = anchorPoint;
    }


    // Destroy the current spring joint, if it exists
    public void DetachSpring()
    {
        if (!springJoint)
            return;

        Destroy(GetComponent<SpringJoint>());
        springJoint = null;
    }
}
