using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems; // Требуется для интерфейсов

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
public class SpriteButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    [Header("Настройки событий")]
    public UnityEvent onClick;

    [Header("Визуальный отклик")]
    public Color normalColor = Color.white;
    public Color hoverColor = new Color(0.8f, 0.8f, 0.8f);
    public Color pressedColor = new Color(0.5f, 0.5f, 0.5f);

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.color = normalColor;
    }

    // Аналог OnMouseEnter
    public void OnPointerEnter(PointerEventData eventData)
    {
        _spriteRenderer.color = hoverColor;
    }

    // Аналог OnMouseExit
    public void OnPointerExit(PointerEventData eventData)
    {
        _spriteRenderer.color = normalColor;
    }

    // Срабатывает в момент зажатия кнопки мыши
    public void OnPointerDown(PointerEventData eventData)
    {
        _spriteRenderer.color = pressedColor;
    }

    // Срабатывает при отпускании кнопки мыши
    public void OnPointerUp(PointerEventData eventData)
    {
        _spriteRenderer.color = normalColor;
    }

    // Аналог OnMouseUpAsButton. Гарантирует, что клик был начат и закончен на этом объекте.
    public void OnPointerClick(PointerEventData eventData)
    {
        _spriteRenderer.color = hoverColor;
        onClick?.Invoke();
    }
}