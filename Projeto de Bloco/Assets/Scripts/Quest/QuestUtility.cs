using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class QuestUtility
{
    #if UNITY_EDITOR
    public static readonly QuestPhase FinishThisQuest =
        AssetDatabase.LoadAssetAtPath<QuestPhase>("Assets/Quests/Phases/Finish.asset");
    #endif
}
