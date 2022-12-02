using DS.Enumerations;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Windows
{
    using Utilities;
    public class DSEditorWindow : EditorWindow
    {
        private readonly string defaultFileName = "DialogueFileName";
        private TextField fileNameTextField;
        private Button saveButton;
        
        [MenuItem("Window/DS/Dialogue Editor")]
        public static void ShowExample()
        {
            DSEditorWindow wnd = GetWindow<DSEditorWindow>();
            wnd.titleContent = new GUIContent("Dialogue Editor");
        }

        private void OnEnable()
        {
            AddGraphView();
            AddToolbar();

            AddStyles();
        }
        

        private void AddStyles()
        {
            rootVisualElement.AddStyleSheets("DialogueSystem/DSVariables.uss");
        }

        private void AddGraphView()
        {
            DSGraphView _graphView = new DSGraphView(this);
            _graphView.StretchToParentSize();
            rootVisualElement.Add(_graphView);
        }
        
        private void AddToolbar()
        {
            Toolbar _toolbar = new Toolbar();

            fileNameTextField = DSElementUtility.CreateTextField(defaultFileName, "File Name:", callback =>
            {
                fileNameTextField.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();
            });
            saveButton = DSElementUtility.CreateButton("Save");
            _toolbar.Add(fileNameTextField);
            _toolbar.Add(saveButton);
            _toolbar.AddStyleSheets("DialogueSystem/DSToolbarStyles.uss");
            
            rootVisualElement.Add(_toolbar);
        }

        public void EnableSaving() => saveButton.SetEnabled(true);

        public void DisableSaving() => saveButton.SetEnabled(false);
        
        }
}