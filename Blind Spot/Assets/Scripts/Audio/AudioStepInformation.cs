using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioStepInformation : MonoBehaviour
{
    [SerializeField] private AudioStepType _stepType;

    public AudioStepType StepType => _stepType;
}

public enum AudioStepType
{
    Grass,
    Wood,
    Asphalt
}