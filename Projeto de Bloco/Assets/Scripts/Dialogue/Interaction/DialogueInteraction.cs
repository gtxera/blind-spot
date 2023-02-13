using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInteraction : MonoBehaviour, IInteractable
{

    [SerializeField] private DialogueSO _dialogue;
    
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    public void Interact(GameObject playerObject)
    {
        DialogueManager.Instance.StartDialogue(_dialogue);
    }
}
