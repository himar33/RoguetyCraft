using MyBox;
using System.Collections.Generic;
using UnityEngine;

namespace RoguetyCraft.Map.Data
{
    public class LevelData : ScriptableObject
    {
        [ReadOnly] public string FileName;
        [ReadOnly] public List<NodeData> Nodes;

        public void Init(string fileName)
        {
            FileName = fileName;
            Nodes = new List<NodeData>();
        }

        private void OnValidate()
        {
            if (FileName != name)
            {
                FileName = name;
            }
        }
    }
}