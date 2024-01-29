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
    private Vector3 movementVector;
    private Vector3 newVelocity = new Vector3();
    
    private const string JUMP_INPUT_ID = "Jump";
    private const string MOVEMENT_INPUT_ID = "Move";

    public float speed = 2;
    public float speedMultiplier = 1;
    public float jumpFloatSpeed = 5;
    private float jumpMultiplier = 1;
    public float dashDuration = 0.5f;

    private const float DASH_SPEED_MULTIPLIER = 5;
    private const float DEFAULT_SPEED_MULTIPLIER = 1;
    
    private bool movementEnabled = false;
    private bool isDashing = false;
    private bool isReady;

    private void Start() {
        Init();
    }

    public void Init() {
        playerInputComponent = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();

        playerInputComponent.onActionTriggered += Movement;
    }

    private void Movement(InputAction.CallbackContext context) {
        if (!movementEnabled || isDashing) {
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
        if (movementVector != Vector3.zero && !Settings.IsPaused) {
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
            SpawnGhost();
            newVelocity.y = jumpFloatSpeed;
        }
        if (!movementEnabled) {
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

    private void SpawnGhost() {
        if (isReady) {
            return;
        }

        TogglePlayerMovement(true);
        PersistentPlayersManager.Instance.currentLevelData.numPlayersReady++;

        playerInputComponent.gameObject.transform.position = PersistentPlayersManager.Instance.currentLevelData.spawnPoints[playerInputComponent.playerIndex].transform.position;
        isReady = true;
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
        speedMultiplier = DASH_SPEED_MULTIPLIER;
        movementVector = -transform.forward;
        DOTween.To(
            () => speedMultiplier,
            x => speedMultiplier = x,
            DEFAULT_SPEED_MULTIPLIER,
            dashDuration
        ).OnComplete(EndDash);
    }

    public void EndDash() {
        isDashing = false;
        movementVector = Vector3.zero;
    }
}