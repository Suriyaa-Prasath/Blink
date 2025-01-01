using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HapticWallTrigger : MonoBehaviour
{
    public XRBaseController controller; // Assign the controller (left or right hand) in the Inspector
    public float hapticIntensity = 0.5f; // Adjust the intensity of the haptics (0.0 to 1.0)
    public float hapticDuration = 0.1f; // Duration of each haptic pulse in seconds

    private bool isInsideTrigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            isInsideTrigger = true;
            StartCoroutine(ContinuousHapticFeedback());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            isInsideTrigger = false;
        }
    }

    private System.Collections.IEnumerator ContinuousHapticFeedback()
    {
        while (isInsideTrigger)
        {
            if (controller != null)
            {
                controller.SendHapticImpulse(hapticIntensity, hapticDuration);
            }
            else
            {
                Debug.LogWarning("Controller not assigned for haptic feedback.");
                yield break;
            }

            yield return new WaitForSeconds(hapticDuration);
        }
    }
}
