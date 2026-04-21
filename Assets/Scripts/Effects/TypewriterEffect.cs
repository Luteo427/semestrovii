using UnityEngine;
using TMPro;
using System.Collections;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] [Range(0.01f, 0.5f)] private float _typingSpeed = 0.025f;
    [SerializeField] [Range(0f, 2f)] private float _startDelay = 0;

    private TextMeshProUGUI _textComponent;
    private string _fullText;
    private Coroutine _typingCoroutine;

    private void Awake()
    {
        _textComponent = GetComponent<TextMeshProUGUI>();
        _fullText = _textComponent.text;
        _textComponent.text = string.Empty;
    }

    private void OnEnable()
    {
        _textComponent.text = string.Empty;
        
        if (_typingCoroutine != null)
        {
            StopCoroutine(_typingCoroutine);
        }
        
        _typingCoroutine = StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        yield return new WaitForSeconds(_startDelay);

        foreach (char c in _fullText)
        {
            _textComponent.text += c;
            yield return new WaitForSeconds(_typingSpeed);
        }
    }
}