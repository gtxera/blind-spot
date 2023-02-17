using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventRaiserInteractible : MonoBehaviour, IInteractable
{
    [SerializeField] private GameEvent _gameEvent;

    public Material InteractableMaterial { get; }
    public void Interact(GameObject playerObject)
    {
        _gameEvent.RaiseEvent();
    }
}
