using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DialogueEventListener
{
    [SerializeField] private DialogueEventSO _dialogueEvent;
    [SerializeField] private UnityEvent _response;
    
    public void EnableListener()
    {
        _dialogueEvent.RegisterListener(this);
    }

    public void UnableListener()
    {
        _dialogueEvent.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        _response?.Invoke();
    }
}
