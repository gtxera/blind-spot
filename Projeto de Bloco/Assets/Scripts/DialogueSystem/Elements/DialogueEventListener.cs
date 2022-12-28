using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueEventListener : MonoBehaviour
{
    [SerializeField] DialogueEventSO dialogueEvent;
    [SerializeField] UnityEvent unityEvent;

    public void RaiseEvent() => unityEvent?.Invoke();

}
