using MyBox;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace RoguetyCraft.Map.Data
{
    /// <summary>
    /// Class to hold and manage level data, serializable for Unity's asset system.
    /// </summary>
    public class LevelData : ScriptableObject
    {
        #region Public Variables

        /// <summary>
        /// The name of the file where this LevelData object will be saved.
        /// </summary>
        [ReadOnly] public string FileName;

        /// <summary>
        /// List of NodeData that make up this level.
        /// </summary>
        [ReadOnly] public List<NodeData> Nodes;

        #endregion

        #region Public Methods

        /// <summary>
        /// Initializes a new instance of LevelData.
        /// </summary>
        /// <param name="fileName">The name of the file to save this LevelData to.</param>
        public void Init(string fileName)
        {
            FileName = fileName;
            Nodes = new List<NodeData>();
        }

        public NodeData GetNodeByIndex(int nodeID)
        {
            return Nodes[nodeID];
        }

        #endregion

        #region Unity Lifecycle Methods

        private void OnValidate()
        {
            if (FileName != name)
            {
                FileName = name;
            }
        }

        #endregion
    }
}