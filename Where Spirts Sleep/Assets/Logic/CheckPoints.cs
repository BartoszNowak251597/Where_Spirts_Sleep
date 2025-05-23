using UnityEngine;
using UnityEngine.SceneManagement;

namespace Logic
{
    public class Checkpoint : MonoBehaviour
    {
        public string checkpointID;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            var player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                SaveSystem.Save(
                    checkpointID,
                    SceneManager.GetActiveScene().buildIndex,
                    player.GetUnlockedAbilities()
                );

                Debug.Log("Checkpoint zapisany: " + checkpointID);
            }
        }
    }
}