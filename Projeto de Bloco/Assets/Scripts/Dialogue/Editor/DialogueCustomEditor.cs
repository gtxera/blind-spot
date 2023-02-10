using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine.Editor;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogueSO))]
public class DialogueCustomEditor : Editor
{
    private SerializedProperty _name, _dialogueLines, _hasNameSet;
    private GUIStyle _dialogueNameStyle, _removeLineButtonStyle, _addLineButtonStyle;
    
    private void OnEnable()
    {
        _name = serializedObject.FindProperty("Name");
        _dialogueLines = serializedObject.FindProperty("DialogueLines");

        _hasNameSet = serializedObject.FindProperty("HasNameSet");
        
        _dialogueNameStyle = new GUIStyle
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = 18,
            normal =
            {
                textColor = Color.white
            }
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (!_hasNameSet.boolValue)
        {
            EditorGUILayout.PropertyField(_name);
            if (GUILayout.Button("Set name"))
            {
                _hasNameSet.boolValue = true;
                serializedObject.ApplyModifiedProperties();
            }
        }
        
        else
        {
            EditorGUILayout.LabelField(_name.stringValue, _dialogueNameStyle);
            EditorGUILayout.Separator();

            for (int i = 0; i < _dialogueLines.arraySize; i++)
            {
                var line = _dialogueLines.GetArrayElementAtIndex(i);
                var dialogueLine = line.objectReferenceValue as DialogueLineSO;
                var wrapperReference = dialogueLine.Wrapper;
                if (wrapperReference.DialogueLineSO == null)
                {
                    var lineObj = line.objectReferenceValue as DialogueLineSO;
                    lineObj.CreateWrapper();
                }

                var wrapper = new SerializedObject(dialogueLine).FindProperty("Wrapper");
                EditorGUILayout.PropertyField(wrapper);
                
                if (GUILayout.Button("Remove line"))
                {
                    for (int j = i; j < _dialogueLines.arraySize - 1; j++)
                    {
                        _dialogueLines.GetArrayElementAtIndex(j).objectReferenceValue =
                            _dialogueLines.GetArrayElementAtIndex(j + 1).objectReferenceValue;
                    }

                    _dialogueLines.arraySize--;
                }
                EditorGUILayout.Separator();
            }

            if (GUILayout.Button("Add Line"))
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("Simple Line"), false, CreateLine, CreateInstance<DialogueSimpleLine>());
                menu.AddItem(new GUIContent("Choice Line"), false, CreateLine, CreateInstance<DialogueChoiceLine>());
                menu.AddItem(new GUIContent("Character Simple Line"), false, CreateLine, CreateInstance<DialogueCharacterSimpleLine>());
                menu.AddItem(new GUIContent("Character Choice Line"), false, CreateLine, CreateInstance<DialogueCharacterChoiceLine>());
                menu.AddItem(new GUIContent("Event Line"), false, CreateLine, CreateInstance<DialogueEventLine>());
                menu.DropDown(GUILayoutUtility.GetLastRect());
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void CreateLine(object newLine)
    {
        var parentPath = $"Assets/Dialogue/Lines/{_name.stringValue}";
        var finalPath = $"Assets/Dialogue/Lines/{_name.stringValue}/{_dialogueLines.arraySize}.asset";

        if (!AssetDatabase.IsValidFolder(parentPath))
        {
            AssetDatabase.CreateFolder("Assets/Dialogue/Lines",$"{_name.stringValue}");
        }
        
        AssetDatabase.CreateAsset(newLine as UnityEngine.Object, finalPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        var lines = serializedObject.FindProperty("DialogueLines");
        lines.arraySize++;
        lines.GetArrayElementAtIndex(lines.arraySize - 1).objectReferenceValue = newLine as UnityEngine.Object;
        serializedObject.ApplyModifiedProperties();
    }
}
