using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueChoice
{
    [TextArea]
    public string ChoiceText;
    public DialogueLineSO NextDialogue;
}
