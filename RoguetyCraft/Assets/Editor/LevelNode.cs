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
        public RoomType Type { get; set; }
        public List<RoomType> LastRooms = new();
        public List<RoomType> NextRooms = new();

        public void Init(Vector2 position)
        {
            Type = RoomType.NORMAL;
            SetPosition(new Rect(position, Vector2.zero));
        }
        public void Draw()
        {
            EnumField typeField = new("Room type: ", RoomType.NORMAL);
            titleContainer.Insert(0, typeField);

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
