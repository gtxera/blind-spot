using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInteraction : MonoBehaviour, IInteractable
{

    [SerializeField] private DialogueSO _dialogue;
    
    public Material InteractableMaterial { get; private set; }
    
    void Start()
    {
        InteractableMaterial = GetComponent<Renderer>().material;
    }

    public void Interact(GameObject playerObject)
    {
        DialogueManager.Instance.StartDialogue(_dialogue);
    }
}
