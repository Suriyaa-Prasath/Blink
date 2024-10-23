using UnityEngine;
using FMODUnity;

public class PlayerFootsteps : MonoBehaviour
{
    [SerializeField]
    private string footstepsEvent = "event:/Character/Player Footsteps"; // Change this to your FMOD event path

    private FMOD.Studio.EventInstance footstepsInstance;
    private CharacterController characterController; // Reference to CharacterController
    private bool isMoving = false;

    void Start()
    {
        // Get the CharacterController component attached to the player
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("CharacterController component not found on the player GameObject.");
        }

        // Create the FMOD instance for footsteps
        footstepsInstance = RuntimeManager.CreateInstance(footstepsEvent);
    }

    void Update()
    {
        // Check if the player is moving
        if (characterController != null)
        {
            // Check if the player is moving by checking the velocity
            if (characterController.velocity.magnitude > 0.1f && !isMoving)
            {
                // Start playing the footstep sound
                isMoving = true;
                footstepsInstance.start();
            }
            else if (characterController.velocity.magnitude <= 0.1f && isMoving)
            {
                // Stop the footstep sound
                isMoving = false;
                footstepsInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
        }
    }

    void OnDisable()
    {
        // Stop and release the footstep sound when the script is disabled
        if (isMoving)
        {
            footstepsInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            isMoving = false;
        }
        footstepsInstance.release();
    }
}
