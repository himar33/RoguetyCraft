using System.Collections.Generic;
using UnityEngine;
using MyBox;
using RoguetyCraft.Map.Data;

namespace RoguetyCraft.Map.Manager
{
    [RequireComponent(typeof(Grid))]
    public class MapManager : DesignPatterns.Singleton.RCSingleton<MapManager>
    {
        [Separator("Map settings")]
        [SerializeField] private List<Room> _roomList = new();

        private List<GameObject> _currGameObjectList = new();

        [ButtonMethod]
        public void GenerateMap()
        {
            _currGameObjectList.Clear();
            int childs = transform.childCount;
            for (int i = childs - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }

            Vector3Int currPos = _roomList[0].Origin;
            foreach (var room in _roomList)
            {
                _currGameObjectList.Add(room.InstantiateRoom(currPos, transform));
                currPos.x += room.Size.x;
            }
        }
    }
}
