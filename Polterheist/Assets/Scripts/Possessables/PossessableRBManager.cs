using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This is placed on all Possessable objects that can be moved.
 */

[RequireComponent(typeof(Rigidbody))]
public class PossessableRBManager : MonoBehaviour
{
    private Dictionary<string, SpringJoint> springJoints = new Dictionary<string, SpringJoint>();

    private Rigidbody rb = null;
    public Rigidbody possessableRigidBody {
        get { return rb; }
        private set { }
    }

    private void Start() {
        rb = GetComponent<Rigidbody>();
    }

    private SpringJoint GetSpringAttached(string playerID)
    {
        SpringJoint joint = null;
        if (!springJoints.TryGetValue(playerID, out joint))
            return null;

        return joint;
    }


    // Create and attach spring joint to given rigidbody
    public void AttachSpringTo(Rigidbody rb, string playerID, Vector3 anchorPoint)
    {
        // Don't attach something else if already attached
        SpringJoint springJoint = GetSpringAttached(playerID);
        if (springJoint)
            return;

        springJoint = gameObject.AddComponent<SpringJoint>();
        springJoint.connectedBody = rb;
        springJoint.autoConfigureConnectedAnchor = false;
        springJoint.connectedAnchor = anchorPoint;
        springJoint.maxDistance = 0.25f;
        springJoint.spring = 20;
        springJoints.Add(playerID, springJoint);

        // TODO: Find appropriate spring strength
        // TODO: Might want to reduce gravity for possessables, so they don't drag as much
    }


    // Destroy the current spring joint, if it exists
    public void DetachSpring(string playerID)
    {
        SpringJoint springJoint = GetSpringAttached(playerID);
        if (!springJoint)
            return;
        springJoints.Remove(playerID);
        Destroy(springJoint);
    }
}
