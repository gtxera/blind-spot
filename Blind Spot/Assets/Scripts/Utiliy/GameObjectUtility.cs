using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectUtility
{
    public static void SetLayerRecursively(GameObject gameObject, int newLayer)
    {
        gameObject.layer = newLayer;

        foreach (Transform child in gameObject.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}
