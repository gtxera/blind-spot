using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveData
{
    [Serializable]
    public struct GameObjectState
    {
        public string GameObjectName;

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

        foreach (var rootObject in activeScene.GetRootGameObjects())
        {
            if (rootObject.TryGetComponent<RectTransform>(out _)) continue;
            
            GameObjectStates.Add(CreateGameObjectStatesRecursively(rootObject));
        }
    }

    private GameObjectState CreateGameObjectStatesRecursively(GameObject gameObject)
    {
        Debug.Log($"{gameObject.name} {gameObject.transform.position.sqrMagnitude}");
        
        var monoBehaviourStates = new List<BehaviourState>();

        foreach (var monoBehaviour in gameObject.GetComponents<Behaviour>())
        {
            monoBehaviourStates.Add(new BehaviourState
            {
                Name = monoBehaviour.GetType().Name,
                Enabled = monoBehaviour.enabled
            });
            Debug.Log(monoBehaviour.GetType().ToString());
        }

        var state = new GameObjectState
        {
            GameObjectName = gameObject.name,
            IsActive = gameObject.activeSelf,
            Position = gameObject.transform.position,
            MonoBehaviourStates = monoBehaviourStates,
        };

        foreach (Transform child in gameObject.transform)
        {
            if (gameObject.TryGetComponent<RectTransform>(out _)) continue;

            GameObjectStates.Add(CreateGameObjectStatesRecursively(child.gameObject));
        }

        return state;
    }
}


