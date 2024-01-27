using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Polterheist/HatData", fileName = "New Hat")]
public class HatData : ScriptableObject {
    public string name;
    public GameObject hatPrefab;
}