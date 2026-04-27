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
        StartCoroutine(LoadAsync(SceneHandler.TargetSceneIndex));
    }

    private void Update()
    {
        if (_loadingIcon != null && !_isLoaded)
        {
            _currentAngle += rotationSpeed * Time.deltaTime;
            
            float steppedAngle = Mathf.Floor(_currentAngle / 45f) * 45f;
            
            _loadingIcon.style.rotate = new Rotate(new Angle(steppedAngle));
        }
    }

    private IEnumerator LoadAsync(int index)
    {
        if (_loadingLabel != null) _loadingLabel.text = loadingText;
        yield return null;

        AsyncOperation operation = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
        
        operation.allowSceneActivation = true;

        while (!operation.isDone)
        {
            yield return null;
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(index));

        _isLoaded = true;
        
        if (_loadingLabel != null)
        {
            _loadingLabel.text = readyText;
            _loadingLabel.AddToClassList("text-ready");
        }

        while (true)
        {
            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                break;
            }
            yield return null;
        }

        SceneManager.UnloadSceneAsync("LoadingScene"); 
    }
}