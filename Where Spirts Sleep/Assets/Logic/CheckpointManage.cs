using UnityEngine;
using UnityEngine.SceneManagement;

namespace Logic
{
    public class CheckpointManager : MonoBehaviour
    {
        private void Start()
        {
            var save = SaveSystem.Load();
            if (save == null)
            {
                Debug.LogWarning("Brak zapisu");
                return;
            }

            Debug.Log("Zapis: checkpoint=" + save.checkpointID + ", scena=" + save.sceneIndex);

            if (save.sceneIndex != SceneManager.GetActiveScene().buildIndex)
            {
                Debug.LogWarning("Zapis nie pasuje do aktualnej sceny!");
                return;
            }


            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                Debug.LogError("Gracz nie znaleziony!");
                return;
            }
            else
            {
                Debug.Log("Gracz znaleziony.");
            }

            var movement = player.GetComponent<PlayerMovement>();
            if (movement == null)
            {
                Debug.LogError("Brak PlayerMovement!");
                return;
            }

            foreach (string ability in save.unlockedAbilities)
            {
                Debug.Log("Odtwarzanie umiejętności: " + ability);
                if (System.Enum.TryParse(ability, out PlayerAbility parsed))
                {
                    movement.UnlockAbility(parsed);
                }
            }
        }
    }
}