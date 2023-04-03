using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteRoad : MonoBehaviour
{
    [SerializeField] private GameObject[] _groups;
    [SerializeField] private GameObject _player;

    [SerializeField] private TravelCountEvent[] _travelCountEvents;

    private const float DISTANCE_BETWEEN_ROADS = 41.8F;

    private float _roadSize;

    private int _currentRoadTravel;

    private int CurrentRoadTravel
    {
        get => _currentRoadTravel;

        set
        {
            _currentRoadTravel = value;
            foreach (var travelCountEvent in _travelCountEvents)
            {
                if (value == travelCountEvent.TravelCount)
                {
                    travelCountEvent.Event.RaiseEvent();
                } 
            }
        }
    }

    private GameObject _currentGroup;

    private void Update()
    {
        foreach (var group in _groups)
        {
            if(group == _currentGroup) continue;
            
            if (_player.transform.position.z > group.transform.position.z - DISTANCE_BETWEEN_ROADS / 2 &&
                _player.transform.position.z < group.transform.position.z + DISTANCE_BETWEEN_ROADS / 2)
            {
                _currentGroup = group;
                
                var index = Array.IndexOf(_groups, group);
                
                if (index == 1)
                {
                    var cachedTransformPos = _groups[4].transform.position;
                    
                    _groups[4].transform.position = new Vector3(cachedTransformPos.x, cachedTransformPos.y ,_groups[0].transform.position.z + DISTANCE_BETWEEN_ROADS);
                    _groups.Move(4, 0);
                    CurrentRoadTravel += 1;
                }
                else if (index == 3)
                {
                    var cachedTransformPos = _groups[0].transform.position;
                    
                    _groups[0].transform.position = new Vector3(cachedTransformPos.x, cachedTransformPos.y ,_groups[4].transform.position.z - DISTANCE_BETWEEN_ROADS);
                    _groups.Move(0, 4);
                    CurrentRoadTravel += 1;
                }
                
                break;
            }
        }
    }

    [Serializable]
    public struct TravelCountEvent
    {
        public int TravelCount;
        public GameEvent Event;
    }
}
