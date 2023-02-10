using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/New Dialogue Character")]
public class DialogueCharacterSO : ScriptableObject
{
    public SerializableDictionary<string, Texture> Portraits;
}
