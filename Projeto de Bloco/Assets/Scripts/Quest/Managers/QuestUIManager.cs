using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class QuestUIManager : MonoBehaviour
{
    private void Start()
    {
        QuestManager.Instance.OnPhaseTransition += (_, phase) =>
        {
            PauseManager.Instance.QuestText.text = phase.PhaseWaypointName;
        };
    }
}
