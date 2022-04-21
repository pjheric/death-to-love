using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class HealthUI : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private Image[] healthTicks;
    [SerializeField] private float maxHealth = 30; //Delete later, here for testing only
    [SerializeField] private FloatAsset health; 
    [SerializeField] private float lerpSpeed = 3f;

    private void Start()
    {
        health.Value = maxHealth; 
    }
    private void Update()
    {
        if(health.Value > maxHealth)
        {
            health.Value = maxHealth; //Prevents overhealing 
        }
        lerpSpeed *= Time.deltaTime;
        FillBar();
    }
    private void FillBar()
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, (health.Value / maxHealth), lerpSpeed);
        for(int i = 0; i < healthTicks.Length; i++)
        {
            healthTicks[i].enabled = !DisplayHealthPoint(health.Value, i); 
        }
    }
    bool DisplayHealthPoint(float _health, int pointNumber)
    {
        return ((pointNumber * healthTicks.Length) >= _health);
    }
}
