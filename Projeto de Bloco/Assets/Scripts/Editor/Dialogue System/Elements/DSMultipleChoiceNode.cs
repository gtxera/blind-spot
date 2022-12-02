using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Elements
{
    using Data.Save;
    using Enumerations;
    using Utilities;
    using Windows;
    public class DSMultipleChoiceNode : DSNode
    {
        public override void Initialize(DSGraphView dsGraphView, Vector2 position)
        {
            base.Initialize(dsGraphView, position);

            defalutBackgroundColor = Color.green;
            ResetStyle();
            
            DialogueType = DialogueType.MultipleChoice;

            DSChioceSaveData _choiceData = new DSChioceSaveData()
            {
                Text = "New Choice"
            };
            
            Choices.Add(_choiceData);
        }

        public override void Draw()
        {
            base.Draw();

            Button _addChoiceButton = DSElementUtility.CreateButton("Add Choice",
                () =>
                {
                    DSChioceSaveData _choiceData = new DSChioceSaveData()
                    {
                        Text = "New Choice"
                    };
                    
                    Port _choicePort = CreateChoicePort(_choiceData);
                    Choices.Add(_choiceData);
                    outputContainer.Add(_choicePort);
                }
            );
            _addChoiceButton.AddToClassList("ds-node__button");
            mainContainer.Insert(1, _addChoiceButton);
            
            foreach (DSChioceSaveData _choice in Choices)
            {
                Port _choicePort = CreateChoicePort(_choice);

                _choicePort.userData = _choice;
                
                outputContainer.Add(_choicePort);
            }
            RefreshExpandedState();
        }
        
        private Port CreateChoicePort(object userData)
        {
            Port _choicePort = this.CreatePort();

            _choicePort.userData = userData;
            DSChioceSaveData _choiceData = (DSChioceSaveData) userData;

            Button _deleteChoiceButton = DSElementUtility.CreateButton("X", () =>
            {
                if(Choices.Count == 1) return;

                if (_choicePort.connected) graphView.DeleteElements(_choicePort.connections);

                Choices.Remove(_choiceData);
                graphView.RemoveElement(_choicePort);
            });
            _deleteChoiceButton.AddToClassList("ds-node__button");
            TextField _choiceTextField = DSElementUtility.CreateTextField(_choiceData.Text, null, callback =>
            {
                _choiceData.Text = callback.newValue;
            });
            _choiceTextField.AddClasses("ds-node__text-field", 
                "ds-node__choice-text-field", 
                "ds-node__text-field__hidden");
            _choicePort.Add(_choiceTextField);
            _choicePort.Add(_deleteChoiceButton);
            return _choicePort;
        }
    }
}
