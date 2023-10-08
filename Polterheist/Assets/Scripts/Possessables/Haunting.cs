using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Haunting : MonoBehaviour {


    public float intensity = 20;

    public void OnHauntTable(Possessable possessable, PlayerPossession possessor)
    {
        float radius = 24;
        Collider[] hitColliders = Physics.OverlapSphere(possessable.transform.position, radius);
        foreach (var hitCollider in hitColliders)
        {
            
            if(LayerMask.LayerToName(hitCollider.gameObject.layer) == "Possessable")
            {
                Vector3 awayVec = (hitCollider.transform.position - possessable.transform.position).normalized;
                Vector3 forceVec = new Vector3(awayVec.x, 1, awayVec.z);
                hitCollider.attachedRigidbody.AddForce(forceVec * intensity);
                Debug.Log("adding explosioin force to " + hitCollider.gameObject.name);

            }
        }
    }

    public void OnHauntLamp(Possessable possessable, PlayerPossession possessor)
    {
    }

    public void SpinCrazy()
    {

    }

    public void MonkeBackflip()
    {

    }
}
