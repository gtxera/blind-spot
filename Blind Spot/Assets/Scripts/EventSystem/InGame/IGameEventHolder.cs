using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameEventHolder
{
    GameEvent Event { get; set; }
}
