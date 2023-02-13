using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine.Editor;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomPropertyDrawer(typeof(DialogueLineSOWrapper))]
public class DialogueSOWrapperCustomPropertyDrawer : PropertyDrawer
{
    private object _dialogeLineSO;

    private const float DEFAULT_SPACE = 5f;
    private bool _initialized;
    private SerializedObject _serializedLine;
    private ReorderableList _choicesList;
    private List<float> _elementsHeights;
    private float _nonBuggedWidth = 0;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = 0;

        if (_dialogeLineSO is DialogueCharacterSimpleLine or DialogueCharacterChoiceLine)
            height += GetCharacterLineHeight() + EditorGUIUtility.singleLineHeight + DEFAULT_SPACE;

        return _dialogeLineSO switch
        {
            DialogueSimpleLine => height + GetSimpleLineHeight(),
            DialogueChoiceLine => height + GetChoiceLineHeight(),
            DialogueEventLine => height + GetEventLineHeight(),
            _ => height
        };
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (!_initialized)
        {
            _dialogeLineSO = property.FindPropertyRelative("DialogueLineSO").objectReferenceValue;
            _serializedLine = new SerializedObject(_dialogeLineSO as DialogueLineSO);
            var choices = _serializedLine.FindProperty("Choices");
            if (choices != null)
            {
                _elementsHeights = new List<float>();
                _choicesList = new ReorderableList(_serializedLine, choices)
                {
                    drawHeaderCallback = ChoicesDrawListHeader,
                    drawElementCallback = ChoicesDrawListItem,
                    elementHeightCallback = ChoicesElementHeight
                };
            }

            _initialized = true;
        }
        
        EditorGUI.BeginProperty(position, label, property);

        if (_dialogeLineSO is DialogueCharacterSimpleLine or DialogueCharacterChoiceLine)
        {
            DrawDialogueCharacter(position);
            position.y += GetCharacterLineHeight() + EditorGUIUtility.singleLineHeight + DEFAULT_SPACE;
        }
        

        switch (_dialogeLineSO)
        {
            case DialogueSimpleLine:
                DrawSimpleLine(position);
                break;
            
            case DialogueChoiceLine:
                DrawChoiceLine(position);
                break;
            case DialogueEventLine:
                DrawEventLine(position);
                break;
        }

