#if UNITY_EDITOR
using UnityEditor;
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

[ExecuteAlways]
public class SaveObjectID : MonoBehaviour
{
    [SerializeField] private uint _id;

    public uint ID => _id;
    

    #if UNITY_EDITOR
    private void Awake()
    {
        if (!Application.isPlaying && _id == 0)
        {
            if (SaveObjectIDManager.GameObjectById.ContainsValue(gameObject)) _id = SaveObjectIDManager.IdByGameObject[gameObject];

            else _id = SaveObjectIDManager.GetFreeId(gameObject);

            EditorUtility.SetDirty(this);
        }
    }
    #endif

    private void Start()
    {
        if (Application.isPlaying && _id == 0)
        {
            if (SaveObjectIDManager.GameObjectById.ContainsValue(gameObject))
                _id = SaveObjectIDManager.IdByGameObject[gameObject];
            else _id = SaveObjectIDManager.GetFreeId(gameObject);
        }
    }
}
