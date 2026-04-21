using System.Collections.Generic;
using UnityEngine;

public class BetZone : MonoBehaviour
{
    public List<Coin> placedCoins = new List<Coin>();
    public Vector2 spacing = new Vector2(0, 0.5f);

    public void AddCoin(Coin coin)
    {
        if (!placedCoins.Contains(coin))
        {
            placedCoins.Add(coin);
            UpdateLayout();
        }
    }

    private void UpdateLayout()
    {
        Vector3 startPos = transform.position;

        for (int i = 0; i < placedCoins.Count; i++)
        {
            Vector3 targetPos = startPos + new Vector3(i / 5 * spacing.x, i % 5 * spacing.y, -1);
            placedCoins[i].MoveTo(targetPos, 0.2f);
        }
    }
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Coin>() != null)
        {
            placedCoins.Add(collision.GetComponent<Coin>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Coin>() != null)
        {
            placedCoins.Remove(collision.GetComponent<Coin>());
        }
    }*/
}
