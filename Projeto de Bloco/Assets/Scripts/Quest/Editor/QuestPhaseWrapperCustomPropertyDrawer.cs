using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine.Editor;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = System.Object;

[CustomPropertyDrawer(typeof(QuestPhaseWrapper))]
public class QuestPhaseWrapperCustomPropertyDrawer : PropertyDrawer
{
    private bool _initialized;
    
    private QuestPhase _questPhase;
    private List<SerializableDictionaryWrapper> _dictionaryWrappers = new();
    private List<float> _listElementsHeight = new();
    private ReorderableList _nextPhasesList;
    private SerializedObject _phaseSerializedObject;
    private Quest _parentQuest;

    private static QuestPhase _defaultQuestPhase = AssetDatabase.LoadAssetAtPath<QuestPhase>("Assets/Quests/Phases/default.asset");

    private const float DEFAULT_SPACE = 5.0f;
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 120;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (!_initialized)
        {

            var questPhase = property.FindPropertyRelative("QuestPhase").objectReferenceValue as QuestPhase;

            foreach (var key in questPhase.NextPhases.Keys)
            {
                var wrapper = new SerializableDictionaryWrapper(questPhase, key, questPhase.NextPhases[key]);
                
                _dictionaryWrappers.Add(wrapper);
                _listElementsHeight.Add(0f);
            }
            
            _nextPhasesList = new ReorderableList(_dictionaryWrappers, typeof(SerializableDictionaryWrapper))
            {
                drawHeaderCallback = NextPhasesListHeader,
                drawElementCallback = DrawNextPhasesElements,
                elementHeightCallback = GetNextPhasesElementHeight,
                onRemoveCallback = RemoveNextPhase,
                onAddCallback = AddNextPhase
            };

            _initialized = true;
        }

        _questPhase = property.FindPropertyRelative("QuestPhase").objectReferenceValue as QuestPhase;
        _phaseSerializedObject = new SerializedObject(_questPhase);
        _parentQuest = _phaseSerializedObject.FindProperty("ParentQuest").objectReferenceValue as Quest;

        EditorGUI.BeginProperty(position, label, property);

        float currentHeight = 0;

        var phaseTextProperty = _phaseSerializedObject.FindProperty("PhaseText");
        var phaseTextRect = new Rect(position.x, position.y, position.width,
            EditorGUI.GetPropertyHeight(phaseTextProperty));

        EditorStyles.label.alignment = TextAnchor.MiddleCenter;
        EditorStyles.textArea.alignment = TextAnchor.UpperLeft;
        EditorGUI.PropertyField(phaseTextRect, phaseTextProperty);

        currentHeight += EditorGUI.GetPropertyHeight(phaseTextProperty) + DEFAULT_SPACE;

        var phaseWaypointProperty = _phaseSerializedObject.FindProperty("PhaseWaypointName");
        var phaseWaypointRect = new Rect(position.x, position.y + currentHeight, position.width, EditorGUIUtility.singleLineHeight);

        EditorStyles.textField.alignment = TextAnchor.UpperLeft;
        EditorGUI.PropertyField(phaseWaypointRect, phaseWaypointProperty);

        if (!phaseWaypointProperty.stringValue.HasAtLeastOneCharacter())
        {
            phaseWaypointProperty.stringValue = "";
        }
        
        currentHeight += EditorGUI.GetPropertyHeight(phaseTextProperty) + DEFAULT_SPACE;
        
        _nextPhasesList.DoLayoutList();
        
        _phaseSerializedObject.ApplyModifiedProperties();
        EditorGUI.EndProperty();
        
