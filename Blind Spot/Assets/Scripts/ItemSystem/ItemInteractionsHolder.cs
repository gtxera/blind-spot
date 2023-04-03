using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemInteractionsHolder : MonoBehaviour
{
    [SerializeField] private ItemInteraction[] _interactions;

    private void Start()
    {
        foreach (var interaction in _interactions)
        {
            interaction.Initialize(transform);
        }
    }

    public void TryInteract(ItemSO item)
    {
        _interactions.First(interaction => interaction.GetItem() == item).Interact();
    }
}
