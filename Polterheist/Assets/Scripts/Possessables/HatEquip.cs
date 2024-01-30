using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatEquip : MonoBehaviour
{

    public HatData hatData;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnablePhysics(Possessable possessable, PlayerPossession possessor)
    {
        GetComponent<Rigidbody>().isKinematic = false;
    }

    public void EquipHat(Possessable possessable, PlayerPossession possessor)
    {
        
    }
}
