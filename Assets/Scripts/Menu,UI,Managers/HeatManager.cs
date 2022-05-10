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
    private float _currentHeatFalloff = 0.0f;
    [SerializeField] private float _heatFalloff = 1.0f;
    [SerializeField] private int _heatPerHit = 1; //Amount of heat gained per successful hit  
    private int _currentHeatNum = 0; //Current heat 
    [SerializeField] private int _heatInitialLevel = 20;
    [SerializeField] private int _heatLevelIncrement;
    private bool _heatLvl1 = false;
    private bool _heatLvl2 = false;


    public void UpdateHeat()
    {
        if (_currentHeatFalloff > 0)
        {
            _heatPanel.SetActive(true);
            _heatNumber.SetText(_currentHeatNum.ToString());
            if (_currentHeatNum >= _heatInitialLevel && _heatLvl1 == false)
            {
                _heatLvl1 = true;
                _heatNumber.color = Color.cyan;
                //_lightAtkCooldown *= 1.15f;
            }
            else if (_currentHeatNum >= _heatInitialLevel + _heatLevelIncrement && _heatLvl2 == false)
            {
                _heatLvl2 = true;
                _heatNumber.color = Color.magenta;
                //_attackRange *= 1.25f;
            }
        }
        else
        {
            //Reset heat levels
            _heatLvl1 = false;
            _heatLvl2 = false;
            //Reset fonts and buffs
            _heatNumber.color = Color.black;
            //Deactivate panels, reset heat number
            _heatPanel.SetActive(false);
            _currentHeatNum = 0;
        }
    }

    public void UpdateHeatUI(float currentHeatFalloff, int currentHeatNum)
    {
        if (currentHeatFalloff > 0)
        {
            _heatPanel.SetActive(true);
            _heatNumber.SetText(currentHeatNum.ToString());
            if (_currentHeatNum >= _heatInitialLevel && _heatLvl1 == false)
            {
                _heatLvl1 = true;
                _heatNumber.color = Color.cyan;
            }
            else if (_currentHeatNum >= _heatInitialLevel + _heatLevelIncrement && _heatLvl2 == false)
            {
                _heatLvl2 = true;
                _heatNumber.color = Color.magenta;
            }
        }
        else
        {
            //Reset heat levels
            _heatLvl1 = false;
            _heatLvl2 = false;
            //Reset fonts and buffs
            _heatNumber.color = Color.black;
            //Deactivate panels, reset heat number
            _heatPanel.SetActive(false);
            _currentHeatNum = 0;
        }
    }

}
