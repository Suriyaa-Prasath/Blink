using UnityEngine;
using System.Collections;

public class OutlineController : MonoBehaviour
{
    public float activeDuration = 3f;
    public float inactiveDuration = 3f;
    private Outline[] outlineObjects;

    // Bright color to interact with the Bloom effect
    public Color glowColor = new Color(5f, 5f, 0f, 1f); // You can adjust this to your preference
    public Color normalColor = Color.white;

    void Start()
    {
        outlineObjects = FindObjectsOfType<Outline>();
        StartCoroutine(ToggleOutline());
    }

    IEnumerator ToggleOutline()
    {
        while (true)
        {
            // Enable the glowing outline on all objects
            SetOutlineState(true, glowColor);

            // Wait for the active duration
            yield return new WaitForSeconds(activeDuration);

            // Disable the outline or set to a normal outline color
            SetOutlineState(false, normalColor);

            // Wait for the inactive duration
            yield return new WaitForSeconds(inactiveDuration);
        }
    }

    void SetOutlineState(bool state, Color outlineColor)
    {
        foreach (Outline outline in outlineObjects)
        {
            outline.enabled = state;
            outline.OutlineColor = outlineColor;
        }
    }
}