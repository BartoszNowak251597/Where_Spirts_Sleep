using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(LoadNextSceneAsync());
    }

    private System.Collections.IEnumerator LoadNextSceneAsync()
    {
        yield return new WaitForSeconds(1f); // symulacja czasu ładowania (opcjonalnie)

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(LoadingData.NextScene);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}