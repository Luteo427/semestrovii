using System.Collections.Generic;
using UnityEngine;

public class CardAnchor : MonoBehaviour
{
    public List<Card> cards = new List<Card>();
    public Vector2 offset;

    private void OnValidate()
    {
        UpdateLayout();
    }

    public void UpdateLayout()
    {
        if (cards == null || cards.Count == 0) return;
        float totalWidth = (cards.Count - 1) * offset.x;
        Vector3 startPos = transform.position - new Vector3(totalWidth / 2f, 0, 0);

        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i] != null)
            {
                //сортирует карты по слоям, чтобы они не лежали не рандомно
                cards[i].GetComponent<SpriteRenderer>().sortingOrder = i;
                Vector3 targetPos = startPos + new Vector3(offset.x * i, offset.y * i, 0);
                if (Application.isPlaying)
                {
                    cards[i].MoveTo(targetPos, 0.2f);
                }
                else
                {
                    cards[i].transform.position = targetPos;
                }
            }
        }
    }
}
