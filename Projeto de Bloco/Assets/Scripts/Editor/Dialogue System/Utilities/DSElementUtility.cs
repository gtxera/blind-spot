using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace DS.Enumerations
{
    using Elements;
    public static class DSElementUtility
    {
        public static Port CreatePort(this DSNode node, string portName = "",
            Orientation orientation = Orientation.Horizontal, Direction direction = Direction.Output, Port.Capacity capacity = Port.Capacity.Single)
        {
            Port _port = node.InstantiatePort(orientation, direction, capacity, typeof(bool));
            _port.portName = portName;

            return _port;
        }
        
        public static Button CreateButton(string text, Action onClick = null)
        {
            Button _button = new Button()
            {
                text = text,
            };
            _button.clicked += onClick;
            return _button;
        }

        public static Foldout CreateFoldout(string title, bool collapsed = false)
        {
            Foldout _foldout = new Foldout()
            {
                text = title,
                value = !collapsed
            };

            return _foldout;
        }
        
        public static TextField CreateTextField(string value = null, string label = null, EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            TextField _textField = new TextField()
            {
                value = value,
                label = label
            };
            
            if(onValueChanged != null) _textField.RegisterValueChangedCallback(onValueChanged);

            return _textField;
        }

        public static TextField CreateTextArea(string value = null, string label = null,
            EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            TextField _textArea = CreateTextField(value, label, onValueChanged);
            _textArea.multiline = true;

            return _textArea;
        }
    }
}