using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private Zone[] allZones;
    private List<Zone> AZones;
    private List<Zone> BZones;
    private GameManager gameManager;

    public int ScoreA;
    public int ScoreB;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;

        // Initialize zone lists
        AZones = new List<Zone>();
        BZones = new List<Zone>();
        GetZones();
    }

    void FixedUpdate()
    {
        if (gameManager.GameInProgress) // Only do scoring when game has started
        {
            ScoreA = CalculateScore(EZone.ZoneA);
            ScoreB = CalculateScore(EZone.ZoneB);
        }
    }

    // Calculates a zone's score
    private int CalculateScore(EZone zone)
    {
        int score = 0;

        List<Zone> zoneList;
        if (zone == EZone.ZoneA) zoneList = AZones;
        else if (zone == EZone.ZoneB) zoneList = BZones;
        else return -1;

        // Early out if no zones
        if (zoneList.Count == 0) return 0;

        // Reset every possessable to not have been scored yet
        foreach(Zone z in zoneList)
        {
            // Early out if no colliders
            if (z.possessableColliders.Length == 0 || z.possessableColliders == null) break;

            int i = 0;
            while (z.possessableColliders[i] != null)
            {
                Possessable possessable = z.possessableColliders[i].GetComponentInParent<Possessable>();
                if (possessable != null)
                {
                    possessable.WasScored = false;
                }

                i++;
            }
        }

        foreach (Zone z in zoneList)
        {
            score += z.GetScore();
        }

        return score;
    }

    // Populates zone lists 
    private void GetZones()
    {
        allZones = FindObjectsByType<Zone>(FindObjectsSortMode.None);

        // Early out if no zones
        if (allZones.Length == 0 || allZones == null) return;

        foreach (Zone zone in allZones)
        {
            switch (zone.thisZone)
            {
                case EZone.ZoneA:
                    {
                        AZones.Add(zone);
                        break;
                    }
                case EZone.ZoneB:
                    {
                        BZones.Add(zone);
                        break;
                    }
                default: break;

            }
        }
    }
}