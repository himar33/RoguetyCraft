using MyBox;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Reflection;
using RoguetyCraft.Map.Data;

namespace RoguetyCraft.Map.Editor.Generic
{
    public struct TileData
    {
        public Vector3Int Pos;
        public TileBase Tile;
        public TileData(Vector3Int _pos, TileBase _tile)
        {
            Pos = _pos;
            Tile = _tile;
        }
    }
    public static class EditorMap
    {
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

        [MenuItem("GameObject/RoguetyCraft/Create Room", true)]
        public static bool ValidateTilemapSelected()
        {
            if (Selection.activeTransform != null)
            {
                return Selection.activeTransform.GetComponent<Tilemap>() != null;
            }
            else return false;
        }

        public static TileData[] GetTiles(this Tilemap tilemap)
        {
            List<TileData> tiles = new();

            for (int y = tilemap.origin.y; y < (tilemap.origin.y + tilemap.size.y); y++)
            {
                for (int x = tilemap.origin.x; x < (tilemap.origin.x + tilemap.size.x); x++)
                {
                    TileBase tile = tilemap.GetTile<TileBase>(new Vector3Int(x, y, 0));
                    if (tile != null)
                    {
                        TileData tileData = new(new Vector3Int(x, y, 0), tile);
                        tiles.Add(tileData);
                    }
                }
            }
            return tiles.ToArray();
        }
    }
}

