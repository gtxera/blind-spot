using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInteraction : MonoBehaviour, IInteractable, IDialogueHolder
{

    [SerializeField] private DialogueSO _startingDialogue;
    
    public DialogueSO Dialogue { get; set; }
    
    public Material[] InteractableMaterials { get; set; }
    
    void Start()
    {
        InteractableMaterials = GetComponent<Renderer>().materials;
        Dialogue = _startingDialogue;
    }

    public void Interact(GameObject playerObject)
    {
        DialogueManager.Instance.StartDialogue(Dialogue);
    }
}
