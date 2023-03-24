using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public static class SaveObjectIDManager
{
    public static Dictionary<uint, GameObject> GameObjectById = new();
    public static Dictionary<GameObject, uint> IdByGameObject = new();

    private static Random _random = new();

    public static uint GetFreeId(GameObject gameObject)
    {
        uint id = 0;

        do
        {
            id = RandomUInt();
        } while (id == 0 || GameObjectById.ContainsKey(id));

        GameObjectById.Add(id, gameObject);
        IdByGameObject.Add(gameObject, id);

        foreach (var pair in GameObjectById)
        {
            Debug.Log(pair.Value);
        }
        
        return id;
    }

    private static uint RandomUInt()
    {
        uint thirtyBits = (uint)_random.Next(1 << 30);
        uint twoBits = (uint)_random.Next(1 << 2);
        return (thirtyBits << 2) | twoBits;
    }
}
