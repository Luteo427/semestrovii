using UnityEngine;
using TMPro;
using System.Collections;

public class TextBlinkEffect : MonoBehaviour
{
    [SerializeField] [Range(0.1f, 10f)] private float _blinkSpeed = 2.0f;
    [SerializeField] [Range(0f, 1f)] private float _minAlpha = 0.0f;
    [SerializeField] [Range(0f, 1f)] private float _maxAlpha = 1.0f;

    private TextMeshProUGUI _textComponent;

    private void Awake()
    {
        _textComponent = GetComponent<TextMeshProUGUI>();
        SetAlpha(_minAlpha);
    }

    private void OnEnable()
    {
        StartCoroutine(PulseRoutine());
    }

    private IEnumerator PulseRoutine()
    {
        float progress = 0f;

        while (true)
        {
            progress += Time.deltaTime * _blinkSpeed;
            // Mathf.PingPong возвращает значение, которое плавно ходит между 0 и 1
            float lerpValue = Mathf.PingPong(progress, 1f);
            float currentAlpha = Mathf.Lerp(_minAlpha, _maxAlpha, lerpValue);

            SetAlpha(currentAlpha);
            yield return null;
        }
    }

    private void SetAlpha(float alpha)
    {
        if (_textComponent == null) return;
        Color c = _textComponent.color;
        c.a = alpha;
        _textComponent.color = c;
    }
}