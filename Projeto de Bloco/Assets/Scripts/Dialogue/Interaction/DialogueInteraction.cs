using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInteraction : MonoBehaviour, IInteractable, IDialogueHolder
{

    [SerializeField] private DialogueSO _startingDialogue;
    
    public DialogueSO Dialogue { get; set; }
    

    public void Interact(GameObject playerObject)
    {
        DialogueManager.Instance.StartDialogue(Dialogue);
    }
    
    public void SetOutlines(bool isActive)
    {
        GameObjectUtility.SetLayerRecursively(gameObject, isActive ? 6 : 0);
    }
}
