using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private UnityEvent _interactionEvent;
    
    public Material[] InteractableMaterials { get; set; }

    private void Start()
    {
        InteractableMaterials = GetComponent<Renderer>().materials;
    }

    public void Interact(GameObject playerObject)
    {
        _interactionEvent?.Invoke();
    }
}
