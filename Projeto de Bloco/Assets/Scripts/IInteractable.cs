using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    private static readonly int HasOutline = Shader.PropertyToID("_HasOutline");

    Material InteractableMaterial
    {
        get;
    }
    
    void Interact(GameObject playerObject);

    void SetOutlines(bool isActive)
    {
        InteractableMaterial.SetFloat(HasOutline, Convert.ToInt32(isActive));
    }
}
