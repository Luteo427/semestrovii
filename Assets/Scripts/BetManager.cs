using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BetManager : MonoBehaviour
{
    public Coin coinPrefab;
    public BetZone coinPoolAnchor;
    public BetZone betZone;
    public int initialCoins = 5;

    public List<Coin> activeCoins = new List<Coin>();

    private void Start()
    {
        SpawnCoins();
    }

    private void Update()
    {
        if (Pointer.current != null)
        {
            Vector2 pointerPosition = Pointer.current.position.ReadValue();
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(pointerPosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
            if (hit.collider != null)
            {
                if (Pointer.current.press.wasPressedThisFrame)
                {
                    if (hit.collider.GetComponent<Coin>() != null)
                    {
                        Coin clickedCoin = hit.collider.GetComponent<Coin>();
                        OnCoinClicked(clickedCoin);
                    }
                    if (hit.collider.GetComponent<BetZone>() != null)
                    {
                        BetZone clickedZone = hit.collider.GetComponent<BetZone>();
                        OnBetZoneClicked(clickedZone);
                    }
                }
            }
        }
    }

    private void OnBetZoneClicked(BetZone clickedZone)
    {
        for (int index = 0; index < activeCoins.Count; index++)
        {
            if (betZone.placedCoins.Contains(activeCoins[index]))
                betZone.placedCoins.Remove(activeCoins[index]);
            if (coinPoolAnchor.placedCoins.Contains(activeCoins[index]))
                coinPoolAnchor.placedCoins.Remove(activeCoins[index]);
            activeCoins[index].transform.localScale /= 1.2f;
            activeCoins[index].transform.position += Vector3.down;
            clickedZone.AddCoin(activeCoins[index]);
        }
        activeCoins.Clear();
    }

    public void OnCoinClicked(Coin coin)
    {
        if (activeCoins.Contains(coin))
        {
            activeCoins.Remove(coin);
            coin.transform.localScale /= 1.2f;
            coin.transform.position += Vector3.down;
        }
        else
        {
            activeCoins.Add(coin);
            coin.transform.localScale *= 1.2f;
            coin.transform.position += Vector3.up;
        }
    }

    private void SpawnCoins()
    {
        for (int i = 0; i < initialCoins; i++)
        {
            Vector3 spawnPos = coinPoolAnchor.transform.position + new Vector3(i/5, i % 5 * 0.1f, 0);
            Coin newCoin = Instantiate(coinPrefab, spawnPos, Quaternion.identity);
            coinPoolAnchor.AddCoin(newCoin);
        }
    }
}
