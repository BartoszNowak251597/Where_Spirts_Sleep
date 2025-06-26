using UnityEngine;
using UnityEngine.SceneManagement;

namespace Logic
{
    public class Checkpoint : MonoBehaviour
    {
        public string checkpointID;

        public Sprite inactiveSprite;
        public Sprite activeSprite;

        public ParticleSystem particle;

        public int jumpsUnlocked = 1;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            var player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                SaveSystem.Save(
                    checkpointID,
                    SceneManager.GetActiveScene().buildIndex,
                    player.GetUnlockedAbilities(),
                    transform.position
                );

                GameManager.Instance.MaxJumps = jumpsUnlocked;

                particle.Play();

                Debug.Log("Checkpoint zapisany: " + checkpointID);
            }
        }
    }
}