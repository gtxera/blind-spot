using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DS.ScriptableObjects
{
    using Enumerations;
    using Data;
    public class DSEventDialogueSO : DSDialogueSO
    {
        [field: SerializeField] public DialogueEventSO DialogueEvent { get; set; }
        
        public void Initialize(string dialogueName, string text, List<DSDialogueChoiceData> choices, DialogueType dialogueType, bool isStartingDialogue, DialogueEventSO dialogueEvent)
        {
            base.Initialize(dialogueName, text, choices, dialogueType, isStartingDialogue);

            DialogueEvent = dialogueEvent;
        }
    }
}
