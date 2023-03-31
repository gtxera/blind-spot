using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueOnTrigger : MonoBehaviour, IDialogueHolder
{
    [SerializeField] private DialogueSO _startingDialogue;
    
    public DialogueSO Dialogue { get; set; }

    private void Start()
    {
        Dialogue = _startingDialogue;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerInputs>(out _))
        {
            DialogueManager.Instance.StartDialogue(Dialogue);
        }
    }
}
