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
    private ReorderableList _nextPhasesList;
    private SerializedObject _phaseSerializedObject;
    private Quest _parentQuest;

    private static QuestPhase _defaultQuestPhase = AssetDatabase.LoadAssetAtPath<QuestPhase>("Assets/Quests/Phases/default.asset");

    private const float DEFAULT_SPACE = 5.0f;
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 100;
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
            }
            
            _nextPhasesList = new ReorderableList(_dictionaryWrappers, typeof(SerializableDictionaryWrapper))
            {
                drawHeaderCallback = NextPhasesListHeader,
                drawElementCallback = DrawNextPhasesElements,
                elementHeightCallback = GetNextPhasesElementHeight,
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

        EditorGUI.PropertyField(phaseTextRect, phaseTextProperty);

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
        float currentHeight = 0;
        
        var nextPhase = _nextPhasesList.list[index] as SerializableDictionaryWrapper;

        nextPhase.GameEvent = EditorGUI.ObjectField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
            nextPhase.GameEvent, typeof(GameEvent), false) as GameEvent;

        currentHeight += EditorGUIUtility.singleLineHeight + DEFAULT_SPACE;

        if (nextPhase.GameEvent != null)
        {
            DrawNextPhaseSelector(new Rect(rect.x, rect.y + currentHeight, rect.width, EditorGUIUtility.singleLineHeight), nextPhase);
        }
    }

    private void DrawNextPhaseSelector(Rect rect, SerializableDictionaryWrapper wrapper)
    {
        if (EditorGUI.DropdownButton(rect, new GUIContent("Choose Next Quest"), FocusType.Passive))
        {
            var menu = new GenericMenu();

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
    }

    private float GetNextPhasesElementHeight(int index)
    {
        return 100;
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

                _phase.NextPhases.Remove(_gameEvent);
                _phase.NextPhases.Add(_gameEvent, value);
                _nextPhase = value;

            }
        }

        private QuestPhase _nextPhase;
        
        private readonly QuestPhase _phase;

        public SerializableDictionaryWrapper(QuestPhase phase, GameEvent gameEvent = null, QuestPhase nextPhase = null)
        {
            _phase = phase;
            _gameEvent = gameEvent;
            _nextPhase = nextPhase;
        }
    }
}
