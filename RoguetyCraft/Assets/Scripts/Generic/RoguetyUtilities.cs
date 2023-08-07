using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RoguetyCraft.Generic.Utility
{
    public static class RoguetyUtilities
    {
        public static bool GetComponent<T>(this GameObject gameObject, out T comp) where T : Component
        {
            if (!gameObject.TryGetComponent<T>(out T component))
            {
                component = gameObject.GetComponentInChildren<T>();
                if (component == null) component = gameObject.GetComponentInParent<T>();
            }
            comp = component;

            return component != null;
        }

        public static bool GetComponents<T>(this GameObject gameObject, out List<T> comp) where T : Component
        {
            List<T> list = new();

            list.AddRange(gameObject.GetComponents<T>());
            list.AddRange(gameObject.GetComponentsInChildren<T>());
            list.AddRange(gameObject.GetComponentsInParent<T>());
            comp = list;

            return list != null;
        }

        public static bool HasComponent<T>(this GameObject gameObject) where T : Component
        {
            if (!gameObject.TryGetComponent<T>(out T component))
            {
                component = gameObject.GetComponentInChildren<T>();
                if (component == null) component = gameObject.GetComponentInParent<T>();
            }

            return component != null;
        }

        public static List<Sprite> GetSpritesFromClip(AnimationClip clip)
        {
            var sprites = new List<Sprite>();
            if (clip != null)
            {
                foreach (var binding in AnimationUtility.GetObjectReferenceCurveBindings(clip))
                {
                    var keyframes = AnimationUtility.GetObjectReferenceCurve(clip, binding);
                    foreach (var frame in keyframes)
                    {
                        sprites.Add((Sprite)frame.value);
                    }
                }
            }
            return sprites;
        }
    }
}