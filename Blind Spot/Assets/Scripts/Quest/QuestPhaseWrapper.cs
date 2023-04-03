using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestPhaseWrapper
{
    public QuestPhase QuestPhase;

    public QuestPhaseWrapper(QuestPhase questPhase)
    {
        QuestPhase = questPhase;
    }
}
