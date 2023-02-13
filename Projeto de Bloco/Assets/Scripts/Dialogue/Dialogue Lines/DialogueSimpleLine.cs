using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSimpleLine : DialogueLineSO
{
    public DialogueLineSO NextDialogue;
    [TextArea]
    public string Text;
}
