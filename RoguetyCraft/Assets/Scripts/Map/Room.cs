using UnityEngine;
using MyBox;
using UnityEngine.Tilemaps;
using System;
using UnityEditor;
using RoguetyCraft.Map.Editor.Generic;

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
        /// The origin of this room in the tilemap.
        /// </summary>
        [ReadOnly] public Vector3Int Origin;

        /// <summary>
        /// The size of this room in the tilemap.
        /// </summary>
        [ReadOnly] public Vector3Int Size;

        [Foldout("Room Tiles", true)]
        [ReadOnly] public TileBase[] TileArray;
        [ReadOnly] public Vector3Int[] TilePositionArray;

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
                    SetRoom(tilemap.origin, tilemap.size, EditorMap.GetTiles(tilemap), EditorMap.GetIDFromObject(_refObj));
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
        public void SetRoom(Vector3Int _origin, Vector3Int _size, Editor.Generic.TileData[] _tiles, int objID)
        {
            Origin = _origin;
            Size = _size;

            TileArray = new TileBase[_tiles.Length];
            TilePositionArray = new Vector3Int[_tiles.Length];

            for (int i = 0; i < _tiles.Length; i++)
            {
                TileArray[i] = _tiles[i].Tile;
                TilePositionArray[i] = _tiles[i].Pos;
            }

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

            Vector3Int[] newPosArray = new Vector3Int[TilePositionArray.Length];
            for (int i = 0; i < newPosArray.Length; i++)
            {
                newPosArray[i] = TilePositionArray[i] + offset;
            }

            var roomTilemap = roomChild.GetComponent<Tilemap>();
            roomTilemap.SetTiles(newPosArray, TileArray);

            var roomTilemapCollider = roomChild.GetComponent<TilemapCollider2D>();
            roomTilemapCollider.usedByComposite = true;

            var roomRB = roomChild.GetComponent<Rigidbody2D>();
            roomRB.bodyType = RigidbodyType2D.Static;

            return room;
        }

        #endregion
    }
}
