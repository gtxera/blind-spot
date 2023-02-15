using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPhase : ScriptableObject
{
    public string PhaseText;

    public GameEvent OnPhaseFinishedEvent;

    public SerializableDictionary<GameEvent, QuestPhase> NextPhases = new();

    public Quest ParentQuest;

    public QuestPhaseWrapper Wrapper;

    public void CreateWrapper()
    {
        Wrapper = new QuestPhaseWrapper(this);
    }
}
