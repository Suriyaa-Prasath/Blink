using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OutlineController : MonoBehaviour
{
    public float activeDuration = 3f;
    public float inactiveDuration = 3f;
    private List<Outline> outlineObjects = new List<Outline>();

    public Color glowColor = new Color(5f, 5f, 0f, 1f);
    public Color normalColor = Color.white;

    void Start()
    {
        RefreshOutlineList(); // Initialize the list with all available Outline components
        StartCoroutine(ToggleOutline());
    }

    IEnumerator ToggleOutline()
    {
        while (true)
        {
            // Refresh the list to remove destroyed objects before toggling
            RefreshOutlineList();

            SetOutlineState(true, glowColor);
            yield return new WaitForSeconds(activeDuration);

            SetOutlineState(false, normalColor);
            yield return new WaitForSeconds(inactiveDuration);
        }
    }

    void SetOutlineState(bool state, Color outlineColor)
    {
        // Remove destroyed objects before applying the state change
        outlineObjects.RemoveAll(item => item == null);

        foreach (Outline outline in outlineObjects)
        {
            if (outline != null) // Additional safety check
            {
                outline.enabled = state;
                outline.OutlineColor = outlineColor;
            }
        }
    }

    void RefreshOutlineList()
    {
        // Get all active Outline components in the scene
        outlineObjects.Clear();
        outlineObjects.AddRange(FindObjectsOfType<Outline>());
    }
}
