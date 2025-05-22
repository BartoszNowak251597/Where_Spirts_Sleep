using UnityEngine;
using UnityEngine.SceneManagement;

namespace Logic
{
    public class MainMenu : MonoBehaviour
    {
        public void PlayGame()
        {
            SaveSystem.Clear();
            SceneManager.LoadScene(1);
        }
        
        public void ContinueGame()
        {
            var save = SaveSystem.Load();
            if (save != null)
            {
                SceneManager.LoadScene(save.sceneName);
            }
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
