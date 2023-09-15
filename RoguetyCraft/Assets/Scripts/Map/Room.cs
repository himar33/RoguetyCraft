using UnityEngine;
using MyBox;
using UnityEngine.Tilemaps;
using System;
using UnityEditor;
using RoguetyCraft.Map.Editor.Generic;
using System.Collections.Generic;

using TileData = RoguetyCraft.Map.Editor.Generic.TileData;

namespace RoguetyCraft.Map.Data
{
    /// <summary>
    /// Enumeration representing the different types of rooms in the game.
    /// </summary>
    [Serializable]
    public enum RoomType
    {
        NORMAL,
        START,
        DOOR_LOCKED,
        TREASURE,
        SHOP,
        TELEPORT,
        EXIT
    }

    [Serializable]
    public enum RoomDirection
    {
        NULL,
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    [Serializable]
    public struct RoomExit
    {
        public Vector3Int StartPosition;
        public Vector3Int EndPosition;
        public RoomDirection ExitDirection;
    }

    /// <summary>
    /// Scriptable object that holds data for a specific room type and layout.
    /// </summary>
    public class Room : ScriptableObject
    {
        #region Public Variables

        [Foldout("Room Data", true)]
        [Layer] public int RoomLayer;

        /// <summary>
        /// The type of this room.
        /// </summary>
        public RoomType RoomType;

        /// <summary>
        /// The size of this room in the tilemap.
        /// </summary>
        [ReadOnly] public Vector3Int Size;

        [ReadOnly] public List<RoomExit> DetectedExits = new();

        [Foldout("Room Tiles", true)]
        [ReadOnly] public TileData[] TileArray;
        [ReadOnly] public List<TileData> EdgeTiles;

        #endregion

        #region Private Variables

        private int _refID;

        #endregion

        #region Methods

        /// <summary>
        /// Reloads the tilemap array for this room from a GameObject reference.
        /// </summary>
        [ButtonMethod]
        public void ReloadTileMapArray()
        {
            GameObject _refObj = EditorUtility.InstanceIDToObject(_refID) as GameObject;
            if (_refObj != null)
            {
                Tilemap tilemap = _refObj.GetComponent<Tilemap>();
                if (tilemap != null)
                {
                    var (tiles, roomSize) = tilemap.GetTilesAndSize();
                    SetRoom(roomSize, tiles, EditorMap.GetIDFromObject(_refObj));
                }
                else
                {
                    EditorUtility.DisplayDialog(
                    "Reloading Tilemap",
                    "The current object reference doesn't have a tilemap component!",
                    "Ok");
                }
            }
            else
            {
                EditorUtility.DisplayDialog(
                    "Reloading Tilemap",
                    "The object reference is null or non-exists!",
                    "Ok");
            }
        }

        /// <summary>
        /// Sets the room parameters.
        /// </summary>
        public void SetRoom(Vector3Int _size, TileData[] _tiles, int objID)
        {
            Size = _size;

            TileArray = _tiles;

            DetectExits();

            _refID = objID;
        }

        /// <summary>
        /// Instantiates a room GameObject based on this room's data.
        /// </summary>
        public GameObject InstantiateRoom(Vector3Int offset, Transform parent)
        {
            GameObject room;
            GameObject roomChild;

            if (parent.childCount == 0)
            {
                room = new(name);
                room.transform.parent = parent;

                roomChild = new("Tilemap", typeof(Tilemap), typeof(TilemapRenderer), typeof(TilemapCollider2D), typeof(CompositeCollider2D));
                roomChild.transform.parent = room.transform;
            }
            else
            {
                room = parent.GetChild(0).gameObject;
                roomChild = room.transform.GetChild(0).gameObject;
            }
            roomChild.layer = RoomLayer;

            var roomTilemap = roomChild.GetComponent<Tilemap>();
            for (int i = 0; i < TileArray.Length; i++)
            {
                roomTilemap.SetTile(TileArray[i].Pos + offset, TileArray[i].Tile);
            }

            var roomTilemapCollider = roomChild.GetComponent<TilemapCollider2D>();
            roomTilemapCollider.usedByComposite = true;

            var roomRB = roomChild.GetComponent<Rigidbody2D>();
            roomRB.bodyType = RigidbodyType2D.Static;

            return room;
        }

        public void DetectExits()
        {
            DetectedExits.Clear();

            EdgeTiles = GetTilesFromEdge();

            bool isCreatingExit = false;
            RoomExit currExit = new();
            foreach (TileData currTile in EdgeTiles)
            {
                if (currTile.Tile != null)
                {
                    if (isCreatingExit)
                    {
                        isCreatingExit = false;
                        currExit.EndPosition = currTile.Pos;
                        currExit.ExitDirection = GetDirectionByPosition(currTile.Pos);
                        DetectedExits.Add(currExit);
                    }
                    else continue;
                }
                else
                {
                    if (isCreatingExit) continue;
                    else
                    {
                        isCreatingExit = true;

                        currExit = new();
                        currExit.StartPosition = currTile.Pos;
                    }
                }
            }
        }

        private RoomDirection GetDirectionByPosition(Vector3Int tilePos)
        {
            if(tilePos.y == Size.y - 1)
            {
                return RoomDirection.UP;
            }
            else if (tilePos.y == 0)
            {
                return RoomDirection.DOWN;
            }
            else if (tilePos.x == 0)
            {
                return RoomDirection.LEFT;
            }
            else if (tilePos.x == Size.x - 1)
            {
                return RoomDirection.RIGHT;
            }
            return RoomDirection.NULL;
        }

        private List<TileData> GetTilesFromEdge()
        {
            List<TileData> edgeTiles = new List<TileData>();

            for (int x = 0; x < Size.x; x++)
            {
                Vector3Int position = new (x, 0, 0);
                edgeTiles.Add(GetTileAtPosition(position));
            }

            for (int y = 1; y < Size.y; y++)
            {
                Vector3Int position = new (Size.x - 1, y, 0);
                edgeTiles.Add(GetTileAtPosition(position));
            }

            for (int x = Size.x - 2; x >= 0; x--)
            {
                Vector3Int position = new (x, Size.y - 1, 0);
                edgeTiles.Add(GetTileAtPosition(position));
            }

            for (int y = Size.y - 2; y >= 1; y--)
            {
                Vector3Int position = new (0, y, 0);
                edgeTiles.Add(GetTileAtPosition(position));
            }

            return edgeTiles;
        }

        private TileData GetTileAtPosition(Vector3Int position)
        {
            int index = Array.FindIndex(TileArray, pos => pos.Pos == position);
            if (index != -1)
            {
                return TileArray[index];
            }
            return new TileData(position, null);
        }

        #endregion
    }
}
