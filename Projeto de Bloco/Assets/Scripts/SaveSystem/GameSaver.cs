using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameSaver : SingletonBehaviour<GameSaver>
{
    private string _saveId = "a";

    private UnityAction<Scene, LoadSceneMode> _loadGameObjectsCallback;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void OnDisable()
    {
        TrySave(SceneManager.GetActiveScene());
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= TrySave;
    }

    private void TrySave(Scene scene, LoadSceneMode mode = LoadSceneMode.Single)
    {
        if (scene.buildIndex != 0) Save();
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
            File.Create(savePath).Close();
            //File.SetAttributes(savePath, FileAttributes.Hidden);
        }

        var saveData = new SaveData(SceneManager.GetActiveScene());

        var serializedData = JsonUtility.ToJson(saveData);
        
        File.WriteAllText(savePath, serializedData);
        
        PlayerPrefs.SetString("LastSave", _saveId);
    }

    public void Load()
    {
        var savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), $@"Blind Spot\{_saveId}.bspt");

        if (!File.Exists(savePath)) return;

        var saveDataJson = File.ReadAllText(savePath);

        var saveData = JsonUtility.FromJson<SaveData>(saveDataJson);

        _loadGameObjectsCallback = delegate(Scene _, LoadSceneMode _) { LoadGameObjects(saveData); };
        SceneManager.sceneLoaded += _loadGameObjectsCallback;
        
        SceneManager.LoadScene(saveData.SceneId);
    }

    private void LoadGameObjects(SaveData saveData)
    {
        var ids = FindObjectsOfType<SaveObjectID>(true);

        foreach (var state in saveData.GameObjectStates)
        {
            var savedObject = ids.First(instance => instance.ID == state.GameObjectId).gameObject;

            savedObject.SetActive(state.IsActive);

            if (savedObject.TryGetComponent<CharacterController>(out var charCtrl))
            {
                charCtrl.enabled = false;
                savedObject.transform.position = state.Position;
            }
            else savedObject.transform.position = state.Position;

            foreach (var behaviourState in state.MonoBehaviourStates)
            {
                (savedObject.GetComponent(behaviourState.Name) as Behaviour)!.enabled = behaviourState.Enabled;
            }

            SceneManager.sceneLoaded -= _loadGameObjectsCallback;
            SceneManager.sceneLoaded += TrySave;
        }
    }

    private Dictionary<float, GameObject> GatherIds(GameObject gameObject)
    {
        Dictionary<float, GameObject> ids = new(){ {gameObject.transform.position.sqrMagnitude, gameObject}};

        foreach (Transform child in gameObject.transform)
        {
            var childIds = GatherIds(child.gameObject);

            foreach (var pair in childIds)
            {
                ids.Add(pair.Key, pair.Value);
            }
        }

        return ids;
    }
}
