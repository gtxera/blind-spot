using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DialogueLineSOWrapper))]
public class DialogueSOWrapperCustomPropertyDrawer : PropertyDrawer
{
    private object _dialogeSO;
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (_dialogeSO == null)
        {
            _dialogeSO = property.FindPropertyRelative("DialogueLineSO").objectReferenceValue;
        }
    }
}
