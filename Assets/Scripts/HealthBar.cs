using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))] // Ensures object will have an Animator
public class HealthBar : MonoBehaviour
{
    [SerializeField] private FloatAsset _health; // Health Asset
    [SerializeField] private float maxHealth;

    private Slider healthBar;
    

    // Start is called before the first frame update
    void Start()
    {
        healthBar = GetComponent<Slider>();
        healthBar.maxValue = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = _health.Value;
    }
}
