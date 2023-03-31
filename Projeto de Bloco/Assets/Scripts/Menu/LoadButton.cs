using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadButton : MonoBehaviour
{
    private string _saveName;

    public void SetSaveId(string id)
    {
        _saveName = id;
    }

    public void Load()
    {
        GameSaver.Instance.SetSaveId(_saveName);
        GameSaver.Instance.Load();
    }
}
