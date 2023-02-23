using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
    public Sprite ItemImage;

    public bool Stackable;

    public GameEvent ItemEvent;

}
