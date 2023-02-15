using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Quest")]
public class Quest : ScriptableObject
{
    public bool IsMandatory;
    
    public List<QuestPhase> QuestPhases = new();
}
