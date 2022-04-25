// Contributor(s): Eric Park, Nathan More
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class HealthUI : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private Image[] healthTicks;
    [SerializeField] private FloatAsset maxHealth; // Max health stored in float asset
    [SerializeField] private FloatAsset health;
    [SerializeField] private float lerpSpeed = 3f;

    private float healthBarStep; // Determines how many health points each tick represents

    private void Start()
    { 
        healthBarStep = maxHealth.Value / healthTicks.Length; // Sets healthBarStep based on max health and num of ticks
    }
    private void Update()
    {
        if(health.Value > maxHealth.Value)
        {
            health.Value = maxHealth.Value; //Prevents overhealing 
        }
        lerpSpeed *= Time.deltaTime;
        FillBar();
    }
    private void FillBar()
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, (health.Value / maxHealth.Value), lerpSpeed);

        for(int i = 0; i < healthTicks.Length; i++)
        {
            healthTicks[i].enabled = !DisplayHealthPoint(health.Value, i); 
        }
    }
    bool DisplayHealthPoint(float health, int pointNumber)
    {
        return ((pointNumber * healthBarStep) >= health);
    }
}
