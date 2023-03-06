using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddItemToInventoryInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemSO _item;

    [SerializeField] private int _amount;
    
    public Material[] InteractableMaterials { get; set; }

    private void Start()
    {
        InteractableMaterials = GetComponent<Renderer>().materials;
    }
    
    public void Interact(GameObject playerObject)
    {
        PlayerInventory.Instance.AddItem(_item, _amount);
        
        Destroy(gameObject);
    }
}
