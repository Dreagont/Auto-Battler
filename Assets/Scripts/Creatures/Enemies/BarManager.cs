using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarManager : MonoBehaviour
{
    public Image fillBar;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateBar(float currentHealth, float maxHealth)
    {
        fillBar.fillAmount = (float)currentHealth / (float)maxHealth;
    }
}
