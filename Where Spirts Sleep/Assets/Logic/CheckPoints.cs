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
        
        private void Start()
        {
            if (particle == null)
                particle = GetComponentInChildren<ParticleSystem>();

            if (particle == null)
                Debug.LogError("[Checkpoint] ❌ ParticleSystem not found!");
            else
                Debug.Log("[Checkpoint] ✅ ParticleSystem found!");
        }

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

                if (particle != null)
                {
                    particle.Play();
                }

                Debug.Log("Checkpoint zapisany: " + checkpointID);
            }
        }
    }
}