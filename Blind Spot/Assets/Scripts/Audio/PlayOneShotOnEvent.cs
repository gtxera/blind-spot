using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOneShotOnEvent : MonoBehaviour
{
    [SerializeField] private GameEvent _gameEvent;
    [SerializeField] private FMODUnity.EventReference _event;

    private GameEventListener _listener;
    
    private void Start()
    {
        _listener = new GameEventListener(_gameEvent, () =>
        {
            FMODUnity.RuntimeManager.PlayOneShot(_event);
        });
        _listener.EnableListener();
    }

    private void OnDisable()
    {
        _listener.DisableListener();
    }
}
