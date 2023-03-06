using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ItemEventValidator))]
public class ItemEventListener : MonoBehaviour
{
    [SerializeField] private EventAndItem[] _eventAndItems;

    private List<GameEventListener> _listeners;

    private void Start()
    {
        var validator = GetComponent<ItemEventValidator>();

        foreach (var pair in _eventAndItems)
        {
            if(pair.Item.ItemEvent == null) continue;

            var listener = new GameEventListener(pair.Item.ItemEvent, () =>
            {
                if(!validator.PlayerInRange) return;
                
                pair.UnityEvent?.Invoke();
            });
            listener.EnableListener();
            
            _listeners.Add(listener);
        }
    }

    private void OnDisable()
    {
        foreach (var listener in _listeners)
        {
            listener.DisableListener();
        }
    }

    [System.Serializable]
    private struct EventAndItem
    {
        public ItemSO Item;
        
        public UnityEvent UnityEvent;
    }
}
