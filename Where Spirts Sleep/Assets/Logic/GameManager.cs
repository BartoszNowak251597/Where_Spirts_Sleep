using System.Collections.Generic;
using UnityEngine;

namespace Logic
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public int MaxJumps { get; set; } = 1;
        public HashSet<PlayerAbility> UnlockedAbilities { get; private set; } = new();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void SaveAbility(PlayerAbility ability)
        {
            UnlockedAbilities.Add(ability);
        }

        public void ClearAbilities()
        {
            UnlockedAbilities.Clear();
        }

        public bool HasAbility(PlayerAbility ability)
        {
            return UnlockedAbilities.Contains(ability);
        }
    }
}