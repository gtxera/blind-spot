using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginQuestInteraction : MonoBehaviour, IInteractable, IQuestHolder
{
    [SerializeField] private Quest _startingQuest;
    
    public Quest Quest { get; set; }
    
    public Material InteractableMaterial { get; }

    private void Start()
    {
        Quest = _startingQuest;
    }

    public void Interact(GameObject playerObject)
    {
        QuestManager.Instance.StartQuest(Quest);
    }
}
