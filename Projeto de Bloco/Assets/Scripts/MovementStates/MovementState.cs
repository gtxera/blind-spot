using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementState
{
    protected bool _isActive;
    
    public virtual void DoUpdate()
    {
        
    }

    public virtual void DoFixedUpdate()
    {
        
    }

    public void ActivateMovement(bool isActive)
    {
        _isActive = isActive;
    }

    public bool GetActiveState()
    {
        return _isActive;
    }
}
