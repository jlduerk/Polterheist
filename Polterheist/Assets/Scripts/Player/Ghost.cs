using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour {
    public Rigidbody rb;    
    public Transform target;
    public Renderer renderer;
    private Material material;
    public Renderer hatRenderer;
    private Material hatMaterial;
    private const string SHADER_SLANTDIR_REFERENCE = "_SlantDir";
    public float wiggleSlantMultiplier = 0.5f;
    private const string SHADER_WIGGLE_SPEED_REFERENCE = "_WiggleSpeed";

    private static readonly int wiggleSlant = Shader.PropertyToID(SHADER_SLANTDIR_REFERENCE);
    private static readonly int wiggleSpeed = Shader.PropertyToID(SHADER_WIGGLE_SPEED_REFERENCE);
    public LayerMask groundLayer;
    public ParticleSystem dropShadow;
    public Vector3 dropShadowOffset = new Vector3(0, 0.25f, 0);
    public float dropShadowDistanceScalar = 2;
    private const int MAX_DROPSHADOW_DISTANCE = 100;
    
    void Start() {
        material = renderer.material;
        if(hatRenderer != null)
        {
            hatMaterial = hatRenderer.material;
        }
    }

    void Update() {
        if(new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude > .2 ) {
            material.SetFloat(wiggleSpeed, 9);
            if (hatMaterial != null)
            {
                hatMaterial.SetFloat(wiggleSpeed, 9);
            }
        }
        else {
            material.SetFloat(wiggleSpeed, 3);
            if (hatMaterial != null)
            {
                hatMaterial.SetFloat(wiggleSpeed, 3);
            }
        }
        material.SetVector(wiggleSlant, rb.velocity * wiggleSlantMultiplier);

        DropShadow();
    }

    private void DropShadow() {
        if (!dropShadow || !dropShadow.gameObject.activeInHierarchy) {
            return;
        }
        
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, MAX_DROPSHADOW_DISTANCE, groundLayer)) {
            dropShadow.transform.position = hit.point + dropShadowOffset;
            float distance = transform.position.y - hit.point.y;
            dropShadow.transform.localScale = Vector3.one * (distance * dropShadowDistanceScalar);
        }
    }
}