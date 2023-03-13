using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentInventoryManager : SingletonBehaviour<PersistentInventoryManager>
{
    private List<PlayerInventory.InventorySpace> _lastInventory = new();

    public List<PlayerInventory.InventorySpace> LastInventory => _lastInventory;
    

    private new void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        FadeOutManager.FadeOutStarted += GetLastInventory;
    }

    private void GetLastInventory()
    {
        _lastInventory = PlayerInventory.Instance.Inventory;
    }
    
    private void OnDisable()
    {
        FadeOutManager.FadeOutStarted -= GetLastInventory;
    }
}
