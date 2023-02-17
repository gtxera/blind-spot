using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginQuestInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private Quest _quest;
    
    public Material InteractableMaterial { get; }
    public void Interact(GameObject playerObject)
    {
        QuestManager.Instance.StartQuest(_quest);
    }
}
