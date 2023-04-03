using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddItemToInventoryInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemSO _item;

    [SerializeField] private int _amount;

    public void Interact(GameObject playerObject)
    {
        PlayerInventory.Instance.AddItem(_item, _amount);

        PlayerInteraction.Instance.IgnoreInteraction(false);
        
        gameObject.SetActive(false);
    }
    
    public void SetOutlines(bool isActive)
    {
        GameObjectUtility.SetLayerRecursively(gameObject, isActive ? 6 : 0);
    }
}
