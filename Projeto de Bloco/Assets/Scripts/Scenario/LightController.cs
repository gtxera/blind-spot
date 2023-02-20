using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LightController : MonoBehaviour
{
    [Range(0, 24)] 
    [SerializeField] private float _hour;

    private float _normalizedHour => _hour / 24;
    
    [SerializeField] private Light _light;

    [SerializeField] private Gradient _cycleGradient;

    private void Update()
    {
        _light.color = _cycleGradient.Evaluate(_normalizedHour);

        transform.eulerAngles = new Vector3(180 * _normalizedHour, 30, 0);
    }
}
