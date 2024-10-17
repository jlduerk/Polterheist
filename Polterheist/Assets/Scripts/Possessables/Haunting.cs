using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;

public class Haunting : MonoBehaviour {
    public float pushBackIntensity = 50;
    public float defaultHauntCooldown = 2f;
    public string hauntSFX = "Haunt";
    public float HauntLaunchForce = 300;
    public float HauntEffectRadius = 3;
    
    public GameObject hauntVFXPrefab;
    private ParticleSystem hauntVFX;

    private Rigidbody possessableRidgidBody;
    private CoroutineHandle defualtHauntTimer;


    private void Start()
    {
        Init();
    }
    private void Init()
    {
        possessableRidgidBody = GetComponent<Rigidbody>();
    }
    
    public void HauntEffect() {
        if (hauntVFX) {
            hauntVFX.Play();
            return;
        }

        hauntVFX = Instantiate(hauntVFXPrefab, transform.position, Quaternion.identity, transform).GetComponent<ParticleSystem>();
        hauntVFX.Play();
    }

    public void OnHauntTable(Possessable possessable, PlayerPossession possessor)
    {
        float radius = 3;
        Collider[] hitColliders = Physics.OverlapSphere(possessable.transform.position, radius);
        foreach (var hitCollider in hitColliders)
        {
            
            if(LayerMask.LayerToName(hitCollider.gameObject.layer) == "Possessable")
            {
                Vector3 awayVec = (hitCollider.transform.position - possessable.transform.position).normalized;
                Vector3 forceVec = new Vector3(awayVec.x, 1, awayVec.z);
                hitCollider.attachedRigidbody.AddForce(forceVec * pushBackIntensity);
                //Debug.Log("adding explosioin force to " + hitCollider.gameObject.name);

            }
        }
    }

    public void OnDefaultHaunt(Possessable possessable, PlayerPossession possessor)
    {
        if (!defualtHauntTimer.IsRunning)
        {
            AudioManager.Instance.Play(hauntSFX);
            defualtHauntTimer = Timing.RunCoroutine(_defualtHauntTimer());
            //Debug.Log($"{possessable.name} HAUNTED");
            HauntEffect();

            Vector3 upForce = new Vector3(1, HauntLaunchForce, 1);
            possessableRidgidBody.AddForce(upForce);
            Collider[] hitColliders = Physics.OverlapSphere(possessable.transform.position, HauntEffectRadius);

            foreach (var hitCollider in hitColliders)
            {

                if (LayerMask.LayerToName(hitCollider.gameObject.layer) == "Possessable")
                {
                    Vector3 awayVec = (hitCollider.transform.position - possessable.transform.position).normalized;
                    Vector3 forceVec = new Vector3(awayVec.x, 1, awayVec.z);
                    hitCollider.attachedRigidbody.AddForce(forceVec * pushBackIntensity * possessableRidgidBody.mass);
                }
            }
        }
    }

    public void HauntFXOnly() {
        AudioManager.Instance.Play(hauntSFX);
        HauntEffect();
    }

    private IEnumerator<float> _defualtHauntTimer()
    {
        yield return Timing.WaitForSeconds(defaultHauntCooldown);
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