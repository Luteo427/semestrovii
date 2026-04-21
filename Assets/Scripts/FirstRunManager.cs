using UnityEngine;

public class FirstRunManager : MonoBehaviour
{
    [SerializeField] private GameObject _cutsceneGroup;
    [SerializeField] private GameObject _mainMenuGroup;

    private const string FirstRunKey = "IsFirstRun";

    private void Awake()
    {
        CheckFirstRun();
    }

    private void CheckFirstRun()
    {
        // PlayerPrefs.GetInt возвращает 0, если ключа не существует
        if (PlayerPrefs.GetInt(FirstRunKey, 0) == 0)
        {
            SetFirstRunExecuted();
            ActivateCutscene();
        }
        else
        {
            ActivateMainMenu();
        }
    }

    private void SetFirstRunExecuted()
    {
        PlayerPrefs.SetInt(FirstRunKey, 1);
        PlayerPrefs.Save();
    }

    private void ActivateCutscene()
    {
        _cutsceneGroup.SetActive(true);
        _mainMenuGroup.SetActive(false);
    }

    private void ActivateMainMenu()
    {
        _cutsceneGroup.SetActive(false);
        _mainMenuGroup.SetActive(true);
    }

    [ContextMenu("Reset First Run (Debug)")]
    private void ResetFirstRun()
    {
        PlayerPrefs.DeleteKey(FirstRunKey);
        Debug.Log("First run status reset.");
    }
}