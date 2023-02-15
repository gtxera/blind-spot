using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(QuestPhaseWrapper))]
public class QuestPhaseWrapperCustomPropertyDrawer : PropertyDrawer
{
    private bool _initialized;

    private object _questPhase;
    private SerializedObject _phaseSerializedObject;
    
    
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (!_initialized)
        {
            _questPhase = property.FindPropertyRelative("QuestPhase").objectReferenceValue;
            Debug.Log(_questPhase);
            _phaseSerializedObject = new SerializedObject(_questPhase as QuestPhase);

            _initialized = true;
        }
        
        
    }
}
