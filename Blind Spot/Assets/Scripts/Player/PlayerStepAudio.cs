using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FMODUnity.StudioEventEmitter))]
public class PlayerStepAudio : MonoBehaviour
{
    private FMODUnity.StudioEventEmitter _emitter;

    private void Start()
    {
        _emitter = GetComponent<FMODUnity.StudioEventEmitter>();
    }

    private void Update()
    {
        if (Physics.Raycast(transform.position, -transform.up, out var hit))
        {
            if (hit.transform.TryGetComponent<AudioStepInformation>(out var info))
            {
                _emitter.SetParameter("Parameter 1", (float)info.StepType);
            }
        }
    }

    public void PlayStepSound()
    {
        _emitter.Play();
    }
}
