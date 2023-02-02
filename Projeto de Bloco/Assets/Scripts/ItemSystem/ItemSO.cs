using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
    public Image ItemImage;
    
    private List<ItemEventListener> _listeners = new List<ItemEventListener>();

    public void RaiseEvent()
    {
        for (int i = _listeners.Count - 1; i >= 0; i--)
        {
            _listeners[i].OnEventRaised();
        }
    }

    public void RegisterListener(ItemEventListener listener)
    {
        _listeners.Add(listener);
    }

    public void UnregisterListener(ItemEventListener listener)
    {
        _listeners.Remove(listener);
    }
}
