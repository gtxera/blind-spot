using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueHolderUpdator : MonoBehaviour
{
    [SerializeField] private IDialogueHolder _dialogueHolder;
    
    [SerializeField] private List<EventAndDialogue> EventsAndDialogues;

    private List<GameEventListener> _eventListeners;

    private void OnEnable()
    {
        foreach (var eventAndDialogue in EventsAndDialogues)
        {
            var listener = new GameEventListener(eventAndDialogue.Event, (() =>
            {
                ChangeDialogue(eventAndDialogue.Dialogue);
            }));
            listener.EnableListener();
            _eventListeners.Add(listener);
        }
    }

    private void OnDisable()
    {
        foreach (var eventListener in _eventListeners)
        {
            eventListener.DisableListener();
        }
    }

    [System.Serializable]
    private struct EventAndDialogue
    {
        public GameEvent Event;
        public DialogueSO Dialogue;
    }

    private void ChangeDialogue(DialogueSO dialogue)
    {
        _dialogueHolder.Dialogue = dialogue;
    }
}
