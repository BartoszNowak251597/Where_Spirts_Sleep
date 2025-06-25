using UnityEngine;
using System.Collections.Generic;

namespace Logic
{
    public class CollectibleManager : MonoBehaviour
    {
        public static CollectibleManager Instance;

        private readonly List<CollectibleID> _collectOrder = new List<CollectibleID>
        {
            CollectibleID.Item1,
            CollectibleID.Item2,
            CollectibleID.Item3
        };

        private int _currentIndex = 0;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        public bool CanCollect(CollectibleID id)
        {
            return _currentIndex < _collectOrder.Count && _collectOrder[_currentIndex] == id;
        }

        public bool AllKeysCollected(int totalRequired)
        {
            return _currentIndex >= totalRequired;
        }

        public void Collect(CollectibleID id)
        {
            if (CanCollect(id))
            {
                Debug.Log($"Zebrano: {id}");
                _currentIndex++;
            }
        }
    }
}