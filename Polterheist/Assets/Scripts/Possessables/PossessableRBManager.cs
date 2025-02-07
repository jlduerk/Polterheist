using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
 * This is placed on all Possessable objects that can be moved.
 */

[RequireComponent(typeof(Rigidbody))]
public class PossessableRBManager : MonoBehaviour
{
    private Dictionary<string, SpringJoint> springJoints = new Dictionary<string, SpringJoint>();
    private Dictionary<string, LineRenderer> lineRenderers = new Dictionary<string, LineRenderer>();
    private Material lineRendererMaterial;
    private const string SHADER_INTENSITY_REFERENCE = "_Intensity";


    private Rigidbody rb = null;
    private Possessable posessable;

    public Rigidbody possessableRigidBody
    {
        get { return rb; }
        private set { }
    }

    private void Awake()
    {
        lineRendererMaterial = Resources.Load("tetherMat", typeof(Material)) as Material;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        posessable = GetComponent<Possessable>();
    }

    private void Update()
    {
        int forLoopLength = lineRenderers.Count;
        for (int i = 0; i < forLoopLength; i++)
        {
            KeyValuePair<string, LineRenderer> line = lineRenderers.ElementAt(i);
            SpringJoint joint = springJoints[line.Key];

            TeamData playerTeamData = PersistentPlayersManager.Instance.GetPlayerTeamData(line.Key);
            Debug.Log(joint.currentForce.magnitude);
            line.Value.startColor = Color.Lerp(playerTeamData.playerColor, Color.black, joint.currentForce.magnitude / 220);
            line.Value.endColor = playerTeamData.playerColor;

            line.Value.SetPositions(new Vector3[] { transform.position, joint.connectedBody.position });
            
            Debug.Log(joint.currentForce.magnitude/220);
            lineRendererMaterial.SetFloat(SHADER_INTENSITY_REFERENCE, joint.currentForce.magnitude/220);

            // detach if fighting over object

            //if (joint.currentForce.magnitude > 220 && springJoints.Count > 1)
            //{
            //    // particle effect
            //    posessable.EjectbyID(line.Key);
            //    return;
            //}

        }
    }

    private SpringJoint GetSpringAttached(string playerID)
    {
        SpringJoint joint = null;
        if (!springJoints.TryGetValue(playerID, out joint))
            return null;

        return joint;
    }

    private LineRenderer GetLineAttached(string playerID)
    {
        LineRenderer line = null;
        if (!lineRenderers.TryGetValue(playerID, out line))
            return null;

        return line;
    }


    // Create and attach spring joint to given rigidbody
    public void AttachSpringTo(Rigidbody rb, string playerID, Vector3 anchorPoint)
    {
        // Don't attach something else if already attached
        SpringJoint springJoint = GetSpringAttached(playerID);
        if (springJoint)
            return;

        TeamData playerTeamData = PersistentPlayersManager.Instance.GetPlayerTeamData(playerID);

        springJoint = gameObject.AddComponent<SpringJoint>();
        springJoint.connectedBody = rb;
        springJoint.autoConfigureConnectedAnchor = false;
        springJoint.connectedAnchor = anchorPoint;
        springJoint.maxDistance = 0.25f;
        springJoint.spring = 20;
        springJoints.Add(playerID, springJoint);

        LineRenderer lineRenderer = springJoint.connectedBody.gameObject.AddComponent<LineRenderer>();
        lineRenderer.enabled = true;
        lineRenderer.material = lineRendererMaterial;
        lineRenderer.startColor = playerTeamData.playerColor;
        lineRenderer.startWidth = 3;
        lineRenderers.Add(playerID, lineRenderer);

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

        LineRenderer line = GetLineAttached(playerID);
        if (!line)
            return;
        lineRenderers.Remove(playerID);
        Destroy(line);
    }
}
