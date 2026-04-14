using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BetZone : MonoBehaviour
{
    public List<Coin> placedCoins = new List<Coin>();
    public Vector2 spacing = new Vector2(0.5f, 0);

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
            Vector3 targetPos = startPos + new Vector3(spacing.x * i, spacing.y * i, 0);
            placedCoins[i].MoveTo(targetPos, 0.2f);
        }
    }
}