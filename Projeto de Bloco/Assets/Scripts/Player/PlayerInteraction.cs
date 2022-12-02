using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Player;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public static PlayerInteraction Instance;
    
    private IInteractable _currentInteraction;

    private bool _currentInteractionIsLocked;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    void Start()
    {
        PlayerInputs.Instance.EKeyDownEvent += CallInteraction;
    }

    private void CallInteraction()
    {
       if(_currentInteraction != null) _currentInteraction.Interact(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!_currentInteractionIsLocked) _currentInteraction = other.GetComponent<IInteractable>();
    }

    private void OnTriggerExit(Collider other)
    {
        if(!_currentInteractionIsLocked) _currentInteraction = null;
    }

    public void LockInteraction(bool isLocked)
    {
        _currentInteractionIsLocked = isLocked;
    }
}
