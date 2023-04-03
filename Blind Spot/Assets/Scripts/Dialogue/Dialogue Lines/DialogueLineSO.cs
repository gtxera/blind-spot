using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueLineSO : ScriptableObject
{
    [HideInInspector] public DialogueLineSOWrapper Wrapper = new DialogueLineSOWrapper();

    public DialogueSO ParentDialogue;

    [HideInInspector] public string LineIdentifier;

    public void CreateWrapper()
    {
        Wrapper.DialogueLineSO = this;
    }
}
