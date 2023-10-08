using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Polterheist/TeamData", fileName = "New TeamData")]
public class TeamData : ScriptableObject {
    public enum Team {
        Red = 1,
        Blue = 2,
        Green = 3,
        Yellow = 4,
    }
    public string teamName = "team";
    public Team team;
    public Material teamMaterial;
    public Material teamMaterialHover;
    public EZone teamZone;
    public Color teamColor;
}
