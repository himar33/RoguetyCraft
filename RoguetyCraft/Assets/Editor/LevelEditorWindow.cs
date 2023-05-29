using UnityEditor;
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
            AddStyles();
        }

        private void AddGraphView()
        {
            LevelEditorGraphView graphView = new();

            graphView.StretchToParentSize();

            rootVisualElement.Add(graphView);
        }

        private void AddStyles()
        {
            StyleSheet styleSheet = (StyleSheet)EditorGUIUtility.Load("Variables.uss");

            rootVisualElement.styleSheets.Add(styleSheet);
        }
    }
}