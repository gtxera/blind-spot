using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEngine;
using System;
using DS.Data.Save;

namespace DS.Elements
{
    using Enumerations;
    using Utilities;
    using Windows;
    using Data.Save;
    
    public class DSNode : Node
    {
        public string ID { get; set; }
        public string DialogueName { get; set; }
        public List<DSChioceSaveData> Choices { get; set; }
        public string Text { get; set; }
        public DialogueType DialogueType { get; set; }
        public DSGroup Group { get; set; }

        protected Color defalutBackgroundColor;

        protected DSGraphView graphView;
        
        public virtual void Initialize(DSGraphView dsGraphView, Vector2 position)
        {
            ID = Guid.NewGuid().ToString();
            DialogueName = "DialogueName";
            Choices = new List<DSChioceSaveData>();
            Text = "DialogueText";
            graphView = dsGraphView;
            
            SetPosition(new Rect(position, Vector2.zero));
            
            mainContainer.AddToClassList("ds-node__main-container");
            extensionContainer.AddToClassList("ds-node__extension-container");
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Disconnect Input Ports", actionEvent => DisconnectInputPorts());
            evt.menu.AppendAction("Disconnect Output Ports", actionEvent => DisconnectOutputPorts());
            
            
            base.BuildContextualMenu(evt);
        }

        public virtual void Draw()
        {
            /*title container*/
            TextField _dialogueNameTextField = DSElementUtility.CreateTextField(DialogueName, null, callback =>
            {
                TextField _target = (TextField) callback.target;
                _target.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();
                if (Group == null)
                {
                    graphView.RemoveUngroupedNode(this);
                    DialogueName = _target.value;
                    graphView.AddUngroupedNode(this);
                    return;
                }

                DSGroup _currentGroup = Group;
                graphView.RemoveGroupedNode(this, _currentGroup);
                DialogueName = callback.newValue;
                graphView.AddGroupedNode(this, _currentGroup);
            });
            _dialogueNameTextField.AddClasses("ds-node__text-field", 
                "ds-node__file-name-text-field", 
                "ds-node__text-field__hidden");
            titleContainer.Insert(0, _dialogueNameTextField);

            /*input port*/
            Port _inputPort = this.CreatePort("Dialogue Connection", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);
            inputContainer.Add(_inputPort);

            /*extension container*/
            VisualElement _customDataContainer = new VisualElement();
            _customDataContainer.AddToClassList("ds-node__custom-data-container");
            
            Foldout _textFoldout = DSElementUtility.CreateFoldout("Dialogue Text");
            
            TextField _textTextField = DSElementUtility.CreateTextArea(Text);

            _textTextField.AddClasses("ds-node__text-field", "ds-node__quote-text-field");

            _textFoldout.Add(_textTextField);
            _customDataContainer.Add(_textFoldout);
            
            extensionContainer.Add(_customDataContainer);
        }

        public void DisconnectAllPorts()
        {
            DisconnectPorts(outputContainer);
            DisconnectPorts(inputContainer);
        }
        
        private void DisconnectPorts(VisualElement container)
        {
            foreach (Port _port in container.Children())
            {
                if (!_port.connected) continue;
                
                graphView.DeleteElements(_port.connections);
            }
        }

        private void DisconnectInputPorts()
        {
            DisconnectPorts(inputContainer);
        }
        
        private void DisconnectOutputPorts()
        {
            DisconnectPorts(outputContainer);
        }
        
        public void SetErrorStyle(Color color)
        {
            mainContainer.style.backgroundColor = color;
        }

        public void ResetStyle()
        {
            mainContainer.style.backgroundColor = defalutBackgroundColor;
        }
    }
}