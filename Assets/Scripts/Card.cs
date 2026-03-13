using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] private CardData _cardData;
    

    private Image _artworkRenderer;

    private void Start()
    {
        if (_cardData != null)
        {
            Initialize(_cardData);
        }
    }

    public void Initialize(CardData newData)
    {
        _cardData = newData;
        _artworkRenderer.sprite = _cardData.Artwork;
    }
}