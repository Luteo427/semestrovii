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
        
        _nextLevelButton = root.Q<Button>("NextLevelButton");

        if (_nextLevelButton != null)
        {
            _nextLevelButton.clicked += LoadNextLevel;
        }
    }

    private void OnDisable()
    {
        if (_nextLevelButton != null)
        {
            _nextLevelButton.clicked -= LoadNextLevel;
        }
    }

    public void LoadNextLevel()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneHandler.LoadLevel(nextIndex);
        }
        else
        {
            Debug.LogWarning("Достигнут конец игры: следующей сцены в Build Settings нет.");
        }
    }
}