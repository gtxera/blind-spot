using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRemover : MonoBehaviour
{
    [SerializeField] private ItemSO _itemToRemove;

    [SerializeField] private int _amount;

    public void RemoveItem()
    {
        PlayerInventory.Instance.RemoveItem(_itemToRemove, _amount);
    }
}
