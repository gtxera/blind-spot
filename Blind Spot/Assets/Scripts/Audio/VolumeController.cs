using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private SoundGroup _soundGroup;
    
    

    private FMOD.Studio.VCA _vca;

    private void Start()
    {
        _vca = FMODUnity.RuntimeManager.GetVCA($"vca:/{_soundGroup.ToString()}");
        
        _volumeSlider.onValueChanged.AddListener(val =>
        {
            _vca.setVolume(val);
        });
    }

    private enum SoundGroup
    {
        Music,
        SFX,
    }
}
