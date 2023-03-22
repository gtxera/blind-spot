using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchDialogueHolderDialogue : MonoBehaviour
{
    [SerializeField] private DialogueSO _newDialogue;

    public void ChangeDialogue()
    {
        GetComponent<IDialogueHolder>().Dialogue = _newDialogue;
    }
}
