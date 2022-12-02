using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DS.ScriptableObjects
{
    using Enumerations;
    using Data;
    public class DSCharacterDialogueSO : DSDialogueSO
    {
        [field: SerializeField] public string CharacterName { get; set; }
        [field: SerializeField] public Sprite CharacterSprite { get; set; }

        public void Initialize(string dialogueName, string text, List<DSDialogueChoiceData> choices, DialogueType dialogueType, bool isStartingDialogue, string characterName, Sprite characterSprite)
        {
            base.Initialize(dialogueName, text, choices, dialogueType, isStartingDialogue);
            
            CharacterName = characterName;
            CharacterSprite = characterSprite;
        }
        
        
    }
}
