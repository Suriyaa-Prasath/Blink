using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthBar : MonoBehaviour
{
    public Slider slider;
    public int maxHealth = 100;
    // Start is called before the first frame update
    void Start()
    {
        slider.maxValue = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
