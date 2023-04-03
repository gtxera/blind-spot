using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventRaiserInteractible : MonoBehaviour, IInteractable, IGameEventHolder
{
    [SerializeField] private GameEvent _startingEvent;
    
    
    public GameEvent Event { get; set; }

    private void Start()
    {
        Event = _startingEvent;

        var mesh = GetComponent<MeshRenderer>();
        
    }
    
    public void Interact(GameObject playerObject)
    {
        Event.RaiseEvent();
    }

    public void SetOutlines(bool isActive)
    {
        gameObject.layer = isActive ? 6 : 0;
    }
}
