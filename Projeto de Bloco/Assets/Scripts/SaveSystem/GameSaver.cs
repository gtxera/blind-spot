using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSaver : SingletonBehaviour<GameSaver>
{
    private string _saveId = "a";

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SetSaveId(string id)
    {
        _saveId = id;
    }

    public void Save()
    {
        var mainPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Blind Spot";

        if (!Directory.Exists(mainPath))
        {
            Directory.CreateDirectory(mainPath);
        }

        var savePath = Path.Combine(mainPath, $"{_saveId}.bspt");
        
        if (!File.Exists(savePath))
        {
            var saveFile = File.Create(savePath);
            //File.SetAttributes(savePath, FileAttributes.Hidden);
        }

        var saveData = new SaveData(SceneManager.GetActiveScene());

        var serializedData = JsonUtility.ToJson(saveData);
        
        File.WriteAllText(savePath, serializedData);
    }

    public void Load()
    {
        var savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), $@"Blind Spot\{_saveId}.bspt");

        if (!File.Exists(savePath)) return;

        var saveDataJson = File.ReadAllText(savePath);

        var saveData = JsonUtility.FromJson<SaveData>(saveDataJson);

        foreach (var state in saveData.GameObjectStates)
        {
            var savedObject = GameObject.Find(state.GameObjectName);

            savedObject.SetActive(state.IsActive);
            savedObject.transform.position = state.Position;

            foreach (var behaviourState in state.MonoBehaviourStates)
            {
                (savedObject.GetComponent(behaviourState.Name) as Behaviour)!.enabled = behaviourState.Enabled;
            }
        }

    }
}
