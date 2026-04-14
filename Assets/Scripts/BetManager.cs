using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BetManager : MonoBehaviour
{
    public Coin coinPrefab;
    public Transform coinPoolAnchor;
    public BetZone betZone;
    public int initialCoins = 5;

    private List<Coin> availableCoins = new List<Coin>();
    private Camera mainCamera;

    private Coin draggedCoin;
    private Vector3 dragStartPosition;

    private InputAction pressAction;
    private InputAction positionAction;

    private void Awake()
    {
        mainCamera = Camera.main;

        pressAction = new InputAction(type: InputActionType.Button, binding: "<Pointer>/press");
        positionAction = new InputAction(type: InputActionType.Value, binding: "<Pointer>/position");
    }

    private void OnEnable()
    {
        pressAction.Enable();
        positionAction.Enable();
    }

    private void OnDisable()
    {
        pressAction.Disable();
        positionAction.Disable();
    }

    private void Start()
    {
        SpawnCoins();
    }

    private void SpawnCoins()
    {
        for (int i = 0; i < initialCoins; i++)
        {
            Vector3 spawnPos = coinPoolAnchor.position + new Vector3(i * 0.1f, i * 0.1f, 0);
            Coin newCoin = Instantiate(coinPrefab, spawnPos, Quaternion.identity);
            availableCoins.Add(newCoin);
        }
    }

    private void Update()
    {
        Vector2 pointerPosition = positionAction.ReadValue<Vector2>();
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(pointerPosition);
        worldPosition.z = 0;

        if (pressAction.WasPressedThisFrame())
        {
            Collider2D[] hits = Physics2D.OverlapPointAll(worldPosition);
            foreach (var hit in hits)
            {
                Coin hitCoin = hit.GetComponent<Coin>();
                if (hitCoin != null && availableCoins.Contains(hitCoin))
                {
                    draggedCoin = hitCoin;
                    dragStartPosition = hitCoin.transform.position;
                    break;
                }
            }
        }

        if (draggedCoin != null && pressAction.IsInProgress())
        {
            draggedCoin.transform.position = worldPosition;
        }

        if (draggedCoin != null && pressAction.WasReleasedThisFrame())
        {
            Collider2D[] dropHits = Physics2D.OverlapPointAll(draggedCoin.transform.position);
            bool droppedInZone = false;

            foreach (var dropHit in dropHits)
            {
                if (dropHit.GetComponent<BetZone>() == betZone)
                {
                    droppedInZone = true;
                    break;
                }
            }

            if (droppedInZone)
            {
                availableCoins.Remove(draggedCoin);
                betZone.AddCoin(draggedCoin);
                ExecuteBetAction(draggedCoin);
            }
            else
            {
                draggedCoin.MoveTo(dragStartPosition, 0.2f);
            }
            
            draggedCoin = null;
        }
    }

    private void ExecuteBetAction(Coin coin)
    {
        // sss
    }
}