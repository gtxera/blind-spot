using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginQuestInteraction : MonoBehaviour, IInteractable, IQuestHolder
{
    [SerializeField] private Quest _startingQuest;
    
    public Quest Quest { get; set; }
    
    public Material[] InteractableMaterials { get; set; }

    private void Start()
    {
        Quest = _startingQuest;

        var mesh = GetComponent<MeshRenderer>();

        InteractableMaterials = mesh.materials;
    }

    public void Interact(GameObject playerObject)
    {
        QuestManager.Instance.StartQuest(Quest);
    }
}
