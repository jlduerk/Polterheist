using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatEquip : MonoBehaviour
{
    public HatData hatData;
    private GameObject hatPrefabInstance = null;

    private void Start()
    {
        SetHatData(hatData);
    }

    public void SetHatData(HatData newHatData)
    {
        hatData = newHatData;

        if (hatPrefabInstance)
        {
            Destroy(hatPrefabInstance);
        }

        if (hatData)
        {
            // Create the new hat prefab geometry, and set up material + collision
            hatPrefabInstance = Instantiate(hatData.hatPrefab, transform);
            hatPrefabInstance.GetComponent<MeshRenderer>().material.SetFloat(Shader.PropertyToID("_WiggleSpeed"), 0);
            GetComponent<MeshCollider>().sharedMesh = hatPrefabInstance.GetComponent<MeshFilter>().mesh;
        }
    }

    public void EquipHat(Possessable possessable, PlayerPossession player)
    {
        player.DropHat();
        player.SetHat(hatData);
        Destroy(gameObject);
    }
}
