using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventRaiserInteractible : MonoBehaviour, IInteractable, IGameEventHolder
{
    [SerializeField] private GameEvent _startingEvent;
    
    public Material[] InteractableMaterials { get; set; }
    
    public GameEvent Event { get; set; }

    private void Start()
    {
        Event = _startingEvent;

        var mesh = GetComponent<MeshRenderer>();

        InteractableMaterials = mesh.materials;
    }

    

    public void Interact(GameObject playerObject)
    {
        Event.RaiseEvent();
    }
}
