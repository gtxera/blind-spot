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

        public List<(Type, bool)> MonoBehaviourStates;

        public List<GameObjectState> ChildObjectsState;
    }
    
    public int SceneId;

    public List<GameObjectState> GameObjectStates;

    public SaveData(Scene activeScene)
    {
        SceneId = activeScene.buildIndex;

        GameObjectStates = new List<GameObjectState>();

        foreach (var rootObject in activeScene.GetRootGameObjects())
        {
            GameObjectStates.Add(CreateGameObjectStatesRecursively(rootObject));
        }
    }

    private GameObjectState CreateGameObjectStatesRecursively(GameObject gameObject)
    {
        var monoBehaviourStates = new List<(Type, bool)>();

        foreach (var monoBehaviour in gameObject.GetComponents<Behaviour>())
        {
            monoBehaviourStates.Add((monoBehaviour.GetType(), monoBehaviour.enabled));
            Debug.Log(monoBehaviour.GetType());
        }

        var state = new GameObjectState
        {
            GameObjectName = gameObject.name,
            IsActive = gameObject.activeSelf,
            MonoBehaviourStates = monoBehaviourStates,
            ChildObjectsState = new List<GameObjectState>()
        };

        foreach (Transform child in gameObject.transform)
        {
            state.ChildObjectsState.Add(CreateGameObjectStatesRecursively(child.gameObject));
        }

        return state;
    }
}


