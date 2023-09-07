using MyBox;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RoguetyCraft.Map.Data
{
    [Serializable]
    public class NodeData
    {
        [ReadOnly] public int NodeID;
        [ReadOnly] public RoomType Type;
        [ReadOnly] public Vector2 Position;

        [ReadOnly] public List<int> InputNodesID;
        [ReadOnly] public List<int> OutputNodesID;

        public NodeData(int nodeId, RoomType type, Vector2 position, List<int>inputID, List<int> outputID)
        {
            NodeID = nodeId;
            Type = type;
            Position = position;

            InputNodesID = new List<int>();
            InputNodesID.AddRange(inputID);

            OutputNodesID = new List<int>();
            OutputNodesID.AddRange(outputID);
        }
    }
}