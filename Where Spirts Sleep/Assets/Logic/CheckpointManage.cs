using UnityEngine;
using UnityEngine.SceneManagement;

namespace Logic
{
    public class CheckpointManager : MonoBehaviour
    {
        private void Start()
        {
            var save = SaveSystem.Load();
            if (save == null) return;

            if (save.sceneName != SceneManager.GetActiveScene().name) return;

            GameObject[] checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");

            foreach (GameObject checkpointObj in checkpoints)
            {
                Checkpoint checkpoint = checkpointObj.GetComponent<Checkpoint>();
                if (checkpoint != null && checkpoint.checkpointID == save.checkpointID)
                {
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    if (player != null)
                    {
                        player.transform.position = checkpointObj.transform.position;

                        PlayerMovement movement = player.GetComponent<PlayerMovement>();
                        if (movement != null)
                        {
                            foreach (string abilityName in save.unlockedAbilities)
                            {
                                if (System.Enum.TryParse(abilityName, out PlayerAbility ability))
                                {
                                    movement.UnlockAbility(ability);
                                }
                            }
                        }

                        Debug.Log("Gracz odrodzony przy checkpoint: " + save.checkpointID);
                    }
                    break;
                }
            }
        }
    }
}