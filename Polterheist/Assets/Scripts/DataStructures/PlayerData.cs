using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData {
    public HatData hatData;
    public TeamData teamData;
    public int playerInputIndex;

    public void PickupHat(HatData newHatData) {
        hatData = newHatData;
    }

    public void DropHat() {
        hatData = null;
    }
}