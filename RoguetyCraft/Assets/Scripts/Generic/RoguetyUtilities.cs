using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RoguetyCraft.Generic.Utility
{
    /// <summary>
    /// Provides a set of utility methods for the Roguety game.
    /// </summary>
    public static class RoguetyUtilities
    {
        #region Component Retrieval

        /// <summary>
        /// Attempts to retrieve a component from a GameObject, its children, or its parent.
        /// </summary>
        /// <typeparam name="T">Type of the component.</typeparam>
        /// <param name="gameObject">The GameObject to search on.</param>
        /// <param name="comp">The retrieved component.</param>
        /// <returns>True if the component was found; false otherwise.</returns>
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

        /// <summary>
        /// Retrieves all components of a given type from a GameObject, its children, and its parent.
        /// </summary>
        /// <typeparam name="T">Type of the component.</typeparam>
        /// <param name="gameObject">The GameObject to search on.</param>
        /// <param name="comp">The list of retrieved components.</param>
        /// <returns>True if any components were found; false otherwise.</returns>
        public static bool GetComponents<T>(this GameObject gameObject, out List<T> comp) where T : Component
        {
            List<T> list = new();

            list.AddRange(gameObject.GetComponents<T>());
            list.AddRange(gameObject.GetComponentsInChildren<T>());
            list.AddRange(gameObject.GetComponentsInParent<T>());
            comp = list;

            return list != null;
        }

        /// <summary>
        /// Checks if a GameObject has a specific component, either in itself, its children, or its parent.
        /// </summary>
        /// <typeparam name="T">Type of the component.</typeparam>
        /// <param name="gameObject">The GameObject to check.</param>
        /// <returns>True if the GameObject has the component; false otherwise.</returns>
        public static bool HasComponent<T>(this GameObject gameObject) where T : Component
        {
            if (!gameObject.TryGetComponent<T>(out T component))
            {
                component = gameObject.GetComponentInChildren<T>();
                if (component == null) component = gameObject.GetComponentInParent<T>();
            }

            return component != null;
        }

        #endregion

        #region Animation Utilities

        /// <summary>
        /// Extracts sprites from an animation clip.
        /// </summary>
        /// <param name="clip">The animation clip to extract from.</param>
        /// <returns>A list of sprites contained in the animation clip.</returns>
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

        #endregion
    }
}