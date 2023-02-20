using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInteraction : MonoBehaviour, IInteractable, IDialogueHolder
{

    [SerializeField] private DialogueSO _startingDialogue;
    
    public DialogueSO Dialogue { get; set; }
    
    public Material InteractableMaterial { get; private set; }
    
    void Start()
    {
        InteractableMaterial = GetComponent<Renderer>().material;
        Dialogue = _startingDialogue;
    }

    public void Interact(GameObject playerObject)
    {
        DialogueManager.Instance.StartDialogue(Dialogue);
    }
}
