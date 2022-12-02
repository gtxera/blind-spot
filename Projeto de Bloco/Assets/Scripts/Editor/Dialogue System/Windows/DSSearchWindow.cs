using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.PackageManager;
using UnityEngine;

namespace DS.Windows
{
    using Elements;
    using Enumerations;
    public class DSSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private DSGraphView graphView;
        private Texture2D identationIcon;
        public void Initialize(DSGraphView dsGraphView)
        {
            graphView = dsGraphView;

            identationIcon = new Texture2D(1, 1);
            identationIcon.SetPixel(0, 0, Color.clear);
            identationIcon.Apply();
        }
        
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> _searchTreeEntries = new List<SearchTreeEntry>()
            {
                new SearchTreeGroupEntry(new GUIContent("Create Element", identationIcon)),
                new SearchTreeGroupEntry(new GUIContent("Dialogue Node", identationIcon), 1),
                new SearchTreeGroupEntry(new GUIContent("Simple Node", identationIcon),2),
                new SearchTreeEntry(new GUIContent("Single Choice", identationIcon))
                {
                    level = 3,
                    userData = DialogueType.SingleChoice
                },
                new SearchTreeEntry(new GUIContent("Multiple Choice", identationIcon))
                {
                    level = 3,
                    userData = DialogueType.MultipleChoice
                },
                new SearchTreeGroupEntry(new GUIContent("Character Node", identationIcon),2),
                new SearchTreeEntry(new GUIContent("Single Choice Character", identationIcon))
                {
                    level = 3,
                    userData = DialogueType.SingleChoiceCharacter
                },
                new SearchTreeEntry(new GUIContent("Multiple Choice Character", identationIcon))
                {
                    level = 3,
                    userData = DialogueType.MultipleChoiceCharacter
                },
                new SearchTreeGroupEntry(new GUIContent("Event Node", identationIcon), 1),
                new SearchTreeEntry(new GUIContent("Single Group", identationIcon))
                {
                    level = 2,
                    userData = DialogueType.Event
                },
                new SearchTreeGroupEntry(new GUIContent("Dialogue Group", identationIcon), 1),
                new SearchTreeEntry(new GUIContent("Single Group", identationIcon))
                {
                    level = 2,
                    userData = new Group()
                }
            };

            return _searchTreeEntries;
        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            Vector2 _localMousePosition = graphView.GetLocalMousePosition(context.screenMousePosition, true);
            switch (searchTreeEntry.userData)
            {
                case DialogueType.SingleChoice:
                {
                    DSSingleChoiceNode _singleChoiceNode =
                        (DSSingleChoiceNode) graphView.CreateNode(DialogueType.SingleChoice, _localMousePosition);
                    graphView.AddElement(_singleChoiceNode);
                    return true;
                }
                case DialogueType.MultipleChoice:
                {
                    DSMultipleChoiceNode _multipleChoiceNode =
                        (DSMultipleChoiceNode) graphView.CreateNode(DialogueType.MultipleChoice, _localMousePosition);
                    graphView.AddElement(_multipleChoiceNode);
                    return true;
                }
                case DialogueType.SingleChoiceCharacter:
                {
                    DSSingleChoiceCharacterNode _singeChoiceCharacterNode =
                        (DSSingleChoiceCharacterNode) graphView.CreateNode(DialogueType.SingleChoiceCharacter, _localMousePosition);
                    graphView.AddElement(_singeChoiceCharacterNode);
                    return true;
                }
                case DialogueType.MultipleChoiceCharacter:
                {
                    DSMultipleChoiceCharacterNode _multipleChoiceCharacterNode =
                        (DSMultipleChoiceCharacterNode) graphView.CreateNode(DialogueType.MultipleChoiceCharacter, _localMousePosition);
                    graphView.AddElement(_multipleChoiceCharacterNode);
                    return true;
                }
                case DialogueType.Event:
                {
                    DSEventNode _eventNode =
                        (DSEventNode) graphView.CreateNode(DialogueType.Event, _localMousePosition);
                    graphView.AddElement(_eventNode);
                    return true;
                }
                case Group _:
                {
                    graphView.CreateGroup("Dialogue Group", _localMousePosition);
                    return true;
                }
                default: return false;
            }
        }
    }
}
