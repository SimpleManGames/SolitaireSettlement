using UnityEngine;

namespace Simplicity.Singleton
{
    /// <summary>
    /// Singleton Helper class
    /// Inherit from Singleton and use the class as T
    /// </summary>
    /// <typeparam name="T">
    /// Class name that will be the singleton.
    /// T will be a MonoBehaviour.
    /// </typeparam>
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        /// <summary>
        /// Holds reference to itself inorder for it to accessed as a static
        /// </summary>
        private static T _instance;

        public static T Instance => _instance as T;

        [field: SerializeField, Tooltip("If true, then this object will be destroyed between loading scenes")]
        private bool DestroyOnLoad { get; set; }

        public virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                if (!DestroyOnLoad)
                    DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject.GetComponent<T>());
            }
        }

        public override string ToString()
        {
            return typeof(T).ToString();
        }
    }
}