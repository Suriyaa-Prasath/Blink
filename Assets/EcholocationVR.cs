using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class EcholocationVR : MonoBehaviour
{
    public float waveRadius = 5f;
    public float lightDuration = 1f;
    public LayerMask detectableObjects;
    private InputDevice controller;

    void Start()
    {
        // Access the VR controller (assuming right hand here)
        var devices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, devices);
        if (devices.Count > 0)
            controller = devices[0];
    }

    void Update()
    {
        if (controller.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerPressed) && triggerPressed)
        {
            EmitSoundWave();
        }
    }

    void EmitSoundWave()
    {
        Collider[] hitObjects = Physics.OverlapSphere(transform.position, waveRadius, detectableObjects);

        foreach (var obj in hitObjects)
        {
            StartCoroutine(TemporarilyLightObject(obj));
        }
    }

    System.Collections.IEnumerator TemporarilyLightObject(Collider obj)
    {
        Renderer objRenderer = obj.GetComponent<Renderer>();
        if (objRenderer != null)
        {
            Material originalMaterial = objRenderer.material;
            objRenderer.material.SetColor("_EmissionColor", Color.white);
            yield return new WaitForSeconds(lightDuration);
            objRenderer.material.SetColor("_EmissionColor", Color.black); // Reset to dark
        }
    }
}
