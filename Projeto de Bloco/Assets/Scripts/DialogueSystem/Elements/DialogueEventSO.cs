using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEventSO : ScriptableObject
{
    private HashSet<DialogueEventListener> _listeners = new HashSet<DialogueEventListener>();
    
    public void Invoke()
    {
        foreach(DialogueEventListener listener in _listeners)
            listener.RaiseEvent();
    }

    public void Register(DialogueEventListener listener) => _listeners.Add(listener);
    public void Deregister(DialogueEventListener listener) => _listeners.Remove(listener);
}
