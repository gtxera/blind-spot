using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPhase : ScriptableObject
{
    [TextArea]
    public string PhaseText;

    public GameEvent OnPhaseFinishedEvent;
    
    public SerializableDictionary<GameEvent, QuestPhase> NextPhases = new();

    public Quest ParentQuest;

    [HideInInspector]
    public QuestPhaseWrapper Wrapper;

    public void CreateWrapper()
    {
        Wrapper = new QuestPhaseWrapper(this);
    }
}
