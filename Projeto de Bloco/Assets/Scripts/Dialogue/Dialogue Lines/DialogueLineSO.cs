using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueLineSO : ScriptableObject
{
    [SerializeReference]
    public DialogueLineSOWrapper Wrapper = new DialogueLineSOWrapper();

    public void CreateWrapper()
    {
        Wrapper.DialogueLineSO = this;
    }
}
