using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadingManager : MonoBehaviour
{
    public void LoadMainMenuScene()
    {
        StartCoroutine(LoadMainMenuDelayed());
    }

    IEnumerator LoadMainMenuDelayed()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadSceneAsync(GameConstants.GameScene.SceneMainMenu);
    }

    public void RestartTheGame()
    {
        StartCoroutine(RestartTheGameDelayed());
    }

    IEnumerator RestartTheGameDelayed()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(GameConstants.GameScene.SceneAdScene);
    }
}
