#if UNITY_EDITOR
using RoguetyCraft.Map.Data;
using UnityEditor;
using UnityEngine;

namespace RoguetyCraft.Generic.CustomEditors
{
    [CustomEditor(typeof(Room))]
    public class RoomEditor : Editor
    {
        private Room _room;

        private void OnEnable()
        {
            _room = target as Room;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}
#endif