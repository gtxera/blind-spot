using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[System.Serializable]
public class GameEventListener
{
    public GameEventListener(GameEvent gameEvent, Action scriptEvent = null)
    {
        _gameEvent = gameEvent;
        _scriptEvent = scriptEvent;
    }
    
    [SerializeField] private GameEvent _gameEvent;
    [SerializeField] private UnityEvent _unityEvent;

    private Action _scriptEvent;

    public void EnableListener()
    {
        _gameEvent.RegisterListener(this);
    }

    public void DisableListener()
    {
        _gameEvent.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        _unityEvent?.Invoke();
        _scriptEvent?.Invoke();
    }
}
