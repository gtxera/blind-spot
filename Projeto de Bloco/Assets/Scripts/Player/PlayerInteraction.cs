using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Player;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteraction : SingletonBehaviour<PlayerInteraction>
{
    private IInteractable _currentInteraction;

    private bool _currentInteractionIsLocked;

    void Start()
    {
        PlayerInputs.Instance.EKeyDownEvent += CallInteraction;
    }

    private void CallInteraction()
    {
        _currentInteraction?.Interact(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_currentInteractionIsLocked)
        {
            _currentInteraction = other.GetComponent<IInteractable>();
            _currentInteraction?.SetOutlines(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_currentInteractionIsLocked)
        {
            _currentInteraction?.SetOutlines(false);
            _currentInteraction = null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!_currentInteractionIsLocked)
        {
            _currentInteraction = other.GetComponent<IInteractable>();
            _currentInteraction?.SetOutlines(true);
        }
    }

    public void LockInteraction(bool isLocked)
    {
        _currentInteraction.SetOutlines(false);
        _currentInteractionIsLocked = isLocked;
    }

    public void IgnoreInteraction(bool isLocked)
    {
        _currentInteraction?.SetOutlines(false);
        _currentInteraction = null;
        _currentInteractionIsLocked = isLocked;
    }
}
