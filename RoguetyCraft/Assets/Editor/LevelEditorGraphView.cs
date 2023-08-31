using RoguetyCraft.Map.Generic;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace RoguetyCraft.Map.Editor.LevelGraph
{
    public class LevelEditorGraphView : GraphView
    {
        private LevelEditorWindow _editorWindow;
        private LevelSearchWindow _searchWindow;

        private List<LevelNode> _nodesList = new List<LevelNode>();

        public LevelEditorGraphView(LevelEditorWindow editorWindow)
        {
            _editorWindow = editorWindow;

            AddManipulators();
            AddSearchWindow();
            AddGridBackground();

            OnElementsDeleted();
            OnGraphViewChanged();

            AddStyles();
        }

        private void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(CreateNodeContextualMenu());
        }

        private void AddSearchWindow()
        {
            if (_searchWindow == null)
            {
                _searchWindow = ScriptableObject.CreateInstance<LevelSearchWindow>();
                _searchWindow.Init(this);
            }
            nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
        }

        private void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();
            Insert(0, gridBackground);
        }

        private void OnElementsDeleted()
        {
            deleteSelection = (operationName, askUser) =>
            {
                Type edgeType = typeof(Edge);

                List<LevelNode> nodesToDelete = new List<LevelNode>();
                List<Edge> edgesToDelete = new List<Edge>();

                foreach (GraphElement selectedElement in selection)
                {
                    if (selectedElement is LevelNode node)
                    {
                        nodesToDelete.Add(node);
                        continue;
                    }

                    if (selectedElement.GetType() == edgeType)
                    {
                        Edge edge = (Edge)selectedElement;
                        edgesToDelete.Add(edge);
                        continue;
                    }
                }

                DeleteElements(edgesToDelete);

                foreach (LevelNode nodeToDelete in nodesToDelete)
                {
                    _nodesList.Remove(nodeToDelete);
                    foreach (LevelNode nodeInList in _nodesList)
                    {
                        int index = _nodesList.IndexOf(nodeInList); 
                        nodeInList.RoomIndex = index;
                        nodeInList.Redraw();
                    }

                    nodeToDelete.DeleteAllPorts();
                    RemoveElement(nodeToDelete);
                }
            };
        }

        private void OnGraphViewChanged()
        {
            graphViewChanged = (changes) =>
            {
                if (changes.edgesToCreate != null)
                {
                    foreach (Edge edge in changes.edgesToCreate)
                    {
                    }
                }

                if (changes.elementsToRemove != null)
                {
                    Type edgeType = typeof(Edge);

                    foreach (GraphElement element in changes.elementsToRemove)
                    {
                    }
                }

                return changes;
            };
        }

        private void AddStyles()
        {
            StyleSheet styleSheet = (StyleSheet) EditorGUIUtility.Load("LevelEditorStyle.uss");
            styleSheets.Add(styleSheet);
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();

            ports.ForEach(port =>
            {
                if (startPort == port) return;
                if (startPort.node == port.node) return;
                if (startPort.direction == port.direction) return;
                compatiblePorts.Add(port);
            });

            return compatiblePorts;
        }

        public Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isSearchWindow = false)
        {
            Vector2 worldMousePosition = mousePosition;

            if (isSearchWindow)
            {
                worldMousePosition -= _editorWindow.position.position;
            }

            Vector2 localMousePosition = contentViewContainer.WorldToLocal(worldMousePosition);
            return localMousePosition;
        }

        private IManipulator CreateNodeContextualMenu()
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction("Add Room", actionEvent => AddElement(CreateNode(GetLocalMousePosition(actionEvent.eventInfo.localMousePosition))))
            );

            return contextualMenuManipulator;
        }

        public LevelNode CreateNode(Vector2 position, RoomType type = RoomType.NORMAL)
        {
            LevelNode node = new();

            node.Init(_nodesList.Count, this, type, position);
            _nodesList.Add(node);
            node.Draw();

            return node;
        }
    }
}
