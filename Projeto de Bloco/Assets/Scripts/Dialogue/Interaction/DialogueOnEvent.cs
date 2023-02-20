using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueOnEvent : MonoBehaviour
{
    [SerializeField] private EventAndDialogue[] _dialogues;
    
    private readonly List<GameEventListener> _listeners = new();

    private void OnEnable()
    {
        foreach (var eventAndDialogue in _dialogues)
        {
            var listener = new GameEventListener(eventAndDialogue.Event, () =>
            {
                DialogueManager.Instance.StartDialogue(eventAndDialogue.Dialogue);
            });
            
            listener.EnableListener();
            
            _listeners.Add(listener);
        }
    }

    private void OnDisable()
    {
        foreach (var listener in _listeners)
        {
            listener.DisableListener();
        }
    }

    [Serializable]
    private struct EventAndDialogue
    {
        public GameEvent Event;
        public DialogueSO Dialogue;
    }
}
