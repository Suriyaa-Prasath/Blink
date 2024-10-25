using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OutlineController : MonoBehaviour
{
    public float activeDuration = 3f;
    public float inactiveDuration = 5f;
    private Outline[] outlineObjects;

    // Bright color to interact with the Bloom effect
    public Color glowColor = new Color(5f, 5f, 0f, 1f); // You can adjust this to your preference
    public Color normalColor = Color.white;

    // List to store objects currently in the player's hand
    private HashSet<Outline> heldObjects = new HashSet<Outline>();

    void Start()
    {
        outlineObjects = FindObjectsOfType<Outline>();
        StartCoroutine(ToggleOutline());
    }

    IEnumerator ToggleOutline()
    {
        while (true)
        {
            // Enable the glowing outline on all objects not held
            SetOutlineState(true, glowColor);

            // Wait for the active duration
            yield return new WaitForSeconds(activeDuration);

            // Disable the outline or set to a normal outline color for non-held objects
            SetOutlineState(false, normalColor);

            // Wait for the inactive duration
            yield return new WaitForSeconds(inactiveDuration);
        }
    }

    void SetOutlineState(bool state, Color outlineColor)
    {
        foreach (Outline outline in outlineObjects)
        {
            // Skip objects currently held by the player
            if (heldObjects.Contains(outline))
            {
                outline.enabled = true;
                outline.OutlineColor = glowColor;
            }
            else
            {
                outline.enabled = state;
                outline.OutlineColor = outlineColor;
            }
        }
    }

    // Call this method when an object is picked up
    public void OnPickUp(Outline outlineObject)
    {
        if (!heldObjects.Contains(outlineObject))
        {
            heldObjects.Add(outlineObject);
            outlineObject.enabled = true;
            outlineObject.OutlineColor = glowColor;
        }
    }

    // Call this method when an object is dropped
    public void OnDrop(Outline outlineObject)
    {
        if (heldObjects.Contains(outlineObject))
        {
            heldObjects.Remove(outlineObject);
        }
    }
}
