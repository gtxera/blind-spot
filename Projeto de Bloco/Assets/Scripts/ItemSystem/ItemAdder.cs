using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAdder : MonoBehaviour
{
    [SerializeField] private ItemSO _itemToAdd;

    [SerializeField] private int _amount;

    public void AddItem()
    {
        PlayerInventory.Instance.AddItem(_itemToAdd, _amount);
    }
}
