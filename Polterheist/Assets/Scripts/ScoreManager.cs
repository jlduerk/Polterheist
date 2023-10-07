using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private List<Zone> AZones;
    private List<Zone> BZones;

    public int ScoreA;
    public int ScoreB;

    // Start is called before the first frame update
    void Start()
    {
        GetZones();
    }

    void FixedUpdate()
    {
        ScoreA = CalculateScore(EZone.ZoneA);
        ScoreB = CalculateScore(EZone.ZoneB);
    }

    // Calculates a zone's score
    private int CalculateScore(EZone zone)
    {
        int score = 0;

        List<Zone> zoneList;
        if (zone == EZone.ZoneA) zoneList = AZones;
        else if (zone == EZone.ZoneB) zoneList = BZones;
        else return -1;

        // Reset every possessable to not have been scored yet
        foreach(Zone z in zoneList)
        {
            foreach (Collider collider in z.possessableColliders)
            {
                Possessable possessable = collider.GetComponentInParent<Possessable>();
                if (possessable != null)
                {
                    possessable.WasScored = false;
                }
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
        Zone[] allZones = FindObjectsByType<Zone>(FindObjectsSortMode.None);
        foreach(Zone zone in allZones)
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
