using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : Manager<SceneController>
{
    [SerializeField] private SceneField _targetScene;

    public void LoadTargetScene()
    {
        SceneManager.LoadScene(_targetScene);
    }

    public void LoadNextScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}