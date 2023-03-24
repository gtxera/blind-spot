using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

public class SaveData
{
    [Serializable]
    public struct GameObjectState
    {
        public uint GameObjectId;

        public bool IsActive;

        public Vector3 Position;
        
        public List<BehaviourState> MonoBehaviourStates;
    }

    [Serializable]
    public struct BehaviourState
    {
        public string Name;
        public bool Enabled;
    }
    
    public int SceneId;

    public List<GameObjectState> GameObjectStates;
    
    public SaveData(Scene activeScene)
    {
        SceneId = activeScene.buildIndex;

        GameObjectStates = new List<GameObjectState>();

        foreach (var saveObject in Object.FindObjectsOfType<SaveObjectID>(true))
        {
            Debug.Log(saveObject.name);
            GameObjectStates.Add(CreateGameObjectState(saveObject.gameObject, saveObject.ID));
        }
    }

    private GameObjectState CreateGameObjectState(GameObject gameObject, uint saveId)
    {
        var monoBehaviourStates = new List<BehaviourState>();

        foreach (var monoBehaviour in gameObject.GetComponents<Behaviour>())
        {
            monoBehaviourStates.Add(new BehaviourState
            {
                Name = monoBehaviour.GetType().Name,
                Enabled = monoBehaviour.enabled
            });
        }

        var state = new GameObjectState
        {
            GameObjectId = saveId,
            IsActive = gameObject.activeSelf,
            Position = gameObject.transform.position,
            MonoBehaviourStates = monoBehaviourStates,
        };

        return state;
    }
}


