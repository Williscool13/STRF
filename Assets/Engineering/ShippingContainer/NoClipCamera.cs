using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NoClipCamera : MonoBehaviour
{
    [SerializeField] PlayerInput input;
    [SerializeField] float moveSpeed = 5f;
    Camera cam;
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mov = input.actions["Move"].ReadValue<Vector2>();

        // Convert input values to a direction based on the camera's forward and right vectors
        Vector3 inputDirection = GetCameraRelativeInputDirection(mov.x, mov.y);

        // Move the player based on the input direction
        MovePlayer(inputDirection);
    }

    Vector3 GetCameraRelativeInputDirection(float horizontalInput, float verticalInput) {
        // Get the camera's forward and right vectors
        Vector3 forward = cam.transform.forward;
        Vector3 right = cam.transform.right;

        // Project the input direction onto the camera's horizontal plane
        forward.y = 0f;
        right.y = 0f;

        // Normalize vectors to get the input direction
        Vector3 inputDirection = (forward.normalized * verticalInput + right.normalized * horizontalInput).normalized;

        return inputDirection;
    }

    void MovePlayer(Vector3 inputDirection) {
        // Move the player based on the input direction
        transform.Translate(inputDirection * moveSpeed * Time.deltaTime, Space.World);
    }
}
