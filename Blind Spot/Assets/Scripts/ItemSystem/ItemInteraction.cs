using System;
using UnityEngine;
using UnityEngine.Events;


[Serializable]
public class ItemInteraction
{
    [SerializeField] private ItemSO _item;
    
    [SerializeField] private DistanceValidator _distanceValidator;

    [SerializeField] private AmountValidator _amountValidator;
    
    [SerializeField] private bool _shouldConsumeAmount;

    [SerializeField] private UnityEvent _unityEvent;
    
    public ItemSO GetItem()
    {
        return _item;
    }

    public void Initialize(Transform transform)
    {
        _distanceValidator.Initialize(transform, _item);
        _amountValidator.Initialize(transform, _item);
    }

    public void Interact()
    {
        if (!_distanceValidator.Validate() || !_amountValidator.Validate()) return;
        
        _unityEvent?.Invoke();
        
        if(_shouldConsumeAmount) PlayerInventory.Instance.RemoveItem(_item, _amountValidator.GetAmount());
    }
}
