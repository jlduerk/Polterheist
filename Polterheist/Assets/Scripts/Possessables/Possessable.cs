using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

[RequireComponent(typeof(Outline))]
public class Possessable : MonoBehaviour
{
    // How many points a team will get from this possessable in their zone
    public int ScoreValue = 1;
    public static bool limitToOnePossession = false;

    // When counting score, used to ensure we don't count a possessable twice
    public bool WasScored = false;

    [SerializeField] private GameObject orbDistortionPrefab = null;
    private GameObject distortionOrb;

    // Events to signal when something has become possessed
    [SerializeField] private UnityEvent<Possessable, PlayerPossession> OnPossessionBegin = new UnityEvent<Possessable, PlayerPossession>();
    [SerializeField] private UnityEvent<Possessable, PlayerPossession> OnPossessionEnd = new UnityEvent<Possessable, PlayerPossession>();
    [SerializeField] private UnityEvent<Possessable, PlayerPossession> OnHaunt = new UnityEvent<Possessable, PlayerPossession>();


    // Handles Rigidbody logic, such as spring joints
    [HideInInspector] public PossessableRBManager rbManager = null;

    //now there is a list of possessingPlayers, for each, add a spring joint connected to a player ID
    public List<PlayerPossession> possessingPlayers = new List<PlayerPossession>();

    private MeshRenderer renderer;
    private Material defaultMaterial;
    private Outline outline;
    private bool IsPossessed => possessingPlayers.Count > 0;
    private void Start()
    {
        // PossessableRBManager is optional - but get a reference to it if it's there
        rbManager = GetComponent<PossessableRBManager>();
        renderer = GetComponentInChildren<MeshRenderer>();
        defaultMaterial = renderer.material;
        outline = gameObject.GetComponent<Outline>();
        distortionOrb = Instantiate(orbDistortionPrefab, transform);
        distortionOrb.transform.localScale = Vector3.zero;
        SetUnPossessedEffect();
    }


    // Return whether this Possessable is ready to be possessed
    public bool CanBePossessed()
    {
        // TODO: may want to be able to "lock" possessables, for some reason
        if(limitToOnePossession)
        {
            return !IsPossessed;
        }
        return true; // This allows any number of players to possess a given object
    }


    // Try to possess this object using the given player
    // Return whether that was successful
    public bool TryPossess(PlayerPossession player)
    {
        if (!CanBePossessed() || !player)
            return false;

        // Store player and have it listen for possession events
        possessingPlayers.Add(player);
        OnPossessionBegin.AddListener(player.OnPossessionBeginAction);
        OnPossessionEnd.AddListener(player.OnPossessionEndAction);


        OnPossessionBegin.Invoke(this, player);

        Debug.Log(player.gameObject.name + " started possessing " + this.gameObject.name, this);

        // Create the spring joint
        // TODO: Maybe the rbManager could just listen to OnPossessionBegin/End
        if (rbManager)
            rbManager.AttachSpringTo(player.GetPlayerRB(), player.playerID, player.PossessableAttachPoint);

        return true;
    }


    // Un-possess this object
    public void Eject(PlayerPossession player)
    {
        if (IsPossessed)
        {
            possessingPlayers.Remove(player);
            OnPossessionEnd.Invoke(this, player);

            OnPossessionBegin.RemoveListener(player.OnPossessionBeginAction);
            OnPossessionEnd.RemoveListener(player.OnPossessionEndAction);

            Debug.Log(player.gameObject.name + " stopped possessing " + this.gameObject.name, this);
        }

        if (rbManager)
            rbManager.DetachSpring(player.playerID);
    }

    private void OverrideMaterial(Material material) {
        if (material == null) {
            renderer.material = defaultMaterial;
            return;
        }
        renderer.material = material;
    }

    public void Haunt(PlayerPossession player)
    {
        OnHaunt.Invoke(this, player);
    }

    public Outline GetOutline()
    {
        return outline;
    }
    private void SetOutline(bool enabled, Color color, float thickness)
    {
        outline.enabled = enabled;
        outline.OutlineColor = color;
        outline.OutlineWidth = thickness;
    }
    public void SetPossessedEffect(Color playerColor, bool outlineEnabled, float outlineThickness, Material materialOverride)
    {
        SetOutline(outlineEnabled, playerColor, outlineThickness);
        OverrideMaterial(materialOverride);
    }

    public void SetPossessedEffect(Color playerColor)
    {
        SetOutline(true, playerColor, 6);

        distortionOrb.GetComponent<Renderer>().material.SetFloat("Strength", 0.0f);
        DG.Tweening.Sequence orbSequence = DOTween.Sequence();
        orbSequence.Insert(0.0f, distortionOrb.transform.DOScale(2.0f, 0.25f));
        orbSequence.Insert(0.0f, distortionOrb.GetComponent<Renderer>().material.DOFloat(1.0f, "Strength", 0.25f));
        orbSequence.Insert(0.25f, distortionOrb.transform.DOScale(0.0f, 0.4f));
    }

    public void SetUnPossessedEffect()
    {
        SetOutline(false, Color.black, 0);

        distortionOrb.GetComponent<Renderer>().material.SetFloat("Strength", 1.0f);
        DG.Tweening.Sequence orbSequence = DOTween.Sequence();
        orbSequence.Insert(0.0f, distortionOrb.transform.DOScale(2.0f, 0.25f));
        orbSequence.Insert(0.0f, distortionOrb.GetComponent<Renderer>().material.DOFloat(0.0f, "Strength", 0.25f));
        orbSequence.Insert(0.25f, distortionOrb.transform.DOScale(0.0f, 0.4f));
    }
}
