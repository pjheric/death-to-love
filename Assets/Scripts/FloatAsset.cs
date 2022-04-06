using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Float Value", menuName = "ScriptableObjects/FloatValue", order = 1)]
public class FloatAsset : ScriptableObject
{
    [SerializeField]
    private float _floatValue;

    public float Value
    {
        get
        {
            return _floatValue;
        }
        set
        {
            _floatValue = value;
        }
    }
}
