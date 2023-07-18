using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguetyCraft.Generic.Utility
{
    public static class Utilities
    {
        public static bool GetComponent<T>(GameObject gameObject, out T comp) where T : Component
        {
            if (!gameObject.TryGetComponent<T>(out T component))
            {
                component = gameObject.GetComponentInChildren<T>();
            }
            comp = component;

            return component != null;
        }

        public static bool GetComponents<T>(GameObject gameObject, out List<T> comp) where T : Component
        {
            List<T> list = new();

            list.AddRange(gameObject.GetComponents<T>());
            list.AddRange(gameObject.GetComponentsInChildren<T>());
            comp = list;

            return list != null;
        }

        public static bool HasComponent<T>(GameObject gameObject) where T : Component
        {
            if (!gameObject.TryGetComponent<T>(out T component))
            {
                component = gameObject.GetComponentInChildren<T>();
            }

            return component != null;
        }
    }
}