        EditorStyles.label.alignment = TextAnchor.UpperLeft;
    }

    private void NextPhasesListHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, "Next Phases");
    }

    private void DrawNextPhasesElements(Rect rect, int index, bool isActive, bool isFocused)
    {
        float currentHeight = DEFAULT_SPACE;
        
        var nextPhase = _nextPhasesList.list[index] as SerializableDictionaryWrapper;

        EditorStyles.label.alignment = TextAnchor.MiddleCenter;
        EditorGUI.LabelField(new Rect(rect.x, rect.y + currentHeight, rect.width, EditorGUIUtility.singleLineHeight), "Event");

        currentHeight += EditorGUIUtility.singleLineHeight + DEFAULT_SPACE;
        
        nextPhase.GameEvent = EditorGUI.ObjectField(new Rect(rect.x, rect.y + currentHeight, rect.width, EditorGUIUtility.singleLineHeight),
            nextPhase.GameEvent, typeof(GameEvent), false) as GameEvent;

        currentHeight += EditorGUIUtility.singleLineHeight + DEFAULT_SPACE;

        if (nextPhase.GameEvent != null)
        {
            EditorGUI.LabelField(new Rect(rect.x, rect.y + currentHeight, rect.width, EditorGUIUtility.singleLineHeight), "Next Phase");

            currentHeight += EditorGUIUtility.singleLineHeight + DEFAULT_SPACE;
            
            if (nextPhase.NextPhase != null)
            {
                EditorGUI.ObjectField(
                    new Rect(rect.x, rect.y + currentHeight, rect.width, EditorGUIUtility.singleLineHeight),
                    nextPhase.NextPhase, typeof(QuestPhase), false);

                currentHeight += EditorGUIUtility.singleLineHeight + DEFAULT_SPACE;
            }
            
            DrawNextPhaseSelector(new Rect(rect.x, rect.y + currentHeight, rect.width, EditorGUIUtility.singleLineHeight), nextPhase);
        }

        currentHeight += EditorGUIUtility.singleLineHeight + DEFAULT_SPACE;

        _listElementsHeight[index] = currentHeight;
        
        EditorStyles.label.alignment = TextAnchor.UpperLeft;
    }

    private void DrawNextPhaseSelector(Rect rect, SerializableDictionaryWrapper wrapper)
    {
        if (EditorGUI.DropdownButton(rect, new GUIContent("Choose Next Phase"), FocusType.Passive))
        {
            var menu = new GenericMenu();

            menu.AddItem(new GUIContent("Finish this quest"), false, data => wrapper.NextPhase = data as QuestPhase, QuestUtility.FinishThisQuest);
            
            foreach (var questPhase in _parentQuest.QuestPhases.Where(questPhase => questPhase.name != _questPhase.name))
            {
                menu.AddItem(new GUIContent(questPhase.name), false, data => wrapper.NextPhase = data as QuestPhase, questPhase);
            }
            
            menu.ShowAsContext();
        }
    }
    
    private void AddNextPhase(ReorderableList list)
    {
        var newPhase = new SerializableDictionaryWrapper(_questPhase);
        list.list.Add(newPhase);
        _listElementsHeight.Add(0f);
    }

    private void RemoveNextPhase(ReorderableList list)
    {
        var indices = list.selectedIndices;

        if (indices.Count == 0)
        {
            var wrapper = list.list[^1] as SerializableDictionaryWrapper;

            RemoveNextPhaseFromObject(wrapper);
            
            list.list.RemoveAt(list.list.Count -1);
            return;
        }
        
        foreach (var index in indices)
        {
            var wrapper = list.list[index] as SerializableDictionaryWrapper;

            RemoveNextPhaseFromObject(wrapper);
            
            list.list.RemoveAt(index);
        }
    }

    private void RemoveNextPhaseFromObject(SerializableDictionaryWrapper wrapper)
    {
        if (wrapper.NextPhase == null) return;
        
        wrapper.Phase.NextPhases.Remove(wrapper.GameEvent);
    }

    private float GetNextPhasesElementHeight(int index)
    {
        return _listElementsHeight[index];
    }
    
    private class SerializableDictionaryWrapper
    {
        public GameEvent GameEvent
        {
            get => _gameEvent;

            set
            {
                if (value == _gameEvent || value == null) return;
                
                _gameEvent = value;
            }
        }

        private GameEvent _gameEvent;

        public QuestPhase NextPhase
        {
            get => _nextPhase;

            set
            {
                if (value == null  ||value == _nextPhase || _gameEvent == null) return;

                Phase.NextPhases.Remove(_gameEvent);
                Phase.NextPhases.Add(_gameEvent, value);
                _nextPhase = value;

            }
        }

        private QuestPhase _nextPhase;
        
        public readonly QuestPhase Phase;

        public SerializableDictionaryWrapper(QuestPhase phase, GameEvent gameEvent = null, QuestPhase nextPhase = null)
        {
            Phase = phase;
            _gameEvent = gameEvent;
            _nextPhase = nextPhase;
        }
    }
}
