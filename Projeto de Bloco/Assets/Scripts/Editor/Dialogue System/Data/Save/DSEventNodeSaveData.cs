using System;
using System.Collections.Generic;
using UnityEngine;

namespace DS.Data.Save
{
    [Serializable]
    public class DSEventNodeSaveData : DSNodeSaveData
    {
        [field: SerializeField] public DialogueEventSO DialogueEvent { get; set; }
    }
}