        _serializedLine.ApplyModifiedProperties();
        EditorGUI.EndProperty();
    }

    private void DrawSimpleLine(Rect position)
    {
        var text = _serializedLine.FindProperty("Text");
        var nextDialogue = _serializedLine.FindProperty("NextDialogue");
        float currentHeight = 0;

        Rect textRect = new Rect(position.x, position.y, position.width, EditorGUI.GetPropertyHeight(text));
        EditorStyles.label.alignment = TextAnchor.MiddleCenter;
        EditorStyles.textArea.fontStyle = FontStyle.Italic;
        EditorGUI.PropertyField(textRect, text);
        currentHeight += EditorGUI.GetPropertyHeight(text) + DEFAULT_SPACE;
        
        EditorStyles.label.alignment = TextAnchor.MiddleLeft;
        Rect nextDialogueRect = new Rect(position.x, position.y + currentHeight, position.width, EditorGUI.GetPropertyHeight(nextDialogue));
        var parentDialogue = _serializedLine.FindProperty("ParentDialogue");
        var dialogueSO = new SerializedObject(parentDialogue.objectReferenceValue as DialogueSO);
        DrawNextDialogueSelector(nextDialogueRect, nextDialogue, dialogueSO.FindProperty("DialogueLines"));
    }

    private float GetSimpleLineHeight()
    {
        var text = _serializedLine.FindProperty("Text");
        var nextDialogue = _serializedLine.FindProperty("NextDialogue");
        float currentHeight = 0;
        currentHeight += EditorGUI.GetPropertyHeight(text) + DEFAULT_SPACE;
        currentHeight += nextDialogue.objectReferenceValue == null
            ? DEFAULT_SPACE + EditorGUIUtility.singleLineHeight
            : DEFAULT_SPACE * 2 + EditorGUIUtility.singleLineHeight * 2;

        return currentHeight;
    }

    private void DrawChoiceLine(Rect position)
    {
        var text = _serializedLine.FindProperty("Text");
        var choices = _serializedLine.FindProperty("Choices");
        float currentHeight = 0;

        Rect textRect = new Rect(position.x, position.y, position.width, EditorGUI.GetPropertyHeight(text));
        EditorStyles.label.alignment = TextAnchor.MiddleCenter;
        EditorStyles.textArea.fontStyle = FontStyle.Italic;
        EditorGUI.PropertyField(textRect, text);
        currentHeight += EditorGUI.GetPropertyHeight(text) + DEFAULT_SPACE;

        _choicesList.DoLayoutList();
    }

    private float GetChoiceLineHeight()
    {
        var text = _serializedLine.FindProperty("Text");
        var choices = _serializedLine.FindProperty("Choices");
        
        return EditorGUI.GetPropertyHeight(text) + EditorGUI.GetPropertyHeight(choices) + DEFAULT_SPACE * 4;
    }

    private void ChoicesDrawListHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, "Choices");
    }

    private void ChoicesDrawListItem(Rect rect, int index, bool isActive, bool isFocused)
    {
        var choice = _choicesList.serializedProperty.GetArrayElementAtIndex(index);
        var text = choice.FindPropertyRelative("ChoiceText");
        var nextDialogue = choice.FindPropertyRelative("NextDialogue");
        float currentHeight = 0;
        
        EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), "Choice Text");

        if (rect.width > 0) _nonBuggedWidth = rect.width;
        var textHeight = EditorStyles.textArea.CalcHeight(new GUIContent(text.stringValue), _nonBuggedWidth);
        currentHeight += DEFAULT_SPACE + EditorGUIUtility.singleLineHeight;
        var textRect = new Rect(rect.x, rect.y + currentHeight, rect.width, textHeight);
        text.stringValue = EditorGUI.TextArea(textRect, text.stringValue, EditorStyles.textArea);

        currentHeight += DEFAULT_SPACE * 2 + textHeight;

        var parentDialogue = _serializedLine.FindProperty("ParentDialogue");
        var dialogueSO = new SerializedObject(parentDialogue.objectReferenceValue as DialogueSO);
        
        currentHeight += DrawNextDialogueSelector(new Rect(rect.x, rect.y + currentHeight, rect.width, EditorGUIUtility.singleLineHeight),
            nextDialogue, dialogueSO.FindProperty("DialogueLines"));
        currentHeight += DEFAULT_SPACE + EditorGUIUtility.singleLineHeight;
        
        try
        {
            _elementsHeights[index] = currentHeight;
        }
        catch (ArgumentOutOfRangeException)
        {
            _elementsHeights.Add(currentHeight);
        }
    }

    private float ChoicesElementHeight(int index)
    {
        try
        {
            return _elementsHeights[index];
        }
        catch (ArgumentOutOfRangeException)
        {
            return 0;
        }
    }

    private float DrawNextDialogueSelector(Rect rect, SerializedProperty nextDialogue, SerializedProperty dialogueLines)
    {
        if (EditorGUI.DropdownButton(rect, new GUIContent("Choose next line"), FocusType.Passive))
        {
            var menu = new GenericMenu();
            
            for (int i = 0; i < dialogueLines.arraySize; i++)
            {
                var line = dialogueLines.GetArrayElementAtIndex(i);
                var lineReference = line.objectReferenceValue as DialogueLineSO;
                
                if(lineReference.LineIdentifier.Equals(_serializedLine.FindProperty("m_Name").stringValue)) continue;
                
                menu.AddItem(new GUIContent($"{lineReference.LineIdentifier}"), false, SetNextDialogue, new NextDialogueWrapper()
                {
                    NextDialogueReference = nextDialogue,
                    NextLine = line
                });
            }
            menu.ShowAsContext();
        }

        rect.y += DEFAULT_SPACE + EditorGUIUtility.singleLineHeight;
        
        if (nextDialogue.objectReferenceValue != null)
        {
            EditorGUI.ObjectField(rect, nextDialogue);
            return DEFAULT_SPACE * 2 + EditorGUIUtility.singleLineHeight;
        }

        return DEFAULT_SPACE;
    }

    private void SetNextDialogue(object obj)
    {
        var wrapper = obj as NextDialogueWrapper;
        wrapper.NextDialogueReference.objectReferenceValue = wrapper.NextLine.objectReferenceValue;
    }

    private class NextDialogueWrapper
    {
        public SerializedProperty NextDialogueReference;
        public SerializedProperty NextLine;
    }

    private void DrawDialogueCharacter(Rect position)
    {
        var characterProperty = _serializedLine.FindProperty("Character");
        var portraitProperty = _serializedLine.FindProperty("Portrait");
        var portraitKey = _serializedLine.FindProperty("PortraitKey");

        EditorStyles.label.fontSize = 12;
        EditorGUI.ObjectField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), characterProperty);
        float currentHeight = EditorGUIUtility.singleLineHeight + DEFAULT_SPACE;
        
        if (characterProperty.objectReferenceValue == null) return;

        var character = characterProperty.objectReferenceValue as DialogueCharacterSO;
        EditorStyles.label.alignment = TextAnchor.MiddleCenter;
        EditorStyles.label.fontSize = 16;
        EditorGUI.LabelField(new Rect(position.x, position.y + currentHeight, position.width, EditorGUIUtility.singleLineHeight), character.Name);
        currentHeight += EditorGUIUtility.singleLineHeight + DEFAULT_SPACE;

        var portraits = character.Portraits;
        string text = "";
        if (portraitKey.stringValue != "")
        {
            Rect portraitRect = new Rect(position.x + position.width/2 - 100, position.y + currentHeight,
                200, 200);
            Color defaultColor = GUI.color;
            GUI.color = Color.clear;
            EditorGUI.DrawTextureTransparent(portraitRect, portraits[portraitKey.stringValue].texture, ScaleMode.ScaleAndCrop);
            GUI.color = defaultColor;
            text = portraitKey.stringValue;
        }
        else
        {
            text = "Choose Portrait";
        }

        currentHeight += 200 + DEFAULT_SPACE;
        var dropdownRect = new Rect(position.x + position.width / 2 - 50, position.y + currentHeight, 100,
            EditorGUIUtility.singleLineHeight);
        if (EditorGUI.DropdownButton(dropdownRect, new GUIContent(text), FocusType.Passive))
        {
            GenericMenu menu = new GenericMenu();
            
            foreach (var key in portraits.Keys)
            {
                menu.AddItem(new GUIContent(key), false, data =>
                {
                    portraitKey.stringValue = key;
                    portraitProperty.objectReferenceValue = portraits[key];
                }, key);
            }

            menu.DropDown(dropdownRect);
        }
        
    }
    
    private float GetCharacterLineHeight()
    {
        return EditorGUIUtility.singleLineHeight * 2 + DEFAULT_SPACE * 4 + 200;
    }

    private void DrawEventLine(Rect position)
    {
        var evt = _serializedLine.FindProperty("DialogueEvent");
        var nextDialogue = _serializedLine.FindProperty("NextDialogue");
        float currentHeight = DEFAULT_SPACE;
        
        EditorStyles.label.alignment = TextAnchor.MiddleCenter;
        EditorStyles.label.fontSize = 16;
        EditorGUI.LabelField(new Rect(position.x, position.y + currentHeight, position.width, EditorGUIUtility.singleLineHeight), "Event");
        currentHeight += DEFAULT_SPACE + EditorGUIUtility.singleLineHeight;

        EditorGUI.ObjectField(new Rect(position.x, position.y + currentHeight, position.width, EditorGUIUtility.singleLineHeight), evt, GUIContent.none);
        currentHeight += DEFAULT_SPACE + EditorGUIUtility.singleLineHeight;
        
        var parentDialogue = _serializedLine.FindProperty("ParentDialogue");
        var dialogueSO = new SerializedObject(parentDialogue.objectReferenceValue as DialogueSO);
        Rect rect = new Rect(position.x, position.y + currentHeight, position.width, EditorGUIUtility.singleLineHeight);
        EditorStyles.label.alignment = TextAnchor.MiddleLeft;
        EditorStyles.label.fontSize = 12;
        DrawNextDialogueSelector(rect, nextDialogue, dialogueSO.FindProperty("DialogueLines"));
    }

    private float GetEventLineHeight()
    {
        float height = DEFAULT_SPACE * 3 + EditorGUIUtility.singleLineHeight * 2;
        var nextDialogue = _serializedLine.FindProperty("NextDialogue");
        
        if(nextDialogue.objectReferenceValue != null) return height + DEFAULT_SPACE * 4 + EditorGUIUtility.singleLineHeight;
        return height + DEFAULT_SPACE + EditorGUIUtility.singleLineHeight;
    }
}
