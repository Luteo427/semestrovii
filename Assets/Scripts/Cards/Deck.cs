using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public Card cardPrefab;
    public List<CardData> startingCards;
    
    private List<CardData> drawPile = new List<CardData>();

    public void InitializeDeck()
    {
        drawPile.Clear();
        drawPile.AddRange(startingCards);
        Shuffle();
    }

    public void Shuffle()
    {
        for (int i = 0; i < drawPile.Count; i++)
        {
            int randomIndex = Random.Range(i, drawPile.Count);
            CardData temp = drawPile[i];
            drawPile[i] = drawPile[randomIndex];
            drawPile[randomIndex] = temp;
        }
    }

    public Card DrawCard(Vector3 spawnPosition)
    {
        if (drawPile.Count == 0) return null;

        CardData drawnData = drawPile[0];
        drawPile.RemoveAt(0);

        Card newCard = Instantiate(cardPrefab, spawnPosition, Quaternion.identity);
        newCard.InitializeData(drawnData);
        
        return newCard;
    }
}