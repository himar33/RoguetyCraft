using System.Collections.Generic;
using UnityEngine;
using MyBox;
using RoguetyCraft.Map.Data;

using EditorMap = RoguetyCraft.Map.Editor.Generic.EditorMap;
using System.Linq;

namespace RoguetyCraft.Map.Manager
{
    [RequireComponent(typeof(Grid))]
    public class MapManager : DesignPatterns.Singleton.RCSingleton<MapManager>
    {
        [Separator("Map settings")]
        [SerializeField] private LevelData _levelData;
        [SerializeField] private List<Room> _roomDataBase = new();

        private List<GameObject> _currGameObjectList = new();
        private Vector3Int _currentPosition;

        [ButtonMethod]
        public void GenerateMap()
        {
            //Destroy and clear the previous level
            _currGameObjectList.Clear();
            int childs = transform.childCount;
            for (int i = childs - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }

            //Find the START room
            int startCount = _levelData.Nodes.FindAll(x => x.Type == RoomType.START).Count;
            if (startCount != 1)
            {
                Debug.LogError($"{_levelData.FileName} (LevelData) file has more than one START room. For the moment only one initial room is accepted.");
                return;
            }
            NodeData startRoom = _levelData.Nodes.Find(x => x.Type == RoomType.START);
            _currentPosition = Vector3Int.zero;
            GenerateRoom(startRoom, _currentPosition);
        }

        [ButtonMethod]
        public void ClearMap()
        {
            _currGameObjectList.Clear();
            int childs = transform.childCount;
            for (int i = childs - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }

        private void GenerateRoom(NodeData node, Vector3Int position, RoomDirection previousDirection = RoomDirection.NULL)
        {
            Room nodeRoom = GetRandomRoomByTypeAndDirection(node.Type, EditorMap.GetOppositeDirection(previousDirection));
            if (nodeRoom == null)
            {
                Debug.LogError($"It does not exists a room of type {node.Type} int the data base");
                return;
            }

            GameObject roomInstance = nodeRoom.InstantiateRoom(position, transform);
            _currGameObjectList.Add(roomInstance);

            RoomDirection exitDirection = RoomDirection.NULL;

            List<RoomDirection> exitDirections = new();
            foreach (RoomExit item in nodeRoom.DetectedExits)
            {
                exitDirections.Add(item.ExitDirection);
            }

            if (previousDirection != RoomDirection.NULL)
            {
                List<RoomDirection> posibleExits = exitDirections.FindAll(x => x != EditorMap.GetOppositeDirection(previousDirection));
                exitDirection = posibleExits.GetRandom();
            }
            else exitDirection = exitDirections.GetRandom();

            if (node.OutputNodesID.Count > 0)
            {
                _currentPosition += GetOffsetBySizeAndDirection(nodeRoom.Size, exitDirection);

                int index = node.OutputNodesID.GetRandom();

                GenerateRoom(_levelData.GetNodeByIndex(index), _currentPosition, exitDirection);
            }
        }

        private Room GetRandomRoomByTypeAndDirection(RoomType roomType, RoomDirection previousDirection)
        {
            List<Room> roomList = new();
            if (previousDirection == RoomDirection.NULL)
            {
                roomList = _roomDataBase.FindAll(x => x.RoomType == roomType);
            }
            else
            {
                roomList = _roomDataBase.FindAll(x => x.RoomType == roomType && x.DetectedExits.Find(x => x.ExitDirection == previousDirection).ExitDirection == previousDirection);
            }
            if (roomList.Count == 0) { return null; }

            int roomIndex = Random.Range(0, roomList.Count);

            return roomList[roomIndex];
        }

        private Vector3Int GetOffsetBySizeAndDirection(Vector3Int size, RoomDirection direction)
        {
            switch (direction)
            {
                case RoomDirection.NULL:
                    return Vector3Int.zero;
                case RoomDirection.UP:
                    return new Vector3Int(0, size.y, 0);
                case RoomDirection.DOWN:
                    return new Vector3Int(0, -size.y, 0);
                case RoomDirection.LEFT:
                    return new Vector3Int(-size.x, 0, 0);
                case RoomDirection.RIGHT:
                    return new Vector3Int(size.x, 0, 0);
                default:
                    return Vector3Int.zero;
            }
        }
    }
}
