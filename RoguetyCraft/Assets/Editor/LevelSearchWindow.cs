#if UNITY_EDITOR
using RoguetyCraft.Map.Data;
using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace RoguetyCraft.Map.Editor.LevelGraph
{
    public class LevelSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private LevelEditorGraphView _view;
        public void Init(LevelEditorGraphView graphView)
        {
            _view = graphView;
        }
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> entries = new()
        {
            new SearchTreeGroupEntry(new GUIContent("Room Nodes"), 1),
        };
            foreach (RoomType enumValue in Enum.GetValues(typeof(RoomType)))
            {
                SearchTreeEntry entry = new(new GUIContent(enumValue.ToString()))
                {
                    level = 2,
                    userData = enumValue,
                };
                entries.Add(entry);
            }
            return entries;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            Vector2 localMousePosition = _view.GetLocalMousePosition(context.screenMousePosition, true);
            LevelNode node = _view.CreateNode(localMousePosition, (RoomType)SearchTreeEntry.userData);
            _view.AddElement(node);
            return true;
        }
    }
}
#endif