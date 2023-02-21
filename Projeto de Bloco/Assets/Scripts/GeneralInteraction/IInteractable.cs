using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IInteractable
{
    private static readonly int HasOutline = Shader.PropertyToID("_HasOutline");

    Material[] InteractableMaterials
    {
        get;
        set;
    }
    
    void Interact(GameObject playerObject);

    void SetOutlines(bool isActive)
    {
        if(!InteractableMaterials.Any()) return;

        foreach (var material in InteractableMaterials)
        {
            material.SetFloat(HasOutline, Convert.ToInt32(isActive));    
        }
    }
}
