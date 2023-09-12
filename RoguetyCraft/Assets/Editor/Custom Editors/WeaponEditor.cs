using MyBox;
using RoguetyCraft.Items.Weapon;
using UnityEditor;
using UnityEngine;

namespace RoguetyCraft.Generic.CustomEditors
{
    [CustomEditor(typeof(ItemWeapon))]
    public class WeaponEditor : Editor
    {
        private ItemWeapon _weapon;

        private void OnEnable()
        {
            _weapon = target as ItemWeapon;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (_weapon.Sprite == null) return;
            GUILayout.Space(20f);
            GUILayout.BeginHorizontal();

            GUIStyle style = GUI.skin.box;
            style.alignment = TextAnchor.MiddleCenter;

            GUILayout.BeginVertical();
            GUILayout.Label("Item preview", GUILayout.Width(100f), GUILayout.Height(16f));
            GUILayout.Label("", GUILayout.Width(100f), GUILayout.Height(100f));
            Texture2D itemTexture = AssetPreview.GetAssetPreview(_weapon.Sprite);
            GUI.Box(GUILayoutUtility.GetLastRect(), itemTexture, style);
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("Gun preview", GUILayout.Width(100f), GUILayout.Height(16f));
            GUILayout.Label("", GUILayout.Width(100f), GUILayout.Height(100f));
            Texture2D gunTexture = AssetPreview.GetAssetPreview(_weapon.GunSprite);
            GUI.Box(GUILayoutUtility.GetLastRect(), gunTexture, style);
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("Bullet preview", GUILayout.Width(100f), GUILayout.Height(16f));
            GUILayout.Label("", GUILayout.Width(100f), GUILayout.Height(100f));
            Texture2D bulletTexture = AssetPreview.GetAssetPreview(_weapon.AnimationSprites[0]);
            GUI.Box(GUILayoutUtility.GetLastRect(), bulletTexture, style);
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
        }
    }
}
