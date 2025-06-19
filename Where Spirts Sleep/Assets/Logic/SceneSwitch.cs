using UnityEngine;
using UnityEngine.SceneManagement;

namespace Logic
{
    public class SceneSwitch : MonoBehaviour
    {
        public int sceneToSwitchTo;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                var player = other.GetComponent<PlayerMovement>();
                if (player != null)
                {
                    SaveSystem.Save(
                        checkpointID: "AUTO",
                        sceneIndex: sceneToSwitchTo,
                        unlockedAbilities: player.GetUnlockedAbilities(),
                        transform.position
                    );
                }
                
                SceneManager.LoadScene(sceneToSwitchTo); // przekazujemy nazwę docelowej sceny
            }
        }
    }
}