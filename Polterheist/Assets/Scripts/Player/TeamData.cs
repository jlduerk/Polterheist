using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Polterheist/TeamData", fileName = "New TeamData")]
public class TeamData : ScriptableObject {
    public enum Team {
        White = 0,
        Red = 1,
        Blue = 2,
        Green = 3,
        Yellow = 4,
    }
    public string teamName = "team";
    public Team team;
    public Material teamMaterial;
    public Material teamMaterialHover;
    public Material teamMaterialPossess;
    public Material portalMaterial;
    public EZone teamZone;
    public Color teamColor;
    [ColorUsageAttribute(true, true)] public Color playerColor;
}
