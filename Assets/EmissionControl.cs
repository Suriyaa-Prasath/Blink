using System.Collections;
using UnityEngine;

public class EmissionController : MonoBehaviour
{
    public Material[] materialsToToggle; // Array of materials with emission
    public Color emissionColor = Color.white; // Emission color
    public float delayBeforeEmission = 15f; // Delay before turning emission on
    public float emissionDuration = 3f; // How long emission stays on

    private bool isEmitting = false;

    void Start()
    {
        // Start the emission cycle
        StartCoroutine(EmissionCycle());
    }

    IEnumerator EmissionCycle()
    {
        while (true)
        {
            // Wait for delay before turning emission on
            yield return new WaitForSeconds(delayBeforeEmission);

            // Turn on emission
            SetEmission(true);

            // Keep the emission on for a certain duration
            yield return new WaitForSeconds(emissionDuration);

            // Turn off emission
            SetEmission(false);
        }
    }

    void SetEmission(bool turnOn)
    {
        foreach (Material mat in materialsToToggle)
        {
            if (turnOn)
            {
                // Enable the keyword for emission
                mat.EnableKeyword("_EMISSION");
                mat.SetColor("_EmissionColor", emissionColor);
            }
            else
            {
                // Disable the emission
                mat.DisableKeyword("_EMISSION");
            }
        }
    }
}
