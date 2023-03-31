using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public static class SaveObjectIDManager
{
    public static HashSet<uint> UsedIds = new();

    private static Random _random = new();

    public static uint GetFreeId()
    {
        uint id = 0;

        do
        {
            id = RandomUInt();
        } while (id == 0 || UsedIds.Contains(id));

        UsedIds.Add(id);

        return id;
    }

    private static uint RandomUInt()
    {
        uint thirtyBits = (uint)_random.Next(1 << 30);
        uint twoBits = (uint)_random.Next(1 << 2);
        return (thirtyBits << 2) | twoBits;
    }
}
