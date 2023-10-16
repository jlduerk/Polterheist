using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class ScoreManager : MonoBehaviour {
    private GameManager gameManager;

    private Zone[] allZones;
    private List<Zone> AZones;
    private List<Zone> BZones;

    [Header("Background Attributes")] 
    public float duration;
    public Ease easeType = Ease.Linear;
    public ParticleSystem backgroundParticle;
    private ParticleSystemRenderer renderer;
    private Material particleMaterial;
    public Color defaultColor;
    public Color blueTeamWinningColor;
    public Color redTeamWinningColor;
    private const float SATURATION_MAGNITUDE = 1;

    public int ScoreA;
    public int ScoreB;
    private TeamData.Team previousWinningTeam;
    
    // Start is called before the first frame update
    void Start() {
        gameManager = GameManager.Instance;

        Init();
    }

    public void Init() {
        // Initialize zone lists
        AZones = new List<Zone>();
        BZones = new List<Zone>();
        GetZones();

        if (backgroundParticle == null) {
            Debug.LogError($"WinningTeamBackground ParticleSystem not assigned to ScoreManager!", gameObject);
            return;
        }
        backgroundParticle.Play();
        particleMaterial = backgroundParticle.GetComponent<ParticleSystemRenderer>().material;
        particleMaterial.color = defaultColor;
    }

    void FixedUpdate() {
        if (gameManager.GameInProgress) {// Only do scoring when game has started
            ScoreA = CalculateScore(EZone.ZoneA);
            ScoreB = CalculateScore(EZone.ZoneB);
            int scoreDifference = Mathf.Abs(ScoreA - ScoreB);
            TeamWinningColorHelper(scoreDifference);
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

    private void TeamWinningColorHelper(int scoreDifference) {
        //team (color)
        TeamData.Team currentWinningTeam;
        if (ScoreA == ScoreB) {
            currentWinningTeam = TeamData.Team.White;
        }
        else if (ScoreA > ScoreB) {
            currentWinningTeam = TeamData.Team.Blue;
        }
        else {
            currentWinningTeam = TeamData.Team.Red;
        }
        if (previousWinningTeam != currentWinningTeam) {
            Color winningTeamColor = defaultColor;
            switch (currentWinningTeam) {
                case TeamData.Team.Blue:
                    winningTeamColor = blueTeamWinningColor;
                    break;
                case TeamData.Team.Red:
                    winningTeamColor = redTeamWinningColor;
                    break;
                case TeamData.Team.White:
                    winningTeamColor = defaultColor;
                    break;
            }
            
            particleMaterial.DOColor(winningTeamColor, duration).SetEase(easeType);
        }
        previousWinningTeam = currentWinningTeam;
    }
}