using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventListenersHolder : MonoBehaviour
{
    [SerializeField] private List<GameEventListener> _listeners = new List<GameEventListener>();

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
            listener.DisableListener();
        }
    }
}
