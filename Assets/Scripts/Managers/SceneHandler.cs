using UnityEngine.SceneManagement;

public static class SceneHandler
{
    public static int TargetSceneIndex { get; private set; }

    public static void LoadLevel(int buildIndex)
    {
        TargetSceneIndex = buildIndex;
        SceneManager.LoadScene("LoadingScene"); 
    }
}