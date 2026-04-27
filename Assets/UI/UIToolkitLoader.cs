using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;

public class UIToolkitLoader : MonoBehaviour
{
    public int sceneToLoadIndex = 1;
    public float rotationSpeed = 200f;
    
    [Header("Text Settings")]
    public string loadingText = "Загрузка игровых ресурсов...";
    public string readyText = "Загрузка завершена! Нажмите Пробел";

    private Label _loadingLabel;
    private VisualElement _loadingIcon;
    private float _currentAngle = 0f;
    private bool _isLoaded = false;

    private void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        _loadingLabel = root.Q<Label>("LoadingText");
        _loadingIcon = root.Q<VisualElement>("LoadingIcon");
        
        _isLoaded = false;
    }

    private void Start()
    {
        // Читаем цель из статического менеджера
        StartCoroutine(LoadAsync(SceneHandler.TargetSceneIndex));
    }

    private void Update()
    {
        if (_loadingIcon != null && !_isLoaded)
        {
            // Увеличиваем базовый угол
            _currentAngle += rotationSpeed * Time.deltaTime;
            
            // Заставляем вращение дергаться шагами по 45 градусов
            float steppedAngle = Mathf.Floor(_currentAngle / 45f) * 45f;
            
            _loadingIcon.style.rotate = new Rotate(new Angle(steppedAngle));
        }
    }

    private IEnumerator LoadAsync(int index)
    {
        if (_loadingLabel != null) _loadingLabel.text = loadingText;
        yield return null;

        // Важно: Загружаем сцену АДДИТИВНО (поверх текущей)
        AsyncOperation operation = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
        
        // Разрешаем активацию. Сцена будет создана прямо сейчас.
        operation.allowSceneActivation = true;

        // Ждем, пока сцена полностью не загрузится и не инициализируется
        while (!operation.isDone)
        {
            yield return null;
        }

        // На этом этапе все объекты новой сцены уже созданы и отработали Start().
        // Делаем новую сцену активной по умолчанию (чтобы свет и настройки окружения брались из нее)
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(index));

        _isLoaded = true;
        
        if (_loadingLabel != null)
        {
            _loadingLabel.text = readyText;
            _loadingLabel.AddToClassList("text-ready");
        }

        // Ждем ввода от пользователя
        while (true)
        {
            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                break; // Выходим из цикла ожидания
            }
            yield return null;
        }

        // Пользователь нажал пробел. Самоуничтожаем сцену загрузки.
        // Укажите точное имя вашей сцены загрузки.
        SceneManager.UnloadSceneAsync("LoadingScene"); 
    }
}