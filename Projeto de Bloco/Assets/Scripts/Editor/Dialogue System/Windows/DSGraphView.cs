using System;
using System.Collections.Generic;
using DS.Data.Save;
using DS.Utilities;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Windows
{
    using Elements;
    using Enumerations;
    using Data.Error;
    public class DSGraphView : GraphView
    {
        private DSEditorWindow editorWindow;
        private DSSearchWindow searchWindow;

        private SerializableDictionary<string, DSNodeErrorData> ungroupedNodes;
        private SerializableDictionary<Group, SerializableDictionary<string, DSNodeErrorData>> groupedNodes;
        private SerializableDictionary<string, DSGroupErrorData> groups;

        private int repeatedNamesAmount;

        public int RepeatedNamesAmount
        {
            get { return repeatedNamesAmount;}
            set
            {
                repeatedNamesAmount = value; 
                if(repeatedNamesAmount == 0) editorWindow.EnableSaving();
                if(repeatedNamesAmount == 1) editorWindow.DisableSaving();
            }
        }

        public DSGraphView(DSEditorWindow dsEditorWindow)
        {
            editorWindow = dsEditorWindow;

            ungroupedNodes = new SerializableDictionary<string, DSNodeErrorData>();
            groupedNodes = new SerializableDictionary<Group, SerializableDictionary<string, DSNodeErrorData>>();
            groups = new SerializableDictionary<string, DSGroupErrorData>();
            
            AddManipulators();
            AddSearchWindow();
            AddGridBackground();
            AddStyles();
            
            OnElementsDeleted();
            OnGroupElementsAdded();
            OnGroupElementsRemoved();
            OnGroupRenamed();
            OnGraphViewChanged();
        }

        private void AddSearchWindow()
        {
            if (searchWindow == null)
            {
                searchWindow = ScriptableObject.CreateInstance<DSSearchWindow>();
                searchWindow.Initialize(this);
            }

            nodeCreationRequest = context =>
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> _compatiblePorts = new List<Port>();

            ports.ForEach(port =>
            {
                if (startPort == port) return;
                if (startPort.node == port.node) return;
                if (startPort.direction == port.direction) return;
                _compatiblePorts.Add(port);
            });
            
            return _compatiblePorts;
        }

        public DSNode CreateNode(DialogueType dialogueType, Vector2 position)
        {
            Type _nodeType = Type.GetType($"DS.Elements.DS{dialogueType}Node");
            
            DSNode _node = (DSNode) Activator.CreateInstance(_nodeType);
            _node.Initialize(this, position);
            _node.Draw();
            AddElement(_node);
            
            AddUngroupedNode(_node);
            
            return _node;
        }

        public void AddUngroupedNode(DSNode node)
        {
            string _nodeName = node.DialogueName.ToLower();

            if (!ungroupedNodes.ContainsKey(_nodeName))
            {
                DSNodeErrorData _nodeErrorData = new DSNodeErrorData();
                
                _nodeErrorData.Nodes.Add(node);
                
                ungroupedNodes.Add(_nodeName, _nodeErrorData);

                return;
            }

            List<DSNode> _ungroupedNodesList = ungroupedNodes[_nodeName].Nodes;
            
            _ungroupedNodesList.Add(node);

            Color _errorColor = ungroupedNodes[_nodeName].ErrorData.Color;
            node.SetErrorStyle(_errorColor);

            if (_ungroupedNodesList.Count == 2)
            {
                ++RepeatedNamesAmount;
                _ungroupedNodesList[0].SetErrorStyle(_errorColor);
            }
        }

        public void RemoveUngroupedNode(DSNode node)
        {
            string _nodeName = node.DialogueName.ToLower();
            List <DSNode> _ungroupedNodesList = ungroupedNodes[_nodeName].Nodes;

            _ungroupedNodesList.Remove(node);
            node.ResetStyle();

            if (_ungroupedNodesList.Count == 1)
            {
                --RepeatedNamesAmount;
                _ungroupedNodesList[0].ResetStyle();

                return;
            }
            if (_ungroupedNodesList.Count == 0)
            {
                ungroupedNodes.Remove(_nodeName);
            }
        }

        public void AddGroupedNode(DSNode node, DSGroup group)
        {
            string _nodeName = node.DialogueName.ToLower();

            node.Group = group;
            
            if(!groupedNodes.ContainsKey(group)) groupedNodes.Add(group, new SerializableDictionary<string, DSNodeErrorData>());

            if (!groupedNodes[group].ContainsKey(_nodeName))
            {
                DSNodeErrorData _nodeErrorData = new DSNodeErrorData();
                _nodeErrorData.Nodes.Add(node);
                
                groupedNodes[group].Add(_nodeName, _nodeErrorData);
                return;
            }

            List<DSNode> groupedNodesList = groupedNodes[group][_nodeName].Nodes;
            
            groupedNodesList.Add(node);

            Color _errorColor = groupedNodes[group][_nodeName].ErrorData.Color;
            node.SetErrorStyle(_errorColor);

            if (groupedNodesList.Count == 2)
            {
                ++RepeatedNamesAmount;
                groupedNodesList[0].SetErrorStyle(_errorColor);
            }
        }

        public void RemoveGroupedNode(DSNode node, Group group)
        {
            string _nodeName = node.DialogueName.ToLower();

            node.Group = null;
            
            List<DSNode> _groupedNodesList = groupedNodes[group][_nodeName].Nodes;
            
            _groupedNodesList.Remove(node);
            node.ResetStyle();

            if (_groupedNodesList.Count == 1)
            {
                --RepeatedNamesAmount;
                _groupedNodesList[0].ResetStyle();
            }

            if (_groupedNodesList.Count == 0)
            {
                groupedNodes[group].Remove(_nodeName);
                if (groupedNodes[group].Count == 0) groupedNodes.Remove(group);
            }
        }
        
        private void AddGroup(DSGroup group)
        {
            string _groupName = group.title.ToLower();

            if (!groups.ContainsKey(_groupName))
            {
                DSGroupErrorData _groupErrorData = new DSGroupErrorData();
                
                _groupErrorData.Groups.Add(group);
                
                groups.Add(_groupName, _groupErrorData);

                return;
            }

            List<DSGroup> _groupsList = groups[_groupName].Groups;
            _groupsList.Add(group);

            Color _errorColor = groups[_groupName].ErrorData.Color;
            group.SetErrorStyles(_errorColor);

            if (_groupsList.Count == 2)
            {
                ++RepeatedNamesAmount;
                _groupsList[0].SetErrorStyles(_errorColor);
            }
        }

        private void RemoveGroup(DSGroup group)
        {
            string _oldGroupName = group.OldTitle.ToLower();
            List<DSGroup> _groupsList = groups[_oldGroupName].Groups;
            _groupsList.Remove(group);
            group.ResetStyles();

            if (_groupsList.Count == 1)
            {
                --RepeatedNamesAmount;
                _groupsList[0].ResetStyles();
                return;
            }
            if (_groupsList.Count == 0) groups.Remove(_oldGroupName);
            }
        
        private void AddManipulators()
        {
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            this.AddManipulator(CreateNodeContextualMethod("Single Choice", DialogueType.SingleChoice));
            this.AddManipulator(CreateNodeContextualMethod("Multiple Choice", DialogueType.MultipleChoice));
            this.AddManipulator(CreateNodeContextualMethod("Single Choice Character", DialogueType.SingleChoiceCharacter));
            this.AddManipulator(CreateNodeContextualMethod("Multiple Choice Character", DialogueType.MultipleChoiceCharacter));
            this.AddManipulator(CreateNodeContextualMethod("Event", DialogueType.Event));
            
            this.AddManipulator(CreateGroupContextualMenu());
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        }

        private IManipulator CreateGroupContextualMenu()
        { 
            ContextualMenuManipulator _manipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction("Add Group", 
                        actionEvent => CreateGroup("Dialogue Group", GetLocalMousePosition(actionEvent.eventInfo.localMousePosition))));
            return _manipulator; 
        }

        public DSGroup CreateGroup(string title, Vector2 position)
        {
            DSGroup _group = new DSGroup(title, position);

            AddGroup(_group);
            
            AddElement(_group);

            foreach (GraphElement _selectedElement in selection)
            {
                if (!(_selectedElement is DSNode))
                {
                    continue;
                }
                DSNode _node = (DSNode) _selectedElement;
                
                _group.AddElement(_node);
            }
            
            return _group;
        }
        

        private IManipulator CreateNodeContextualMethod(string actionTitle, DialogueType dialogueType)
        {
            ContextualMenuManipulator _manipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction(actionTitle, 
                    actionEvent => AddElement(CreateNode(dialogueType, GetLocalMousePosition(actionEvent.eventInfo.localMousePosition))))
                );

            return _manipulator;
        }

        private void AddStyles()
        {
            this.AddStyleSheets("DialogueSystem/DSGraphViewStyles.uss",
                "DialogueSystem/DSNodeStyles.uss");
        }

        private void AddGridBackground()
        {
            GridBackground _gridBackground = new GridBackground();
            _gridBackground.StretchToParentSize();
            Insert(0, _gridBackground);
        }

        public Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isSearchWindow = false)
        {
            Vector2 worldMousePosition = mousePosition;

            if (isSearchWindow)
            {
                worldMousePosition -= editorWindow.position.position;
            }
            Vector2 localMousePosition = contentViewContainer.WorldToLocal(worldMousePosition);
            return localMousePosition;
        }

        private void OnElementsDeleted()
        {
            Type _groupType = typeof(DSGroup);
            Type _edgeType = typeof(Edge);
            deleteSelection = (operationName, askUser) =>
            {
                List<DSNode> _nodesToDelete = new List<DSNode>();
                List<Edge> _edgesToDelete = new List<Edge>();
                List<DSGroup> _groupsToDelete = new List<DSGroup>();
                foreach (GraphElement _element in selection)
                {
                    if (_element is DSNode _node)
                    {
                        _nodesToDelete.Add(_node);
                        continue;
                    }

                    if (_element.GetType() == _edgeType)
                    {
                        Edge _edge = (Edge)_element;
                        _edgesToDelete.Add(_edge);
                        continue;
                    }
                    
                    if (_element.GetType() != _groupType) continue;

                    DSGroup _group = (DSGroup) _element;
                    RemoveGroup(_group);
                    _groupsToDelete.Add(_group);
                }
                
                DeleteElements(_edgesToDelete);

                foreach (DSNode _node in _nodesToDelete)
                {
                    if(_node.Group != null) _node.Group.RemoveElement(_node);
                    
                    RemoveUngroupedNode(_node);
                    _node.DisconnectAllPorts();
                    RemoveElement(_node);
                }

                foreach (DSGroup _group in _groupsToDelete)
                {
                    List<DSNode> _groupNodes = new List<DSNode>();
                    foreach (GraphElement _groupElement in _group.containedElements)
                    {
                        if (!(_groupElement is DSNode))
                        {
                            continue;
                        }
                        
                        _groupNodes.Add( (DSNode) _groupElement);
                    }
                    _group.RemoveElements(_groupNodes);
                    Remove(_group);
                    RemoveElement(_group);
                }
            };
        }

        private void OnGroupElementsAdded()
        {
            elementsAddedToGroup = (group, elements) =>
            {
                foreach (GraphElement _element in elements)
                {
                    if (!(_element is DSNode)) continue;

                    DSGroup _nodeGroup = (DSGroup)group;
                    DSNode _node = (DSNode)_element;
                    
                    RemoveUngroupedNode(_node);
                    AddGroupedNode(_node, _nodeGroup);
                }
            };
        }

        private void OnGroupElementsRemoved()
        {
            elementsRemovedFromGroup = (group, elements) =>
            {
                foreach (GraphElement _element in elements)
                {
                    if (!(_element is DSNode)) continue;

                    DSNode _node = (DSNode)_element;
                    
                    RemoveGroupedNode(_node, group);
                    AddUngroupedNode(_node);
                }
            };
        }

        private void OnGroupRenamed()
        {
            groupTitleChanged = (group, newTitle) =>
            {
                DSGroup _dsGroup = (DSGroup) group;

                _dsGroup.title = newTitle.RemoveWhitespaces().RemoveSpecialCharacters();
                
                RemoveGroup(_dsGroup);
                
                _dsGroup.OldTitle = _dsGroup.title;
                
                AddGroup(_dsGroup);
            };
        }

        private void OnGraphViewChanged()
        {
            graphViewChanged = (changes) =>
            {
                if (changes.edgesToCreate != null)
                {
                    foreach (Edge _edge in changes.edgesToCreate)
                    {
                        DSNode _nextNode = (DSNode) _edge.input.node;

                        DSChioceSaveData _choiceData = (DSChioceSaveData)_edge.output.userData;

                        _choiceData.NodeID = _nextNode.ID;
                    }
                }

                if (changes.elementsToRemove != null)
                {
                    Type _edgeType = typeof(Edge);

                    foreach (GraphElement _element in changes.elementsToRemove)
                    {
                        if (_element.GetType() != _edgeType) continue;

                        Edge _edge = (Edge)_element;

                        DSChioceSaveData _choideData = (DSChioceSaveData)_edge.output.userData;
                        _choideData.NodeID = "";
                    }
                }
                
                return changes;
            };
        }
    }
}