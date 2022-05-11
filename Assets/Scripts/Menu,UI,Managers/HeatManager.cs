using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 
public class HeatManager : MonoBehaviour
{
    //Heat System: 
    //UI Fields
    [SerializeField] private GameObject _heatPanel;
    [SerializeField] private TextMeshProUGUI _heatNumber;

    //Variable Fields
    public float CurrentHeatFalloff = 0.0f;
    public int CurrentHeatNum = 0;
    public bool HeatLvl1 = false;
    public bool HeatLvl2 = false;
    public float HeatFalloff;
    public int HeatPerHit = 1; 
    [SerializeField] private int _heatInitialLevel = 20;
    [SerializeField] private int _heatLevelIncrement;






    public void UpdateHeatUI()
    {
        if (CurrentHeatFalloff > 0)
        {
            _heatPanel.SetActive(true);
            _heatNumber.SetText(CurrentHeatNum.ToString());
            if (CurrentHeatNum >= _heatInitialLevel && HeatLvl1 == false)
            {
                HeatLvl1 = true;
                _heatNumber.color = Color.cyan;
            }
            else if (CurrentHeatNum >= _heatInitialLevel + _heatLevelIncrement && HeatLvl2 == false)
            {
                HeatLvl2 = true;
                _heatNumber.color = Color.magenta;
            }
        }
        else
        {
            //Reset heat levels
            HeatLvl1 = false;
            HeatLvl2 = false;
            //Reset fonts and buffs
            _heatNumber.color = Color.black;
            //Deactivate panels, reset heat number
            _heatPanel.SetActive(false);
            CurrentHeatNum = 0; 
        }
    }

}
