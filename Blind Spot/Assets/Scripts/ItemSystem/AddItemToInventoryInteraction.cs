using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FMODUnity.StudioEventEmitter))]
public class AddItemToInventoryInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemSO _item;

    [SerializeField] private int _amount;

    private FMODUnity.EventReference _eventReference;

    private FMODUnity.StudioEventEmitter _emitter;

    private const string PATH_TO_PICKUP_SOUND = "event:/PickUpSound";

    private void Start()
    {
        _emitter = GetComponent<FMODUnity.StudioEventEmitter>();
        _eventReference = FMODUnity.RuntimeManager.PathToEventReference(PATH_TO_PICKUP_SOUND);
        _emitter.EventReference = _eventReference;
    }

    public void Interact(GameObject playerObject)
    {
        PlayerInventory.Instance.AddItem(_item, _amount);

        PlayerInteraction.Instance.IgnoreInteraction(false);
        
        _emitter.Play();
        
        gameObject.SetActive(false);
    }
    
    public void SetOutlines(bool isActive)
    {
        GameObjectUtility.SetLayerRecursively(gameObject, isActive ? 6 : 0);
    }
}
