#if UNITY_EDITOR
using RoguetyCraft.Map.Data;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace RoguetyCraft.Map.Editor.LevelGraph
{
    public class LevelNode : Node
    {
        public string RoomName { get; set; }
        public int RoomIndex { get; set; }
        public RoomType Type { get; set; }

        public List<int> InputRooms { get; set; } = new();
        public List<int> OutputRooms { get; set; } = new();

        private LevelEditorGraphView _graphView;
        private TextElement _indexField;
        private TextElement _inputNodeInfo;
        private TextElement _outputNodeInfo;
        private Port _inputPort;
        private Port _outputPort;

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

            _indexField = new()
            {
                text = $"Index: {RoomIndex}"
            };
            contentContainer.Add(_indexField);

            EnumField typeField = new("Room type: ", Type);
            contentContainer.Add(typeField);

            _inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            _inputPort.portName = "Room From";
            inputContainer.Add(_inputPort);

            _outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
            _outputPort.portName = "Room To";
            outputContainer.Add(_outputPort);

            _inputNodeInfo = new()
            {
                text = "Last Rooms ID:"
            };
            contentContainer.Add(_inputNodeInfo);

            _outputNodeInfo = new()
            {
                text = "Next Rooms ID:"
            };
            contentContainer.Add(_outputNodeInfo);

            RefreshExpandedState();
        }

        public void OnNodeChange()
        {
            _indexField.text = $"Index: {RoomIndex}";
        }

        public void AddNodeConnection()
        {
            string inputTextID = "Last Rooms ID:";
            foreach (int inputRoom in InputRooms)
            {
                inputTextID += $" [{inputRoom}]";
            }
            _inputNodeInfo.text = inputTextID;

            string outputTextID = "Next Rooms ID:";
            foreach (int outputRoom in OutputRooms)
            {
                outputTextID += $" [{outputRoom}]";
            }
            _outputNodeInfo.text = outputTextID;
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

        public void DeleteNode(int nodeToDelete)
        {
            if (InputRooms.Exists(x => x == nodeToDelete)) InputRooms.Remove(nodeToDelete);
            else if (OutputRooms.Exists(x => x == nodeToDelete)) OutputRooms.Remove(nodeToDelete);
            AddNodeConnection();
        }
    }
}
#endif