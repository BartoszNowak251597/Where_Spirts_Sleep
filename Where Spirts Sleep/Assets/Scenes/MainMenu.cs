using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Chapter 1 tutorial area");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
