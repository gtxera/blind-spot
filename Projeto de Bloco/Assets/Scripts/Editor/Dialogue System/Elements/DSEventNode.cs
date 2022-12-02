using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Elements{
    using Utilities;
    using Windows;
    using Enumerations;
    public class DSEventNode : DSSingleChoiceNode
    {
        public DialogueEventSO DialogueEvent { get; set; }
        
        public override void Initialize(DSGraphView dsGraphView, Vector2 position)
        {
            base.Initialize(dsGraphView, position);
            
            DialogueName = "EventName";
            
            defalutBackgroundColor = Color.magenta;
            ResetStyle();
            
            DialogueType = DialogueType.Event;

        }

        public override void Draw()
        {
            base.Draw();
            
            extensionContainer.Clear();
            Foldout _eventFoldout = DSElementUtility.CreateFoldout("Event");
            ObjectField _eventField = new ObjectField()
            {
                objectType = typeof(DialogueEventSO),
                allowSceneObjects = false,
                value = DialogueEvent
            };
            _eventField.RegisterValueChangedCallback(value =>
            {
                DialogueEvent = _eventField.value as DialogueEventSO;
            });
            _eventField.AddToClassList("ds-node__object-field");
            _eventFoldout.Add(_eventField);
            extensionContainer.Add(_eventFoldout);
        }
    }
}
