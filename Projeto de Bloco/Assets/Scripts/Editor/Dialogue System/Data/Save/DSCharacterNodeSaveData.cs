using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DS.Data.Save
{
    [Serializable]
    public class DSCharacterNodeSaveData : DSNodeSaveData
    {
        [field: SerializeField] public string CharacterName { get; set; }
        [field: SerializeField] public Sprite CharacterSprite { get; set; }
    }
}