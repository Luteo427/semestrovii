using System.Collections;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class Card : MonoBehaviour
{
    [SerializeField] private CardData _cardData;
    public BoxCollider2D collider;
    public bool IsOpened;
    private bool _isHovered;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        ApplySprite();
    }

    private void OnValidate()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.delayCall += DelayedApplySprite;
        #endif
    }

    private void DelayedApplySprite()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.delayCall -= DelayedApplySprite;
        if (this == null) return;
        ApplySprite();
        #endif
    }

    public void InitializeData(CardData data)
    {
        _cardData = data;
        ApplySprite();
    }

    public void ApplySprite()
    {
        if (_cardData == null) return;
        if (_spriteRenderer == null) _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_spriteRenderer != null)
        {
            _spriteRenderer.sprite = IsOpened ? _cardData.FrontSprite : _cardData.BackSprite;
        }
    }

    public void SetOpened(bool state)
    {
        IsOpened = state;
        ApplySprite();
    }

    public void MoveTo(Vector3 targetPosition, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(MoveRoutine(targetPosition, duration));
    }

    private IEnumerator MoveRoutine(Vector3 target, float duration)
    {
        Vector3 start = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(start, target, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = target;
    }

    private void OnMouseOver()
    {
        if (!_isHovered)
        {
            _isHovered = true;
            _spriteRenderer.sortingOrder += 100;
            transform.localScale *= 1.2f;
        }
    }

    private void OnMouseExit()
    {
        if (_isHovered)
        {
            _isHovered = false;
            transform.localScale /= 1.2f;
            _spriteRenderer.sortingOrder -= 100;
        }
    }
    /*
    private void OnMouseDown()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnCardClicked(this);
        }
    }*/
}
