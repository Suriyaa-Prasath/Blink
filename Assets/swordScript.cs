using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordScript : MonoBehaviour
{
    public SwordEnemyAI swordAI;
    // Start is called before the first frame update
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("slashed the mf");
            swordAI.slashPlayer();
        }
    }
}
