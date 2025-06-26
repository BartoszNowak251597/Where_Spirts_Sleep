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

            var cameraFollow = Camera.main.GetComponent<FollowCharacter>();
            if (cameraFollow != null)
            {
                cameraFollow.target = player.transform;
                cameraFollow.ClearOverride();
                Debug.Log("Kamera przypisana do gracza.");
            }

            var movement = player.GetComponent<PlayerMovement>();
            if (movement == null)
            {
                Debug.LogError("Brak komponentu PlayerMovement!");
                return;
            }

            var checkpoint = GameObject.FindObjectsOfType<Checkpoint>()
                .FirstOrDefault(c => c.checkpointID == save.checkpointID);

            if (checkpoint != null)
            {
                Debug.Log("Przenoszę gracza do checkpointu: " + checkpoint.checkpointID);
                player.transform.position = checkpoint.transform.position;

                GameManager.Instance.MaxJumps = checkpoint.jumpsUnlocked;
                Debug.Log($"[CHECKPOINT MANAGER] Od checkpointu: maxJumps={checkpoint.jumpsUnlocked}");
            }
            else
            {
                Debug.Log("Checkpoint nie znaleziony, używam zapamiętanej pozycji.");
                player.transform.position = new Vector3(save.checkpointPosX, save.checkpointPosY, player.transform.position.z);
                GameManager.Instance.MaxJumps = 1;
            }
        }
    }
}
