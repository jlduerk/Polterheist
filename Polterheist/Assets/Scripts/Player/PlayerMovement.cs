using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    private PlayerInput playerInputComponent = null;
    private Rigidbody playerRigidbody;
    private const string MOVEMENT_INPUT_ID = "Move";
    public float speed = 2;

    private void Start() {
        Init();
    }

    private void Init() {
        playerInputComponent = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();

        playerInputComponent.onActionTriggered += Movement;
    }

    private void Movement(InputAction.CallbackContext context) {
        if (context.action.name.Equals(MOVEMENT_INPUT_ID) && context.performed) {
            Vector2 inputDelta = context.ReadValue<Vector2>();
            Vector3 movementVector = new Vector3(inputDelta.x, 0, inputDelta.y);
            Debug.Log($"{name} {movementVector}");
            playerRigidbody.AddForce(movementVector * speed, ForceMode.Acceleration);
        }
    }
}