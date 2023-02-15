using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Cinemachine.Editor;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Quest))]
public class QuestEditor : Editor
{

    private SerializedProperty _nameProperty;
    private SerializedProperty _isMandatoryProperty;
    private SerializedProperty _questPhasesProperty;

    private List<bool> _shownQuests = new List<bool>();

    private void OnEnable()
    {
        _nameProperty = serializedObject.FindProperty("m_Name");
        _questPhasesProperty = serializedObject.FindProperty("QuestPhases");
        _isMandatoryProperty = serializedObject.FindProperty("IsMandatory");

        for (int i = 0; i < _questPhasesProperty.arraySize; i++)
        {
            _shownQuests.Add(false);
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        EditorStyles.label.fontSize = 18;
        EditorStyles.label.alignment = TextAnchor.MiddleCenter;

        EditorGUILayout.LabelField(_nameProperty.stringValue);
        
        EditorStyles.label.fontSize = 12;
        EditorStyles.label.alignment = TextAnchor.UpperLeft;
        
        for (int i = 0; i < _questPhasesProperty.arraySize; i++)
        {
            var phaseProperty = _questPhasesProperty.GetArrayElementAtIndex(i);
            var phaseSO = phaseProperty.objectReferenceValue as QuestPhase;
            var wrapper = phaseSO.Wrapper;
            Debug.Log(wrapper);
            if(wrapper.QuestPhase == null) phaseSO.CreateWrapper();

            var phaseObject = new SerializedObject(phaseSO);
            var wrapperProperty = phaseObject.FindProperty("Wrapper");

            Rect nameRect = EditorGUILayout.BeginHorizontal();
            
            EditorStyles.foldoutHeader.fixedWidth = 13;
            _shownQuests[i] = EditorGUILayout.BeginFoldoutHeaderGroup(_shownQuests[i], GUIContent.none);

            EditorStyles.textField.alignment = TextAnchor.MiddleCenter;
            var phaseNameProperty = phaseObject.FindProperty("m_Name");
            EditorGUI.DelayedTextField(nameRect, phaseNameProperty, GUIContent.none);
            
            EditorGUILayout.EndHorizontal();    
            
            if (_shownQuests[i])
            {
                EditorGUILayout.PropertyField(wrapperProperty);
            }
            
            EditorGUILayout.Separator();

            phaseObject.ApplyModifiedProperties();
        }
        
        

        if (GUILayout.Button("Add Phase"))
        {
            CreatePhase();
            _shownQuests.Add(true);
        }

        EditorStyles.label.fontSize = 12;
        EditorStyles.label.alignment = TextAnchor.UpperLeft;
        EditorStyles.textField.alignment = TextAnchor.UpperLeft;
        EditorStyles.foldoutHeader.fixedWidth = 0;

        serializedObject.ApplyModifiedProperties();
    }

    private void CreatePhase()
    {
        CreateQuestPhaseEditorWindow.ShowWindow(serializedObject, _questPhasesProperty, _nameProperty.stringValue);
    }
}
