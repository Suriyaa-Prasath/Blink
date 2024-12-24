using UnityEngine;
using FMODUnity;

public class VRListenerFollow : MonoBehaviour
{
    public Transform playerHead;

    void Update()
    {
        if (playerHead != null)
        {
            transform.position = playerHead.position;
            transform.rotation = playerHead.rotation;
        }
    }
}
