using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Logic
{
    [System.Serializable]
    public class SaveData
    {
        public string checkpointID;
        public int sceneIndex;
        public List<string> unlockedAbilities = new();
        public float checkpointPosX;
        public float checkpointPosY;
    }


    public static class SaveSystem
    {
        private const string SaveKey = "PlayerSave";

        public static void Save(string checkpointID, int sceneIndex, IEnumerable<PlayerAbility> unlockedAbilities, Vector2 checkpointPosition)
        {
            SaveData data = new SaveData
            {
                checkpointID = checkpointID,
                sceneIndex = sceneIndex,
                checkpointPosX = checkpointPosition.x,
                checkpointPosY = checkpointPosition.y,
                unlockedAbilities = unlockedAbilities.Select(a => a.ToString()).ToList()
            };

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