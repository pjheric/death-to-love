using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class HealthUI : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private Image[] healthTicks;
    [SerializeField] private float health, maxHealth = 100; //Delete later, here for testing only
    [SerializeField] private float lerpSpeed = 3f;

    private void Start()
    {
        health = maxHealth; 
    }
    private void Update()
    {
        if(health > maxHealth)
        {
            health = maxHealth; //Prevents overhealing 
        }
        lerpSpeed *= Time.deltaTime;
        FillBar();
        ChangeColor(); 
    }
    private void FillBar()
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, (health / maxHealth), lerpSpeed);
        for(int i = 0; i < healthTicks.Length; i++)
        {
            healthTicks[i].enabled = !DisplayHealthPoint(health, i); 
        }
    }
    private void ChangeColor()
    {
        Color healthColor = Color.Lerp(Color.red, Color.green, (health / maxHealth));
        healthBar.color = healthColor; 
    }
    bool DisplayHealthPoint(float _health, int pointNumber)
    {
        return ((pointNumber * healthTicks.Length) >= _health);
    }
}
