using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/New Dialogue")]
public class DialogueSO : ScriptableObject
{
    public string Name;
    public bool HasNameSet;
    public List<DialogueLineSO> DialogueLines;
}
