using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class GameEventListener
{
    [SerializeField] private GameEvent _gameEvent;
    [SerializeField] private UnityEvent _event;

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
        _event?.Invoke();
    }
}
