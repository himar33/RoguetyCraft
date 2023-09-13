using MyBox;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Reflection;
using RoguetyCraft.Map.Data;

namespace RoguetyCraft.Map.Editor.Generic
{
    /// <summary>
    /// Struct to hold information about a Tile in a Tilemap.
    /// </summary>
    public struct TileData
    {
        /// <summary>
        /// The position of the Tile.
        /// </summary>
        public Vector3Int Pos;

        /// <summary>
        /// The TileBase object representing the Tile.
        /// </summary>
        public TileBase Tile;

        /// <summary>
        /// Initializes a new TileData struct.
        /// </summary>
        /// <param name="_pos">Position of the Tile.</param>
        /// <param name="_tile">TileBase object.</param>
        public TileData(Vector3Int _pos, TileBase _tile)
        {
            Pos = _pos;
            Tile = _tile;
        }
    }

    /// <summary>
    /// Static class to provide utility functions for editing maps in the Editor.
    /// </summary>
    public static class EditorMap
    {
        #region Menu Items

        /// <summary>
        /// Creates a room template based on a selected Tilemap.
        /// </summary>
        [MenuItem("GameObject/RoguetyCraft/Create Room", priority = 1)]
        public static void CreateRoomTemplate()
        {
            Tilemap tilemap = Selection.activeTransform.GetComponent<Tilemap>();

            if (tilemap == null)
            {
                EditorUtility.DisplayDialog(
                    "Select Tilemap",
                    "You Must Select a Valid Tilemap!",
                    "Ok");
                return;
            }

            string path = EditorUtility.SaveFilePanelInProject("Save tilemap", tilemap.name + "asset", "asset",
            "Please enter a file name to save the tilemap to");

            if (path.Length != 0)
            {
                var roomObject = ScriptableObject.CreateInstance<Room>();
                int localId = GetIDFromObject(tilemap.gameObject);
                roomObject.SetRoom(tilemap.origin, tilemap.size, GetTiles(tilemap), localId);

                AssetDatabase.CreateAsset(roomObject, path);
            }
        }

        /// <summary>
        /// Validates whether a Tilemap is selected.
        /// </summary>
        [MenuItem("GameObject/RoguetyCraft/Create Room", true)]
        public static bool ValidateTilemapSelected()
        {
            if (Selection.activeTransform != null)
            {
                return Selection.activeTransform.GetComponent<Tilemap>() != null;
            }
            else return false;
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Gets the local identifier of a Unity object.
        /// </summary>
        /// <param name="tilemap">The Unity object.</param>
        /// <returns>The local identifier of the object.</returns>
        public static int GetIDFromObject(UnityEngine.Object tilemap)
        {
            PropertyInfo inspectorModeInfo =
                            typeof(SerializedObject).GetProperty("inspectorMode", BindingFlags.NonPublic | BindingFlags.Instance);

            SerializedObject serializedObject = new SerializedObject(tilemap);
            inspectorModeInfo.SetValue(serializedObject, InspectorMode.Debug, null);

            SerializedProperty localIdProp =
                serializedObject.FindProperty("m_LocalIdentfierInFile");   //note the misspelling!

            int localId = localIdProp.intValue;
            return localId;
        }

        /// <summary>
        /// Fetches all tiles in a Tilemap as an array of TileData structs.
        /// </summary>
        /// <param name="tilemap">The Tilemap to process.</param>
        /// <returns>An array of TileData structs.</returns>
        public static TileData[] GetTiles(this Tilemap tilemap)
        {
            List<TileData> tiles = new List<TileData>();

            for (int y = tilemap.origin.y; y < (tilemap.origin.y + tilemap.size.y); y++)
            {
                for (int x = tilemap.origin.x; x < (tilemap.origin.x + tilemap.size.x); x++)
                {
                    TileBase tile = tilemap.GetTile<TileBase>(new Vector3Int(x, y, 0));
                    if (tile != null)
                    {
                        TileData tileData = new TileData(new Vector3Int(x, y, 0), tile);
                        tiles.Add(tileData);
                    }
                }
            }
            return tiles.ToArray();
        }

        #endregion
    }
}

