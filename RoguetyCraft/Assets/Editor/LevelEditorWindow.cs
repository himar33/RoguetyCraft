#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace RoguetyCraft.Map.Editor.LevelGraph
{
    public class LevelEditorWindow : EditorWindow
    {
        private LevelEditorGraphView _graphView;

        private readonly string _defaultFileName = "LevelGraphFileName";

        private static TextField _fileNameField;
        private Button _saveButton;

        [MenuItem("Window/Roguety/Level Graph")]
        public static void Open()
        {
            LevelEditorWindow wnd = GetWindow<LevelEditorWindow>();
            wnd.titleContent = new GUIContent("LevelGraphEditor");
        }

        private void OnEnable()
        {
            AddGraphView();
            AddToolbar();
            AddStyles();
        }

        private void AddGraphView()
        {
            _graphView = new(this);

            _graphView.StretchToParentSize();

            rootVisualElement.Add(_graphView);
        }

        private void AddToolbar()
        {
            Toolbar toolbar = new();
            rootVisualElement.Add(toolbar);

            _fileNameField = new TextField("File Name:")
            {
                value = "New File"
            };
            toolbar.Add(_fileNameField);

            _saveButton = new ToolbarButton(() => Save())
            {
                text = "Save",
            };
            toolbar.Add(_saveButton);

            var loadButton = new ToolbarButton(() => Load())
            {
                text = "Load",
            };
            toolbar.Add(loadButton);

            var clearButton = new ToolbarButton(() => Clear())
            {
                text = "Clear",
            };
            toolbar.Add(clearButton);

            var resetButton = new ToolbarButton(() => ResetGraph())
            {
                text = "Reset",
            };
            toolbar.Add(resetButton);
        }

        private void AddStyles()
        {
            StyleSheet styleSheet = (StyleSheet)EditorGUIUtility.Load("Variables.uss");

            rootVisualElement.styleSheets.Add(styleSheet);
        }

        private void Save()
        {
            if (string.IsNullOrEmpty(_fileNameField.value))
            {
                EditorUtility.DisplayDialog("Invalid file name.", "Please ensure the file name you've typed in is valid.", "Please!");

                return;
            }

            LevelEditorSerializer.Init(_graphView, _fileNameField.value);
            LevelEditorSerializer.Save();
        }

        private void Load()
        {
            string filePath = EditorUtility.OpenFilePanel("Level Graphs", "Assets", "asset");

            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            Clear();

            LevelEditorSerializer.Init(_graphView, Path.GetFileNameWithoutExtension(filePath));
            LevelEditorSerializer.Load();
        }

        private void Clear()
        {
            _graphView.ClearGraph();
        }

        private void ResetGraph()
        {
            Clear();

            UpdateFileName(_defaultFileName);
        }

        public static void UpdateFileName(string newFileName)
        {
            _fileNameField.value = newFileName;
        }

        public void EnableSaving()
        {
            _saveButton.SetEnabled(true);
        }

        public void DisableSaving()
        {
            _saveButton.SetEnabled(false);
        }
    }
}
#endif