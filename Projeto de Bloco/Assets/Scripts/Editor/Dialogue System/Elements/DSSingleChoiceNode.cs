using System.Collections;
using System.Collections.Generic;
using DS.Data;
using DS.Utilities;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DS.Elements
{
    using Data.Save;
    using Enumerations;
    using Windows;
    public class DSSingleChoiceNode : DSNode
    {
        public override void Initialize(DSGraphView dsGraphView, Vector2 position)
        {
            base.Initialize(dsGraphView, position);

            defalutBackgroundColor = Color.blue;
            ResetStyle();
            
            DialogueType = DialogueType.SingleChoice;

            DSChioceSaveData choiceData = new DSChioceSaveData()
            {
                Text = "Next Dialogue"
            };
            
            Choices.Add(choiceData);
        }
        
        public override void Draw()
        {
            base.Draw();
            
            /*output container*/
            foreach (DSChioceSaveData _choice in Choices)
            {
                Port _choicePort = this.CreatePort(_choice.Text);
                outputContainer.Add(_choicePort);
                
                RefreshExpandedState();
            }
        }
    }
}
