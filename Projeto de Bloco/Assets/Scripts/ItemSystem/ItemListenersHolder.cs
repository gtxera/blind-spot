using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemListenersHolder : MonoBehaviour
{
    [SerializeField] private ItemEventListener[] _listeners;

    private void OnEnable()
    {
        foreach (var listener in _listeners)
        {
            listener.EnableListener();
        }
    }

    private void OnDisable()
    {
        foreach (var listener in _listeners)
        {
            listener.UnableListener();
        }
    }
}
