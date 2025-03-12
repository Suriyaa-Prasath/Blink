using UnityEngine;

public class SpriteTransparency : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [Range(0f, 1f)] public float transparency = 1f; // Slider in Inspector

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Ensure the material supports transparency
        if (spriteRenderer.material != null)
        {
            spriteRenderer.material.renderQueue = 3000; // Transparent queue
        }
    }

    void Update()
    {
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = transparency;
            spriteRenderer.color = color;
        }
    }
}
