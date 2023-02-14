using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameObjectPool
{
    private GameObject _objectPrefab, _poolParent;

    private List<GameObject> _objectPool = new List<GameObject>();

    public List<GameObject> Pool => _objectPool;

    public int Count => _objectPool.Count;
    
    public GameObjectPool(GameObject objectPrefab, GameObject poolParent)
    {
        _objectPrefab = objectPrefab;
        _poolParent = poolParent;
    }

    public GameObject GetObject(bool setActive)
    {
        foreach (var gameObject in _objectPool.Where(gameObject => !gameObject.activeSelf))
        {
            gameObject.SetActive(setActive);
            return gameObject;
        }

        var newObject = Object.Instantiate(_objectPrefab, _poolParent.transform).gameObject;
        _objectPool.Add(newObject);

        return newObject;
    }

    public void DisableObject(GameObject objectToDisable)
    {
        if (_objectPool.Contains(objectToDisable))
        {
            objectToDisable.SetActive(false);
        }
    }
    
}
