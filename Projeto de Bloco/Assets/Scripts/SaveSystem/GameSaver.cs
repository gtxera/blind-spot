using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSaver : SingletonBehaviour<GameSaver>
{
    private string _saveId = "a";

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

        var savePath = Path.Combine(mainPath, $"{_saveId}.txt");
        
        if (!File.Exists(savePath))
        {
            var saveFile = File.Create(savePath);
            //File.SetAttributes(savePath, FileAttributes.Hidden);
        }

        var saveData = new SaveData(SceneManager.GetActiveScene());

        var serializer = new JsonSerializer
        {
            NullValueHandling = NullValueHandling.Ignore,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
            TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple
        };

        var writer = new StreamWriter(savePath);
        var jsonWriter = new JsonTextWriter(writer);
        
        serializer.Serialize(jsonWriter, saveData);
    }
}
