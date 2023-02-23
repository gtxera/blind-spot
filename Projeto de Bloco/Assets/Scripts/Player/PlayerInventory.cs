using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInventory : SingletonBehaviour<PlayerInventory>
{
    private readonly List<InventorySpace> _inventory = new();

    public event Action<ItemSO, int> ItemNewAdded; 

    public event Action<ItemSO, int> ItemCountChanged;

    public event Action<ItemSO> ItemRemoved;

    private bool _mouseInInventory;

    public bool HasItem(ItemSO item)
    {
        return _inventory.Any(space => space.Item == item);
    }

    public int GetItemCount(ItemSO item)
    {
        return HasItem(item) ? _inventory.First(space => space.Item == item).ItemCount : 0;
    }

    public void AddItem(ItemSO item, int amount = 1)
    {
        var space = _inventory.FirstOrDefault(space => space.Item == item);

        if (space == null)
        {
            _inventory.Add(new InventorySpace(item, item.Stackable ? amount : 1));
            ItemNewAdded?.Invoke(item, amount);
            return;
        }

        space.ItemCount += space.Item.Stackable ? amount : 0;
        ItemCountChanged?.Invoke(item, space.ItemCount);
    }

    public void RemoveItem(ItemSO item, int amount = 1)
    {
        if (!HasItem(item)) return;
        
        var space = _inventory.FirstOrDefault(space => space.Item == item);

        if (space.ItemCount < amount) return;

        space.ItemCount -= amount;
        ItemCountChanged?.Invoke(item, space.ItemCount);

        if (space.ItemCount == 0)
        {
            _inventory.Remove(space);
            ItemRemoved?.Invoke(item);
        }
    }

    private class InventorySpace
    {
        public ItemSO Item;

        public int ItemCount;
        
        public InventorySpace(ItemSO item, int amount = 1)
        {
            Item = item;
            ItemCount = amount;
        }
    }
}
