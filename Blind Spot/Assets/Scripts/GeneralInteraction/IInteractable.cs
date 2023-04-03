using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IInteractable
{
    void Interact(GameObject playerObject);

    void SetOutlines(bool isActive);
}
