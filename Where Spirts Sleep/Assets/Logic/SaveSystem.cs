using System.Collections.Generic;
using UnityEngine;

namespace Logic
{
    [System.Serializable]
    public class SaveData
    {
        public string checkpointID;
        public string sceneName;
        public List<string> unlockedAbilities = new();
    }

    public static class SaveSystem
    {
        private const string SaveKey = "PlayerSave";

        public static void Save(string checkpointID, string sceneName, IEnumerable<PlayerAbility> unlockedAbilities)
        {
            SaveData data = new SaveData
            {
                checkpointID = checkpointID,
                sceneName = sceneName,
                unlockedAbilities = new List<string>()
            };

            foreach (var ability in unlockedAbilities)
            {
                data.unlockedAbilities.Add(ability.ToString());
            }

            string json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(SaveKey, json);
            PlayerPrefs.Save();
        }

        public static SaveData Load()
        {
            if (!PlayerPrefs.HasKey(SaveKey)) return null;
            string json = PlayerPrefs.GetString(SaveKey);
            return JsonUtility.FromJson<SaveData>(json);
        }

        public static void Clear()
        {
            PlayerPrefs.DeleteKey(SaveKey);
        }
    }
}