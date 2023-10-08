using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    private PlayerInput playerInputComponent = null;
    private Rigidbody playerRigidbody;
    private const string MOVEMENT_INPUT_ID = "Move";
    private const string JUMP_INPUT_ID = "Jump";
    private Vector3 movementVector;
    private Vector3 newVelocity = new Vector3();
    public float speed = 2;
    public float jumpFloatSpeed = 5;
    private float jumpMultiplier = 1;

    private void Start() {
        Init();
    }

    private void Init() {
        playerInputComponent = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();

        playerInputComponent.onActionTriggered += Movement;
    }

    private void Movement(InputAction.CallbackContext context) {
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

    private void FixedUpdate() {
        if (playerRigidbody == null) {
            return;
        }
        
        newVelocity.x = movementVector.x * speed;
        newVelocity.y = playerRigidbody.velocity.y;
        newVelocity.z = movementVector.z * speed;
        if (playerInputComponent.actions[JUMP_INPUT_ID].IsPressed()) {
            newVelocity.y = jumpFloatSpeed;
        }
        playerRigidbody.velocity = newVelocity;
    }

    public void JumpMultiplier(Possessable possessable) {
        jumpMultiplier = possessable.rbManager.possessableRigidBody.mass;
    }

    public void ResetJumpMultiplier() {
        jumpMultiplier = 1;
    }
}