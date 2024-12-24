using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HapticInteractable : MonoBehaviour
{
    [Range(0f, 1f)]
    public float intensity;
    public float duration;

    void Start()
    {
        XRBaseInteractable interactable = GetComponent<XRBaseInteractable>();
        interactable.activated.AddListener(TriggerHaptic);

    }
    
    public void TriggerHaptic(BaseInteractionEventArgs eventArgs)
    {
        if (eventArgs.interactorObject is XRBaseControllerInteractor interactable)
        {
            TriggerHaptic(interactable.xrController);
        }
    }
    public void TriggerHaptic(XRBaseController controller)
    {
        if(intensity>0)
        {
            controller.SendHapticImpulse(intensity, duration);
        }
    }
    
}
