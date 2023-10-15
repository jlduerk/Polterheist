using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public Rigidbody rb;    
    public Transform target;
    public Renderer renderer;
    private Material material;
    private const string SHADER_SLANTDIR_REFERENCE = "_SlantDir";
    public float wiggleSlantMultiplier = 0.5f;
    private const string SHADER_WIGGLE_SPEED_REFERENCE = "_WiggleSpeed";
    
    private static readonly int wiggleSlant = Shader.PropertyToID(SHADER_SLANTDIR_REFERENCE);
    private static readonly int wiggleSpeed = Shader.PropertyToID(SHADER_WIGGLE_SPEED_REFERENCE);


    // Start is called before the first frame update
    void Start()
    {
        material = renderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        if(new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude > .2 )
        {
            material.SetFloat(wiggleSpeed, 9);
        }
        else
        {
            material.SetFloat(wiggleSpeed, 3);
        }
        material.SetVector(wiggleSlant, rb.velocity * wiggleSlantMultiplier);
    }
}
