using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandPunch : MonoBehaviour
{
    [SerializeField] private InputActionReference leftGripAction; // Reference to XRI LeftHand GripButton
    private bool isLeftGripPressed = false; // Tracks the state of the left grip button

    private void OnEnable()
    {
        // Subscribe to the input action
        if (leftGripAction != null)
        {
            leftGripAction.action.performed += OnLeftGripPerformed;
            leftGripAction.action.canceled += OnLeftGripCanceled;
            leftGripAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        // Unsubscribe from the input action
        if (leftGripAction != null)
        {
            leftGripAction.action.performed -= OnLeftGripPerformed;
            leftGripAction.action.canceled -= OnLeftGripCanceled;
            leftGripAction.action.Disable();
        }
    }

    private void OnLeftGripPerformed(InputAction.CallbackContext context)
    {
        isLeftGripPressed = true; // Grip is pressed
        Debug.Log("Left Grip Pressed!");
    }

    private void OnLeftGripCanceled(InputAction.CallbackContext context)
    {
        isLeftGripPressed = false; // Grip is released
        Debug.Log("Left Grip Released!");
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the left grip is pressed and the collider is tagged as "Enemy"
        if (isLeftGripPressed && other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.PunchDie();
                Debug.Log("Enemy defeated!");
            }
        }
    }
}
