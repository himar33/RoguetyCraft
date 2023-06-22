using RoguetyCraft.Map.Generic;
using System.Collections.Generic;
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

        public void Init(int index, RoomType type, Vector2 position)
        {
            RoomName = $"Room_{index}";
            RoomIndex = index;
            Type = type;
            SetPosition(new Rect(position, Vector2.zero));
        }
        public void Draw()
        {
            TextField titleField = new()
            {
                value = RoomName
            };
            titleContainer.Insert(0, titleField);

            TextElement indexField = new()
            {
                text = RoomIndex.ToString()
            };
            contentContainer.Add(indexField);

            EnumField typeField = new("Room type: ", Type);
            contentContainer.Add(typeField);

            Port inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            inputPort.portName = "Room From";
            inputContainer.Add(inputPort);

            Port outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
            outputPort.portName = "Room to";
            outputContainer.Add(outputPort);

            RefreshExpandedState();
        }
    }
}
