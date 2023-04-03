using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameObjectPool
{
    private readonly GameObject _objectPrefab, _poolParent;

    public List<GameObject> Pool { get; } = new List<GameObject>();

    public int Count => Pool.Count;
    
    public GameObjectPool(GameObject objectPrefab, GameObject poolParent = null)
    {
        _objectPrefab = objectPrefab;
        _poolParent = poolParent;
    }

    public GameObject GetObject(bool setActive)
    {
        foreach (var gameObject in Pool.Where(gameObject => !gameObject.activeSelf))
        {
            gameObject.SetActive(setActive);
            return gameObject;
        }

        var newObject = Object.Instantiate(_objectPrefab, _poolParent.transform).gameObject;
        Pool.Add(newObject);
        newObject.SetActive(setActive);
        
        return newObject;
    }

    public void DisableObject(GameObject objectToDisable)
    {
        if (Pool.Contains(objectToDisable))
        {
            objectToDisable.SetActive(false);
        }
    }
    
}
