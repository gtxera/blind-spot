using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEventRaiser : MonoBehaviour
{
    [SerializeField] private ItemSO item;

    public void RaiseItemEvent()
    {
        item.ItemEvent.RaiseEvent();
    }
}
