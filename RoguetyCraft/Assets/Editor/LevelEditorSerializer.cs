#if UNITY_EDITOR
using RoguetyCraft.Map.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace RoguetyCraft.Map.Editor.LevelGraph
{
    public static class LevelEditorSerializer
    {
        private static LevelEditorGraphView _graphView;

        private static string _graphFileName;
        private static List<LevelNode> _nodes;
        private static List<LevelNode> _loadedNodes;

        public static void Init(LevelEditorGraphView graphView, string graphName)
        {
            _graphView = graphView;

            _graphFileName = graphName;
            _nodes = new List<LevelNode>();
            _loadedNodes = new List<LevelNode>();
        }

        #region Save Methods
        public static void Save()
        {
            CreateDefaultFolders();
            GetNodes();

            LevelData levelData = CreateAsset<LevelData>("Assets/Demo/LevelGraph", $"{_graphFileName}");
            levelData.Init(_graphFileName);

            SaveNodes(levelData);

            SaveAsset(levelData); 
        }

        private static void CreateDefaultFolders()
        {
            CreateFolder("Assets/Demo", "LevelGraph");
        }

        private static void GetNodes()
        {
            _graphView.graphElements.ForEach(graphElement =>
            {
                if (graphElement is LevelNode node)
                {
                    _nodes.Add(node);

                    return;
                }
            });
        }

        private static void SaveNodes(LevelData levelData)
        {
            foreach (LevelNode node in _nodes)
            {
                SaveNodeToGraph(node, levelData);
            }
        }

        private static void SaveNodeToGraph(LevelNode node, LevelData levelData)
        {
            List<int> inputNodesID = new List<int>();
            foreach (int inputNode in node.InputRooms)
            {
                inputNodesID.Add(inputNode);
            }

            List<int> outputNodesID = new List<int>();
            foreach (int outputNode in node.OutputRooms)
            {
                outputNodesID.Add(outputNode);
            }

            NodeData nodeData = new(node.RoomIndex, node.Type, node.GetPosition().position, inputNodesID, outputNodesID);

            levelData.Nodes.Add(nodeData);
        }

        public static void SaveAsset(UnityEngine.Object asset)
        {
            EditorUtility.SetDirty(asset);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        #endregion

        #region Load Methods
        public static void Load()
        {
            LevelData levelData = LoadAsset<LevelData>("Assets/Demo/LevelGraph", _graphFileName);

            if (levelData == null)
            {
                EditorUtility.DisplayDialog(
                    "Couldn't find the file!",
                    "The file mentioned with the following path couldn't be found:\n\n" +
                    $"\"Assets/Demo/LevelGraph/{_graphFileName}\".\n\n" +
                    "Make sure you choose and existent file placed at the folder path mentioned above.",
                    "Thanks!"
                );

                return;
            }
            LevelEditorWindow.UpdateFileName(levelData.FileName);

            LoadNodes(levelData.Nodes);
            LoadNodesEdges();
        }

        private static void LoadNodes(List<NodeData> nodes)
        {
            foreach(NodeData nodeData in nodes)
            {
                LevelNode node = _graphView.CreateNode(nodeData.Position, nodeData.Type, nodeData.NodeID);

                node.InputRooms = nodeData.InputNodesID;
                node.OutputRooms = nodeData.OutputNodesID;

                node.AddNodeConnection();

                _graphView.AddElement(node);
                _loadedNodes.Add(node);
            }
        }

        private static void LoadNodesEdges()
        {
            foreach (LevelNode loadedNode in _loadedNodes)
            {
                foreach (int inputID in loadedNode.InputRooms)
                {
                    LevelNode lastRoom = _loadedNodes.Find(x => x.RoomIndex == inputID);
                    Port lastNodeOutputPort = (Port)lastRoom.outputContainer.Children().First();

                    Port loadedNodeInputPort = (Port)loadedNode.inputContainer.Children().First();
                    Edge edge = loadedNodeInputPort.ConnectTo(lastNodeOutputPort);

                    _graphView.AddElement(edge);
                    loadedNode.RefreshPorts();
                }
            }
        }
        #endregion

        #region Utility Methods
        private static void CreateFolder(string parentPath, string newFolder)
        {
            if (AssetDatabase.IsValidFolder($"{parentPath}/{newFolder}"))
            {
                return;
            }

            AssetDatabase.CreateFolder(parentPath, newFolder);
        }

        public static T CreateAsset<T>(string path, string assetName) where T : ScriptableObject
        {
            string fullPath = $"{path}/{assetName}.asset";
            T asset = LoadAsset<T>(path, assetName);
            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(asset, fullPath);
            }
            return asset;
        }

        public static T LoadAsset<T>(string path, string assetName) where T : ScriptableObject
        {
            string fullPath = $"{path}/{assetName}.asset";

            return AssetDatabase.LoadAssetAtPath<T>(fullPath);
        }
        #endregion
    }
}
#endif