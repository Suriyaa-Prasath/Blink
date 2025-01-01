using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;

public class HandPunch : MonoBehaviour
{
    [SerializeField] private InputActionReference leftGripAction; // Reference to XRI LeftHand GripButton
    [SerializeField] private InputActionReference aButtonAction; // Reference to XRI "A" Button
    [SerializeField] private Canvas targetCanvas; // Canvas to toggle on/off
    public EventReference handpunch;

    private bool isLeftGripPressed = false; // Tracks the state of the left grip button
    private bool isCanvasVisible = false;  // Tracks the visibility of the canvas

    private Vector3 lastPosition;          // For manual speed calculation
    private float currentSpeed;            // Calculated speed of the hand
    [SerializeField] private float minPunchSpeed = 2.0f; // Minimum speed for a valid punch

    private void Start()
    {
        lastPosition = transform.position; // Initialize the last position
    }

    private void Update()
    {
        // Calculate speed manually as the distance moved per frame
        currentSpeed = (transform.position - lastPosition).magnitude / Time.deltaTime;
        lastPosition = transform.position; // Update last position

        // Optional: Debug the calculated speed
        //Debug.Log($"Current Hand Speed: {currentSpeed}");
    }

    private void OnEnable()
    {
        // Subscribe to the input actions
        if (leftGripAction != null)
        {
            leftGripAction.action.performed += OnLeftGripPerformed;
            leftGripAction.action.canceled += OnLeftGripCanceled;
            leftGripAction.action.Enable();
        }

        if (aButtonAction != null)
        {
            aButtonAction.action.performed += OnAButtonPerformed;
            aButtonAction.action.canceled += OnAButtonCanceled;
            aButtonAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        // Unsubscribe from the input actions
        if (leftGripAction != null)
        {
            leftGripAction.action.performed -= OnLeftGripPerformed;
            leftGripAction.action.canceled -= OnLeftGripCanceled;
            leftGripAction.action.Disable();
        }

        if (aButtonAction != null)
        {
            aButtonAction.action.performed -= OnAButtonPerformed;
            aButtonAction.action.canceled -= OnAButtonCanceled;
            aButtonAction.action.Disable();
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

    private void OnAButtonPerformed(InputAction.CallbackContext context)
    {
        // Toggle the canvas visibility on
        if (!isCanvasVisible)
        {
            isCanvasVisible = true;
            targetCanvas.gameObject.SetActive(true);
            Debug.Log("Canvas turned ON");
        }
    }

    private void OnAButtonCanceled(InputAction.CallbackContext context)
    {
        // Toggle the canvas visibility off
        if (isCanvasVisible)
        {
            isCanvasVisible = false;
            targetCanvas.gameObject.SetActive(false);
            Debug.Log("Canvas turned OFF");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider is tagged as "Enemy"
        if (other.CompareTag("Enemy"))
        {
            Debug.Log($"Collision detected with Enemy: {other.name}");

            if (isLeftGripPressed)
            {
                Debug.Log("Left grip is pressed!");

                if (currentSpeed >= minPunchSpeed)
                {
                    Debug.Log("Punch landed with sufficient speed!");

                    // Play sound and apply punch effect
                    FMODUnity.RuntimeManager.PlayOneShot(handpunch, transform.position);

                    Enemy enemy = other.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.PunchDie();
                        Debug.Log("Enemy defeated!");
                    }
                    else
                    {
                        Debug.LogWarning("Enemy script is missing on the collided object!");
                    }
                }
                else
                {
                    Debug.Log("Punch missed! Speed too low.");
                }
            }
            else
            {
                Debug.LogWarning("Left grip not pressed. Punch ignored.");
            }
        }
        
    }
}
