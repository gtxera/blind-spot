using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine.Editor;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogueSO))]
public class DialogueCustomEditor : Editor
{
    private SerializedProperty _name, _dialogueLines, _hasNameSet;
    private GUIStyle _dialogueNameStyle;
    private List<bool> _shownLines;
    private List<string> _currentLineName;
    
    private void OnEnable()
    {
        _name = serializedObject.FindProperty("Name");
        _dialogueLines = serializedObject.FindProperty("DialogueLines");
        _hasNameSet = serializedObject.FindProperty("HasNameSet");
        
        _shownLines = new List<bool>();
        _currentLineName = new List<string>();
        
        for (int i = 0; i < _dialogueLines.arraySize; i++)
        {
            _shownLines.Add(false);
            var line = _dialogueLines.GetArrayElementAtIndex(i).objectReferenceValue as DialogueLineSO;
            _currentLineName.Add(line.LineIdentifier);
        }

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

        var defaultColor = GUI.backgroundColor;

        if (!_hasNameSet.boolValue)
        {
            EditorStyles.textField.alignment = TextAnchor.UpperLeft;
            EditorGUILayout.PropertyField(_name);
            if (GUILayout.Button("Set name"))
            {
                if (!CheckIfIsValidName(_name.stringValue))
                {
                    EditorUtility.DisplayDialog("Empty dialogue name", "Enter a valid name", "Ok");
                    return;
                }
                _hasNameSet.boolValue = true;
                serializedObject.ApplyModifiedProperties();
                var path = AssetDatabase.GetAssetPath(serializedObject.targetObject);
                AssetDatabase.RenameAsset(path, _name.stringValue);
                AssetDatabase.Refresh();
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
                    dialogueLine.CreateWrapper();
                }

                var color = dialogueLine switch
                {
                    DialogueCharacterSimpleLine => new Color(0.36f, 0.33f, 0.01f),
                    DialogueCharacterChoiceLine => new Color(0.4f, 0.25f, 0.01f),
                    DialogueSimpleLine => new Color(0.31f, 0.08f, 0.02f),
                    DialogueChoiceLine => new Color(0.4f, 0.01f, 0.31f),
                    DialogueEventLine => new Color(0.12f, 0f, 0.36f),
                    _ => GUI.backgroundColor
                };

                GUI.backgroundColor = color;

                var lineObject = new SerializedObject(dialogueLine);
                var wrapper = lineObject.FindProperty("Wrapper");
                
                Rect backgroundRect = EditorGUILayout.BeginVertical();
                backgroundRect.y += 5f;
                EditorGUI.DrawRect(backgroundRect,color);
                
                Rect labelRect = EditorGUILayout.BeginHorizontal();
                
                EditorStyles.foldoutHeader.fixedWidth = 13;
                _shownLines[i] = EditorGUILayout.BeginFoldoutHeaderGroup(_shownLines[i], GUIContent.none);

                EditorStyles.textField.alignment = TextAnchor.MiddleCenter;
                var lineIdentifier = lineObject.FindProperty("LineIdentifier");
                EditorGUI.DelayedTextField(labelRect, lineIdentifier, GUIContent.none);
                
                if (CheckIfIsValidName(lineIdentifier.stringValue) && lineIdentifier.stringValue != _currentLineName[i])
                {
                    RenameLine(dialogueLine, lineIdentifier.stringValue, i);
                    lineObject.FindProperty("m_Name").stringValue = lineIdentifier.stringValue;
                }

                EditorStyles.textArea.alignment = TextAnchor.UpperLeft;
                
                EditorGUILayout.EndHorizontal();
                
                GUI.backgroundColor = defaultColor;
                if (_shownLines[i])
                {
                    
                    EditorGUILayout.PropertyField(wrapper);

                    EditorGUILayout.Separator();
                    
                    GUI.backgroundColor = Color.red;
                    if (GUILayout.Button("Remove line"))
                    {
                        for (int j = i; j < _dialogueLines.arraySize - 1; j++)
                        {
                            _dialogueLines.GetArrayElementAtIndex(j).objectReferenceValue =
                                _dialogueLines.GetArrayElementAtIndex(j + 1).objectReferenceValue;
                            _shownLines[j] = _shownLines[j + 1];
                        }

                        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(dialogueLine.GetInstanceID()));
                        _dialogueLines.arraySize--;
                        continue;
                    }
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndFoldoutHeaderGroup();

                lineObject.ApplyModifiedProperties();                
                EditorGUILayout.Separator();
            }

            GUI.backgroundColor = defaultColor;
            if (GUILayout.Button("Add Line"))
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("Simple Line"), false, CreateLine, CreateInstance<DialogueSimpleLine>());
                menu.AddItem(new GUIContent("Choice Line"), false, CreateLine, CreateInstance<DialogueChoiceLine>());
                menu.AddItem(new GUIContent("Character Simple Line"), false, CreateLine, CreateInstance<DialogueCharacterSimpleLine>());
                menu.AddItem(new GUIContent("Character Choice Line"), false, CreateLine, CreateInstance<DialogueCharacterChoiceLine>());
                menu.AddItem(new GUIContent("Event Line"), false, CreateLine, CreateInstance<DialogueEventLine>());
                menu.ShowAsContext();
            }
        }

        EditorStyles.textField.alignment = TextAnchor.UpperLeft;
        EditorStyles.label.fontSize = 12;
        EditorStyles.foldoutHeader.fixedWidth = 0;
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

        var line = newLine as DialogueLineSO;
        line.ParentDialogue = serializedObject.targetObject as DialogueSO;
        
        AssetDatabase.CreateAsset(line, finalPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        var lines = serializedObject.FindProperty("DialogueLines");
        line.LineIdentifier = $"{lines.arraySize}";
        lines.arraySize++;
        lines.GetArrayElementAtIndex(lines.arraySize - 1).objectReferenceValue = newLine as UnityEngine.Object;
        
        try
        {
            _shownLines[lines.arraySize - 1] = true;
        }
        catch (ArgumentOutOfRangeException)
        {
            _shownLines.Add(true);
        }
        
        try
        {
            _currentLineName[lines.arraySize - 1] = line.LineIdentifier;
        }
        catch (ArgumentOutOfRangeException)
        {
            _currentLineName.Add(line.LineIdentifier);
        }
        
        serializedObject.ApplyModifiedProperties();
    }

    private void RenameLine(DialogueLineSO lineToRename, string newName, int index)
    {
        AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(lineToRename.GetInstanceID()), newName);
        lineToRename.name = newName;
        AssetDatabase.Refresh();
        _currentLineName[index] = newName;
    }

    private bool CheckIfIsValidName(string name)
    {
        return name.Any(Char.IsLetter);
    }
}
