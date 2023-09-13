using MyBox;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RoguetyCraft.Map.Data
{
    /// <summary>
    /// Serializable class that holds the data for a specific node in a level.
    /// </summary>
    [Serializable]
    public class NodeData
    {
        #region Public Variables

        /// <summary>
        /// The unique identifier for this node.
        /// </summary>
        [ReadOnly] public int NodeID;

        /// <summary>
        /// The type of room this node corresponds to.
        /// </summary>
        [ReadOnly] public RoomType Type;

        /// <summary>
        /// The position of this node in 2D space.
        /// </summary>
        [ReadOnly] public Vector2 Position;

        /// <summary>
        /// List of unique identifiers for input nodes connected to this node.
        /// </summary>
        [ReadOnly] public List<int> InputNodesID;

        /// <summary>
        /// List of unique identifiers for output nodes connected to this node.
        /// </summary>
        [ReadOnly] public List<int> OutputNodesID;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of NodeData with the specified parameters.
        /// </summary>
        /// <param name="nodeId">The unique identifier for this node.</param>
        /// <param name="type">The type of room this node corresponds to.</param>
        /// <param name="position">The position of this node in 2D space.</param>
        /// <param name="inputID">List of unique identifiers for input nodes connected to this node.</param>
        /// <param name="outputID">List of unique identifiers for output nodes connected to this node.</param>
        public NodeData(int nodeId, RoomType type, Vector2 position, List<int> inputID, List<int> outputID)
        {
            NodeID = nodeId;
            Type = type;
            Position = position;

            InputNodesID = new List<int>();
            InputNodesID.AddRange(inputID);

            OutputNodesID = new List<int>();
            OutputNodesID.AddRange(outputID);
        }

        #endregion
    }
}