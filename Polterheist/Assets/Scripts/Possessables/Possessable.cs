using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Possessable : MonoBehaviour
{
    // How many points a team will get from this possessable in their zone
    public int ScoreValue = 1;

    // When counting score, used to ensure we don't count a possessable twice
    public bool WasScored = false;

    // Events to signal when something has become possessed
    [SerializeField] private UnityEvent<Possessable, PlayerPossession> OnPossessionBegin = new UnityEvent<Possessable, PlayerPossession>();
    [SerializeField] private UnityEvent<Possessable, PlayerPossession> OnPossessionEnd = new UnityEvent<Possessable, PlayerPossession>();
    [SerializeField] private UnityEvent OnHaunt = new UnityEvent();


    // Handles Rigidbody logic, such as spring joints
    [HideInInspector] public PossessableRBManager rbManager = null;

    private PlayerPossession possessedBy = null;

    private MeshRenderer renderer;
    private Material defaultMaterial;
    public bool IsPossessed => possessedBy != null;
    private void Start()
    {
        // PossessableRBManager is optional - but get a reference to it if it's there
        rbManager = GetComponent<PossessableRBManager>();
        renderer = GetComponentInChildren<MeshRenderer>();
        defaultMaterial = renderer.material;
    }


    // Return whether this Possessable is ready to be possessed
    public bool CanBePossessed()
    {
        // TODO: may want to be able to "lock" possessables, for some reason
        return !IsPossessed;
    }


    // Try to possess this object using the given player
    // Return whether that was successful
    public bool TryPossess(PlayerPossession player)
    {
        if (!CanBePossessed() || !player)
            return false;

        // Store player and have it listen for possession events
        possessedBy = player;
        OnPossessionBegin.AddListener(possessedBy.OnPossessionBeginAction);
        OnPossessionEnd.AddListener(possessedBy.OnPossessionEndAction);


        OnPossessionBegin.Invoke(this, possessedBy);

        Debug.Log(possessedBy.gameObject.name + " started possessing " + this.gameObject.name, this);

        // Create the spring joint
        // TODO: Maybe the rbManager could just listen to OnPossessionBegin/End
        if (rbManager)
            rbManager.AttachSpringTo(player.GetPlayerRB(), player.PossessableAttachPoint);

        return true;
    }


    // Un-possess this object
    public void Eject()
    {
        if (IsPossessed)
        {
            OnPossessionEnd.Invoke(this, possessedBy);

            OnPossessionBegin.RemoveListener(possessedBy.OnPossessionBeginAction);
            OnPossessionEnd.RemoveListener(possessedBy.OnPossessionEndAction);

            Debug.Log(possessedBy.gameObject.name + " stopped possessing " + this.gameObject.name, this);
        }

        if (rbManager)
            rbManager.DetachSpring();

        possessedBy = null;
    }

    public void SetColor(Material material) {
        if (material == null) {
            renderer.material = defaultMaterial;
            return;
        }
        renderer.material = material;
    }

    public void Haunt()
    {
        Debug.Log("HAUNT NOWf");
        OnHaunt.Invoke();
    }
}
