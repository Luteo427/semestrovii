using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // Подключение новой системы ввода

public class DebugMenu : MonoBehaviour
{
    [Tooltip("Действие для вызова меню. По умолчанию: Тильда (~)")]
    // Создаем локальное действие с привязкой по умолчанию
    public InputAction ToggleMenuAction = new("ToggleDebug", binding: "<Keyboard>/backquote");

    private bool _showMenu = false;
    private bool _godMode = false;

    // В новой системе ввода действия нужно явно включать и выключать
    private void OnEnable()
    {
        ToggleMenuAction.Enable();
    }

    private void OnDisable()
    {
        ToggleMenuAction.Disable();
    }

    private void Update()
    {
        // Свойство triggered возвращает true только в кадре нажатия (аналог GetKeyDown)
        if (ToggleMenuAction.triggered)
        {
            _showMenu = !_showMenu;
        }
    }

    private void OnGUI()
    {
        if (!_showMenu) return;

        Rect windowRect = new(20, 20, 250, 300);
        GUI.Box(windowRect, "Отладочное меню");

        GUILayout.BeginArea(new Rect(30, 50, 230, 260));

        if (GUILayout.Button("Перезагрузить сцену"))
        {
            ReloadCurrentScene();
        }

        if (GUILayout.Button("Восстановить здоровье"))
        {
            Debug.Log("[Debug] Здоровье игрока восстановлено.");
        }

        if (GUILayout.Button("Добавить 1000 золота"))
        {
            Debug.Log("[Debug] Добавлено 1000 золота.");
        }

        GUILayout.Space(10);

        bool previousGodMode = _godMode;
        _godMode = GUILayout.Toggle(_godMode, " Включить God Mode");
        
        if (_godMode != previousGodMode)
        {
            Debug.Log($"[Debug] Режим бога: {_godMode}");
        }

        GUILayout.Space(20);

        if (GUILayout.Button("Закрыть меню"))
        {
            _showMenu = false;
        }

        GUILayout.EndArea();
    }

    private void ReloadCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}