using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class Coin : MonoBehaviour
{
    [SerializeField] private Collider2D _collider;
    private bool _isHovered;

    [SerializeField] private SpriteRenderer _spriteRenderer;

    public void MoveTo(Vector3 targetPosition, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(MoveRoutine(targetPosition, duration));
    }

    private IEnumerator MoveRoutine(Vector3 target, float duration)
    {
        _collider.enabled = false;
        Vector3 start = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(start, target, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        _collider.enabled = true;
        transform.position = target;
    }

    private void OnMouseOver()
    {
        if (!_isHovered)
        {
            if (!_isHovered)
            {
                _isHovered = true;
                _spriteRenderer.sortingOrder += 100;
                transform.localScale *= 1.2f;
            }
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
}
