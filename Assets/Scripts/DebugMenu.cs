using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // Подключение новой системы ввода

public class DebugMenu : MonoBehaviour
{
    [Tooltip("Действие для вызова меню. По умолчанию: Тильда (~)")]
    // Создаем локальное действие с привязкой по умолчанию
    public InputAction toggleMenuAction = new InputAction("ToggleDebug", binding: "<Keyboard>/backquote");

    private bool showMenu = false;
    private bool godMode = false;

    // В новой системе ввода действия нужно явно включать и выключать
    private void OnEnable()
    {
        toggleMenuAction.Enable();
    }

    private void OnDisable()
    {
        toggleMenuAction.Disable();
    }

    void Update()
    {
        // Свойство triggered возвращает true только в кадре нажатия (аналог GetKeyDown)
        if (toggleMenuAction.triggered)
        {
            showMenu = !showMenu;
        }
    }

    void OnGUI()
    {
        if (!showMenu) return;

        Rect windowRect = new Rect(20, 20, 250, 300);
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

        bool previousGodMode = godMode;
        godMode = GUILayout.Toggle(godMode, " Включить God Mode");
        
        if (godMode != previousGodMode)
        {
            Debug.Log($"[Debug] Режим бога: {godMode}");
        }

        GUILayout.Space(20);

        if (GUILayout.Button("Закрыть меню"))
        {
            showMenu = false;
        }

        GUILayout.EndArea();
    }

    private void ReloadCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}