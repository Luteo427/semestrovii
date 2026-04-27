using UnityEngine.SceneManagement;

public static class SceneHandler
{
    // Хранит индекс сцены, которую мы хотим загрузить
    public static int TargetSceneIndex { get; private set; }

    // Метод для вызова загрузки из любой точки игры
    public static void LoadLevel(int buildIndex)
    {
        TargetSceneIndex = buildIndex;
        // Загружаем саму сцену загрузки (укажите её точное имя из Build Settings)
        SceneManager.LoadScene("LoadingScene"); 
    }
}