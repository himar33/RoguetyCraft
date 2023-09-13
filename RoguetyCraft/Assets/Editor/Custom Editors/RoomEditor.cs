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

            GUILayout.Space(20f);

            GUIStyle style = GUI.skin.box;
            style.alignment = TextAnchor.MiddleCenter;

            GUILayout.BeginVertical();

            GUILayout.Label("Item preview", GUILayout.Width(100f), GUILayout.Height(16f));
            //GUILayout.Label("", GUILayout.Width(100f), GUILayout.Height(100f));
            //Texture2D itemTexture = AssetPreview.GetMiniThumbnail(_room.TileArray[0]);
            //GUI.Box(GUILayoutUtility.GetLastRect(), itemTexture, style);

            GUILayout.EndVertical();
        }
    }
}
#endif