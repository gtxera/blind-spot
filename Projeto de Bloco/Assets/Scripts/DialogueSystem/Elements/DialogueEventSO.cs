using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEventSO : ScriptableObject
{
    public virtual void RunEvent()
    {
        Debug.Log("Event called");
    }
}
