using NUnit.Framework.Interfaces;
using RoguetyCraft.Map.Generic;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace RoguetyCraft.Map.Editor.LevelGraph
{
    public class LevelNode : Node
    {
        public string RoomName;
        public int RoomIndex;
        public RoomType Type { get; set; }
        public List<RoomType> LastRooms = new();
        public List<RoomType> NextRooms = new();

        private LevelEditorGraphView _graphView;
        private TextElement IndexField;
        private Port inputPort;
        private Port outputPort;

        public void Init(int index, LevelEditorGraphView graphView, RoomType type, Vector2 position)
        {
            RoomName = type.ToString();
            RoomIndex = index;
            Type = type;
            SetPosition(new Rect(position, Vector2.zero));

            _graphView = graphView;
        }
        public void Draw()
        {
            TextElement titleField = new()
            {
                text = RoomName
            };
            titleContainer.Insert(0, titleField);

            IndexField = new()
            {
                text = $"Index: {RoomIndex}"
            };
            contentContainer.Add(IndexField);

            EnumField typeField = new("Room type: ", Type);
            contentContainer.Add(typeField);

            inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            inputPort.portName = "Room From";
            inputContainer.Add(inputPort);

            outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
            outputPort.portName = "Room To";
            outputContainer.Add(outputPort);

            RefreshExpandedState();
        }

        public void Redraw()
        {
            IndexField.text = $"Index: {RoomIndex}";
        }

        public void DeleteAllPorts()
        {
            DeleteInputPorts();
            DeleteOutputPorts();
        }

        private void DeleteInputPorts()
        {
            DeletePorts(inputContainer);
        }

        private void DeleteOutputPorts()
        {
            DeletePorts(outputContainer);
        }

        private void DeletePorts(VisualElement portContainer)
        {
            foreach (Port port in portContainer.Children())
            {
                if (!port.connected) continue;

                _graphView.DeleteElements(port.connections);
            }
        }
    }
}
