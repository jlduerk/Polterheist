using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    public Material invisibleMaterial;
    public GameObject spawnEffectPrefab;
    private ParticleSystem spawnEffect;
    private PlayerInput playerInputComponent = null;
    private Rigidbody playerRigidbody;
    private const string MOVEMENT_INPUT_ID = "Move";
    private const string JUMP_INPUT_ID = "Jump";
    private Vector3 movementVector;
    private Vector3 newVelocity = new Vector3();
    public float speed = 2;
    public float dashSpeedMultiplier = 5;
    public float speedMultiplier = 1;
    private float OGspeed;
    private float DEFAULT_SPEED_MULTIPLIER = 1.0f;
    public float jumpFloatSpeed = 5;
    private float jumpMultiplier = 1;
    private bool hasJoined;
    private bool movementEnabled = true;
    private const float DASH_SPEED = 50;
    private bool isDashing = false;
    public float dashDuration = 0.5f;

    private void Start() {
        Init();
    }

    public void Init() {
        playerInputComponent = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();

        playerInputComponent.onActionTriggered += Movement;
        OGspeed = speed;
    }

    private void Movement(InputAction.CallbackContext context) {
        RegisterPlayer();
        if (!GameManager.Instance.GameInProgress || !movementEnabled || isDashing) {
            return;
        }
        switch (context.action.name) {
            case MOVEMENT_INPUT_ID:
                if (context.performed) {
                    Vector2 inputDelta = context.ReadValue<Vector2>();
                    movementVector = new Vector3(inputDelta.x, 0, inputDelta.y);
                }
                else if (context.canceled) {
                    movementVector = Vector3.zero;
                }
                break;
            default: 
                break;
        }
    }

    private void Update() {
        if (!movementEnabled) {
            return;
        }
        if (movementVector != Vector3.zero && !GameManager.Instance.Paused) {
            transform.rotation = Quaternion.LookRotation(-movementVector);
        }
    }

    private void FixedUpdate() {

        if (playerRigidbody == null) {
            return;
        }
        
        newVelocity.x = movementVector.x * speed * speedMultiplier;
        newVelocity.y = playerRigidbody.velocity.y;
        newVelocity.z = movementVector.z * speed * speedMultiplier;
        if (playerInputComponent.actions[JUMP_INPUT_ID].IsPressed()) {
            RegisterPlayer();
            newVelocity.y = jumpFloatSpeed;
        }
        if (!GameManager.Instance.GameInProgress || !movementEnabled) {
            return;
        }
        playerRigidbody.velocity = newVelocity;
    }

    public void JumpMultiplier(Possessable possessable) {
        jumpMultiplier = possessable.rbManager.possessableRigidBody.mass;
    }

    public void ResetJumpMultiplier() {
        jumpMultiplier = 1;
    }

    private void RegisterPlayer() {
        if (hasJoined) {
            return;
        }

        hasJoined = true;
        GameManager.Instance.RegisterPlayer(playerInputComponent);
        SpawnEffect(transform);
    }
    
    public void SpawnEffect(Transform spawnTransform) {
        if (!spawnEffectPrefab) {
            Debug.LogError($"No spawnEffectPrefab assigned!");
            return;
        }
        if (spawnEffect) {
            spawnEffect.Play();
            return;
        }

        spawnEffect = Instantiate(spawnEffectPrefab, spawnTransform.position, Quaternion.identity, spawnTransform).GetComponent<ParticleSystem>();
        spawnEffect.Play();
    }

    public void TogglePlayerMovement(bool enable) {
        movementEnabled = enable;
    }

    public void Dash() {
        // Crank up movement speed multiplier, and override movement direction
        if (isDashing)
            return;

        isDashing = true;
        speedMultiplier = dashSpeedMultiplier;
        movementVector = -transform.forward;
        DOTween.To(() => speedMultiplier, x => speedMultiplier = x, DEFAULT_SPEED_MULTIPLIER, dashDuration).OnComplete(EndDash);
    }

    public void EndDash() {
        isDashing = false;
        movementVector = Vector3.zero;
    }
}