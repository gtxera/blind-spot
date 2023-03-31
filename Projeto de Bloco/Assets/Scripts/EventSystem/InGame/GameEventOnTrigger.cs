using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventOnTrigger : MonoBehaviour, IGameEventHolder
{
    [SerializeField] private GameEvent _startingEvent;
    
    public GameEvent Event { get; set; }

    private void Start()
    {
        Event = _startingEvent;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerMovementStateMachine>(out _))
        {
            Event.RaiseEvent();
        }
    }
}
