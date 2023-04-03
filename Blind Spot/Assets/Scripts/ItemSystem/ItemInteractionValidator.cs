using System;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public abstract class ItemInteractionValidator
{
    protected Transform _transform;

    protected ItemSO _item;
    
    public abstract bool Validate();

    public void Initialize(Transform transform, ItemSO item)
    {
        _transform = transform;
        _item = item;
    }
}

[Serializable]
public class DistanceValidator : ItemInteractionValidator
{
    [SerializeField] private float _maxDistance;

    public override bool Validate()
    {
        var playerTransform = Object.FindObjectOfType<PlayerAnimations>().transform;

        return Vector3.Distance(_transform.position, playerTransform.position) <= _maxDistance;
    }
}

[Serializable]
public class AmountValidator : ItemInteractionValidator
{
    [SerializeField] private int _amount = 1;

    public override bool Validate()
    {
        return PlayerInventory.Instance.GetItemCount(_item) >= _amount;
    }

    public int GetAmount()
    {
        return _amount;
    }
}
