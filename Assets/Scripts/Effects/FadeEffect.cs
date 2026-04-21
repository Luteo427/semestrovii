using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeEffect : MonoBehaviour
{
    [SerializeField] [Range(0.1f, 10f)] private float _fadeDuration = 2.0f;
    [SerializeField] [Range(0f, 5f)] private float _startDelay = 0.5f;

    private Image _fadeImage;

    private void Awake()
    {
        _fadeImage = GetComponent<Image>();
        if (_fadeImage != null)
        {
            Color c = _fadeImage.color;
            c.a = 1f;
            _fadeImage.color = c;
        }
    }

    private void Start()
    {
        StartCoroutine(DoFadeOut());
    }

    private IEnumerator DoFadeOut()
    {
        yield return new WaitForSeconds(_startDelay);

        float timer = 0f;
        Color startColor = _fadeImage.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        while (timer < _fadeDuration)
        {
            timer += Time.deltaTime;
            _fadeImage.color = Color.Lerp(startColor, endColor, timer / _fadeDuration);
            yield return null;
        }

        _fadeImage.color = endColor;
        _fadeImage.gameObject.SetActive(false);
    }
}