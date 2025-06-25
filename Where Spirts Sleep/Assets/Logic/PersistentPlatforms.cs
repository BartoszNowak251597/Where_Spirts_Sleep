using UnityEngine;

namespace Logic
{
    public class PersistentPlatform : MonoBehaviour
    {
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}