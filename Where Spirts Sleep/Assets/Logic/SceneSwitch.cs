using UnityEngine;
using UnityEngine.SceneManagement;

namespace Logic
{
    public class SceneSwitch : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                SceneManager.LoadScene(2);
            }
        }
    
    }
}
