using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInventory : SingletonBehaviour<PlayerInventory>
{
    [SerializeField] private GameObject _persistentInventoryManagerPrefab;
    
    private List<InventorySpace> _inventory = new();

    public List<InventorySpace> Inventory => _inventory;

    public event Action<ItemSO, int> ItemNewAdded; 

    public event Action<ItemSO, int> ItemCountChanged;

    public event Action<ItemSO> ItemRemoved;

    private bool _mouseInInventory;

    private IEnumerator Start()
    {
        if (FindObjectOfType<PersistentInventoryManager>() == null)
        {
            Instantiate(_persistentInventoryManagerPrefab);
        }
        
        else
        {
            _inventory = PersistentInventoryManager.Instance.LastInventory;
        }
        
        yield return new WaitForEndOfFrame();
        PopulateInventoryUI();
    }

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

    public void ReorderInventory(ItemSO movedItem, int position)
    {
        var movedItemSpace = _inventory.First(space => space.Item = movedItem);
        
        for (int i = position; i < _inventory.Count; i++)
        {
            _inventory[i + 1] = _inventory[i];
        }

        _inventory[position] = movedItemSpace;
    }

    private void PopulateInventoryUI()
    {
        foreach (var space in _inventory)
        {
            ItemNewAdded?.Invoke(space.Item, space.ItemCount);
        }
    }

    public class InventorySpace
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
