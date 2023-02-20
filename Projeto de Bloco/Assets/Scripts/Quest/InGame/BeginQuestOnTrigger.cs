using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginQuestOnTrigger : MonoBehaviour, IQuestHolder
{

    [SerializeField] private Quest _startingQuest;
    
    public Quest Quest { get; set; }
    
    private void Start()
    {
        Quest = _startingQuest;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerInputs>(out _))
        {
            QuestManager.Instance.StartQuest(Quest);
        }
    }
}
