using UnityEngine;
using UnityEngine.SceneManagement;

namespace Logic
{
    public class SceneSwitch : MonoBehaviour
    {
        public int sceneToSwitchTo;
        public int sceneIndex;
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
                        unlockedAbilities: player.GetUnlockedAbilities()
                    );
                }

                SceneManager.LoadScene(sceneToSwitchTo);
            }
        }
    
    }
}
