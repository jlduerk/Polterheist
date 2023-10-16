using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Polterheist/PossessableSet", fileName = "New PossessableSet")]
public class PossessableSet : ScriptableObject {
    public Possessable[] possessables;
}
