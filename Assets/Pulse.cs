using UnityEngine;

public class Pulse : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public SphereCollider sphereCollider;

    void OnCollisionEnter(Collision collision)
    {
        // Get the contact point
        ContactPoint contact = collision.contacts[0];

        // Calculate the distance from the sphere center to the contact point
        float distance = Vector3.Distance(sphereCollider.transform.position, contact.point);

        // Check if the contact point is within the sphere
        if (distance <= sphereCollider.radius)
        {
            // Create a new material with the custom shader
            Material material = new Material(Shader.Find("Custom/MaskShader"));

            // Create a 3D texture that represents the colliding part of the mesh
            int textureSize = 16; // adjust this value to change the resolution of the 3D texture
            Texture3D maskTexture = new Texture3D(textureSize, textureSize, textureSize, TextureFormat.R8, false);
            maskTexture.SetPixels(new Color[textureSize * textureSize * textureSize]);
            maskTexture.Apply();

            // Set the _MaskTex property of the material
            material.SetTexture("_MaskTex", maskTexture);

            // Set the material of the mesh renderer
            meshRenderer.material = material;
        }
    }
}