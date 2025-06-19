using System.Linq;
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

            Debug.Log("Gracz znaleziony.");

            // Kamera śledzi gracza
            var cameraFollow = Camera.main.GetComponent<FollowCharacter>();
            if (cameraFollow != null)
            {
                cameraFollow.target = player.transform;
                cameraFollow.ClearOverride();
                Debug.Log("Kamera przypisana do gracza.");
            }

            // 🔧 Tu był brakujący fragment:
            var movement = player.GetComponent<PlayerMovement>();
            if (movement == null)
            {
                Debug.LogError("Brak komponentu PlayerMovement!");
                return;
            }

            // Odtwarzanie umiejętności
            foreach (string ability in save.unlockedAbilities)
            {
                Debug.Log("Odtwarzanie umiejętności: " + ability);
                if (System.Enum.TryParse(ability, out PlayerAbility parsed))
                {
                    movement.UnlockAbility(parsed);
                }
            }

            // Przenoszenie gracza
            var checkpoint = GameObject.FindObjectsOfType<Checkpoint>()
                .FirstOrDefault(c => c.checkpointID == save.checkpointID);

            if (checkpoint != null)
            {
                Debug.Log("Przenoszę gracza do checkpointu: " + checkpoint.checkpointID);
                player.transform.position = checkpoint.transform.position;
            }
            else
            {
                Debug.Log("Checkpoint nie znaleziony, używam zapamiętanej pozycji.");
                player.transform.position = new Vector3(save.checkpointPosX, save.checkpointPosY, player.transform.position.z);
            }
        }
    }
}
