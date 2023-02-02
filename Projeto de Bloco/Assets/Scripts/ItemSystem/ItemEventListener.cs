using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class ItemEventListener
{
    [SerializeField] private ItemSO item;
    [SerializeField] private UnityEvent response;

    public void EnableListener()
    {
        item.RegisterListener(this);
    }

    public void UnableListener()
    {
        item.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        response.Invoke();
    }
}
