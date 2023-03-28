using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaiseEventCall : MonoBehaviour
{
    [SerializeField] private GameEvent _gameEvent;

    public void RaiseEvent()
    {
        _gameEvent.RaiseEvent();
    }
}
