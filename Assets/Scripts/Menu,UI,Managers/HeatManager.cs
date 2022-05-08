using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 
public class HeatManager : MonoBehaviour
{
    //UI Fields
    [SerializeField] private GameObject _heatPanel;
    [SerializeField] private TextMeshProUGUI _heatNumber; 

    //Heat System Fields
    public int _heatPerHit = 1; //Amount of heat gained per successful hit  
    private int _currentHeatNum = 0; //Current heat 
    [SerializeField] private int _heatInitialLevel = 20;
    [SerializeField] private int _heatLevelIncrement;



    public void UpdateHeat(float heatFalloff)
    {
        //Step 1: determine heat falloff
        //Heat falls off either if no one attacked within the falloff time
        if (heatFalloff > 0)
        {
            _heatPanel.SetActive(true);
            _currentHeatNum += +_heatPerHit;
            _heatNumber.SetText(_currentHeatNum.ToString());
            if (_currentHeatNum >= _heatInitialLevel && _currentHeatNum < _heatInitialLevel + _heatLevelIncrement)
            {
                _heatNumber.color = Color.cyan;
            }
            else if (_currentHeatNum >= 20)
            {
                _heatNumber.color = Color.magenta;
            }
        }
        else
        {
            _heatPanel.SetActive(false);
            _currentHeatNum = 0;
        }

    }
}
