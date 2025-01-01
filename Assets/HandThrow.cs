using UnityEngine;

public class HandThrow : MonoBehaviour
{
    [SerializeField] private GameObject bottlePrefab;  // Prefab of the bottle to throw
    [SerializeField] private Transform handTransform;  // Transform of the hand
    [SerializeField] private float throwForceMultiplier = 1f; // Multiplier to adjust the throw force
    private GameObject currentBottle;                 // Reference to the bottle being held
    private Rigidbody bottleRigidbody;               // Rigidbody of the bottle
    private Vector3 lastHandPosition;                // To track the hand's last position
    private Vector3 handVelocity;                    // Calculated velocity of the hand
    private bool isHoldingBottle = true;             // Whether the hand is currently holding the bottle

    private void Start()
    {
        
    }

    private void Update()
    {
        if (isHoldingBottle)
        {
            // Calculate hand velocity
            handVelocity = (handTransform.position - lastHandPosition) / Time.deltaTime;
            lastHandPosition = handTransform.position;

            // Update the bottle's position to follow the hand
            currentBottle.transform.position = handTransform.position;
        }
        else
        {
            // When not holding, reset the velocity tracker
            lastHandPosition = handTransform.position;
            handVelocity = Vector3.zero;
        }

        // Simulate release by checking hand release condition (e.g., velocity threshold or grip open)
        if (isHoldingBottle && ShouldReleaseBottle())
        {
            ReleaseBottle();
        }
    }

   
    private bool ShouldReleaseBottle()
    {
        // Define your logic for when to release the bottle
        // Example: Release when hand velocity exceeds a certain threshold
        return handVelocity.magnitude > 0.5f; // Adjust the threshold based on your needs
    }

    private void ReleaseBottle()
    {
        if (currentBottle != null && bottleRigidbody != null)
        {
            // Stop holding the bottle
            isHoldingBottle = false;

            // Detach the bottle
            bottleRigidbody.isKinematic = false;
            currentBottle.transform.SetParent(null);

            // Apply the velocity of the hand as the throw force
            bottleRigidbody.velocity = handVelocity * throwForceMultiplier;

            Debug.Log($"Bottle thrown with velocity: {bottleRigidbody.velocity}");

            
        }
    }
}
