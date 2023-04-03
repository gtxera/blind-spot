using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private UnityEvent _interactionEvent;

    public void Interact(GameObject playerObject)
    {
        _interactionEvent?.Invoke();
    }
    
    public void SetOutlines(bool isActive)
    {
        GameObjectUtility.SetLayerRecursively(gameObject, isActive ? 6 : 0);
    }
}
