using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEventValidator : MonoBehaviour
{
    public bool PlayerInRange { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerAnimations>() != null)
        {
            PlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerAnimations>() != null)
        {
            PlayerInRange = false;
        }
    }
}
