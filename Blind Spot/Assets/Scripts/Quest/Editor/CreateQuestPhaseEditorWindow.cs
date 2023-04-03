using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Cinemachine.Editor;
using UnityEditor;
using UnityEngine;

public class CreateQuestPhaseEditorWindow : EditorWindow
{
    private SerializedObject _questObject;
    private SerializedProperty _questPhasesProperty;

    private string _questName;
    private string _phaseName = "Enter phase name";
    
    private void OnGUI()
    {
       _phaseName = EditorGUILayout.TextField(_phaseName);

       if (GUILayout.Button("Create Phase"))
       {
           if (!IsValidName())
           {
               EditorUtility.DisplayDialog("Invalid name", "Choose a name that does not exists in the current quest or is not empty",
                   "Ok");
               return;
           }
           
           var path = $"Assets/Quests/Phases/{_questName}";
           var finalPath = Path.Combine(path, $"{_phaseName}.asset");

           if (!AssetDatabase.IsValidFolder(path))
           {
               AssetDatabase.CreateFolder($"Assets/Quests/Phases", _questName);
               
           }
           
           var newPhase = CreateInstance<QuestPhase>();
           newPhase.ParentQuest = _questObject.targetObject as Quest;
           newPhase.name = _phaseName;
           
           AssetDatabase.CreateAsset(newPhase, finalPath);
           AssetDatabase.SaveAssets();
           AssetDatabase.Refresh();

           _questPhasesProperty.arraySize++;
           _questPhasesProperty.GetArrayElementAtIndex(_questPhasesProperty.arraySize - 1).objectReferenceValue = newPhase;
           _questObject.ApplyModifiedProperties();
           Close();
       }
    }

    private bool IsValidName()
    {
        if (!_phaseName.HasAtLeastOneCharacter()) return false;
        
        for (int i = 0; i < _questPhasesProperty.arraySize; i++)
        {
            if (_phaseName == _questPhasesProperty.GetArrayElementAtIndex(i).objectReferenceValue.name)
            {
                return false;
            }
        }

        return true;
    }

    public static void ShowWindow(SerializedObject serializedObject, SerializedProperty questPhasesProperty, string questName)
    {
        CreateQuestPhaseEditorWindow window = (CreateQuestPhaseEditorWindow)GetWindow(typeof(CreateQuestPhaseEditorWindow));
        window._questObject = serializedObject;
        window._questName = questName;
        window._questPhasesProperty = questPhasesProperty;
        window.ShowModal();
    }
}
