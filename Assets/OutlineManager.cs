using UnityEngine;

public class OutlineManager : MonoBehaviour
{
    public static OutlineManager Instance { get; private set; }

    public Material outlineMaterial;
    public float interval = 15f;
    public float outlineDuration = 3f;

    private Material[] originalMaterials;
    private Renderer[] renderers;

    private void Awake()
    {
        // Implement Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        renderers = FindObjectsOfType<Renderer>();
        originalMaterials = new Material[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].material;
        }

        InvokeRepeating("ApplyOutline", interval, interval);
    }

    private void ApplyOutline()
    {
        foreach (var renderer in renderers)
        {
            renderer.material = outlineMaterial;
        }
        Invoke("RemoveOutline", outlineDuration);
    }

    private void RemoveOutline()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = originalMaterials[i];
        }
    }
}
