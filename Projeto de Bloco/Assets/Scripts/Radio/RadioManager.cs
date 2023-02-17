using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioManager : SingletonBehaviour<RadioManager>
{
    [SerializeField] private float _frequencyIncrement;

    private const float MOUSE_WHEEL_NORMALIZER = 10f;
    
    public float CurrentFrequency
    {
        get => _currentFrequency;
        private set => _currentFrequency = Mathf.Clamp(value.RoundWithDecimals(1), 90f, 150f);
    }

    private float _currentFrequency = 100f;
    
    void Start()
    {
        PlayerInputs.Instance.MouseWheelMove += AdjustFrequency;
        PlayerInputs.Instance.RadioKeyDown += TryStartRadioDialogue;
    }

    private void AdjustFrequency(float mouseMovement)
    {
        CurrentFrequency += mouseMovement * _frequencyIncrement * MOUSE_WHEEL_NORMALIZER;
    }

    private void TryStartRadioDialogue()
    {
        switch (_currentFrequency)
        {
            case 103.2f:
                Debug.Log("opa");
                break;
            
            default:
                Debug.Log("statica");
                break;
        }
    }
}
