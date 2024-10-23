using UnityEngine;
using FMODUnity;

public class PlayerMovement : MonoBehaviour
{
    public EventReference footstepSound; // Reference to FMOD footstep sound
    private Vector3 lastPosition;
    private bool isMoving = false;
    public float footstepInterval = 0.5f;  // Time interval between each footstep sound
    private float footstepTimer = 0f;

    void Start()
    {
        // Store the initial position of the player
        lastPosition = transform.position;
      
    }

    void Update()
    {
        // Check if the player has moved
        if (Vector3.Distance(lastPosition, transform.position) > 0.01f) // You can adjust the threshold
        {
            isMoving = true;
            lastPosition = transform.position;
            footstepTimer += Time.deltaTime;
            if (footstepTimer >= footstepInterval)
            {
                // Play footstep sound using FMOD
                FMODUnity.RuntimeManager.PlayOneShot(footstepSound, transform.position);
                footstepTimer = 0f;  // Reset the timer
            }


        }
        else
        {
            isMoving = false;

        }
    }
}
