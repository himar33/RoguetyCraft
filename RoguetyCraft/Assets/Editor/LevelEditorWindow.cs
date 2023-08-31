using System;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace RoguetyCraft.Map.Editor.LevelGraph
{
    public class LevelEditorWindow : EditorWindow
    {

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
            LevelEditorGraphView graphView = new(this);

            graphView.StretchToParentSize();

            rootVisualElement.Add(graphView);
        }

        private void AddToolbar()
        {
            Toolbar toolbar = new();
            rootVisualElement.Add(toolbar);

            var fileNameField = new TextField("Filename:")
            {
                value = "New File"
            };
            toolbar.Add(fileNameField);

            var saveButton = new ToolbarButton()
            {
                text = "Save",
            };
            toolbar.Add(saveButton);
        }

        private void AddStyles()
        {
            StyleSheet styleSheet = (StyleSheet)EditorGUIUtility.Load("Variables.uss");

            rootVisualElement.styleSheets.Add(styleSheet);
        }
    }
}