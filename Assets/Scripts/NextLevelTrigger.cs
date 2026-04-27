using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class NextLevelTrigger : MonoBehaviour
{
    private Button _nextLevelButton;

    private void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null) return;

        var root = uiDocument.rootVisualElement;
        
        // Ищем кнопку по имени (ID), которое вы зададите в UI Builder
        _nextLevelButton = root.Q<Button>("NextLevelButton");

        if (_nextLevelButton != null)
        {
            // Подписываемся на событие клика
            _nextLevelButton.clicked += LoadNextLevel;
        }
    }

    private void OnDisable()
    {
        // Обязательно отписываемся от события, чтобы избежать утечек памяти
        if (_nextLevelButton != null)
        {
            _nextLevelButton.clicked -= LoadNextLevel;
        }
    }

    // Метод можно сделать публичным, если захотите вызывать его не только из UI Toolkit
    public void LoadNextLevel()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        // Защита от ошибки (Blind spot: попытка загрузить уровень, которого нет)
        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneHandler.LoadLevel(nextIndex);
        }
        else
        {
            Debug.LogWarning("Достигнут конец игры: следующей сцены в Build Settings нет.");
            // Здесь можно загружать главное меню:
            // WastelandSceneManager.LoadLevel(0);
        }
    }
}