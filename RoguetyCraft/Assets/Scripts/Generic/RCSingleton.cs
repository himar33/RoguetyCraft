using UnityEngine;

namespace RoguetyCraft.DesignPatterns.Singleton
{
    /// <summary>
    /// Singleton pattern implementation for MonoBehaviour classes.
    /// </summary>
    /// <typeparam name="T">Type of the singleton.</typeparam>
    public class RCSingleton<T> : MonoBehaviour where T : Component
    {
        #region Singleton Logic

        /// <summary>
        /// Gets the singleton instance of type T.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = (T)FindObjectOfType(typeof(T));
                    if (instance == null)
                    {
                        SetupInstance();
                    }
                }
                return instance;
            }
        }
        private static T instance;

        #endregion

        #region Unity Callbacks

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        public virtual void Awake()
        {
            RemoveDuplicates();
        }

        #endregion

        #region Helper Methods

        private static void SetupInstance()
        {
            instance = (T)FindObjectOfType(typeof(T));

            if (instance == null)
            {
                GameObject go = new GameObject();
                go.name = typeof(T).Name;
                instance = go.AddComponent<T>();
                DontDestroyOnLoad(go);
            }
        }

        private void RemoveDuplicates()
        {
            if (instance == null)
            {
                instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        #endregion
    }
}
