using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginQuestOnEvent : MonoBehaviour
{
    [SerializeField] private  EventAndQuest[] _quests;

    private readonly List<GameEventListener> _listeners = new();

    private void OnEnable()
    {
        foreach (var pair in _quests)
        {
            var listener = new GameEventListener(pair.Event, (() =>
            {
                QuestManager.Instance.StartQuest(pair.Quest);
            }));
            
            listener.EnableListener();
            
            _listeners.Add(listener);
        }
    }

    private void OnDisable()
    {
        foreach (var listener in _listeners)
        {
            listener.DisableListener();
        }
    }

    [Serializable]
    private struct EventAndQuest
    {
        public GameEvent Event;
        public Quest Quest;
    }
}
