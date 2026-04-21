using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum GameState { Dealing, PlayerSelection, BotTurn, Resolution, TablePhase }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState currentState;
    public CardAnchor playerHand;
    public CardAnchor botHand;
    public CardAnchor playerDiscardPile;
    public CardAnchor botDiscardPile;
    public CardAnchor tableAnchor;
    public CardAnchor commonDiscardPile;
    public Deck deck;

    public int initialDealCount = 6;
    public int cardsToKeep = 2;
    [SerializeField] private SpriteRenderer[] objectsToBlur;
    private bool _zoomedIn;
    private IEnumerator _nextCoroutine;
    private List<Card> playerSelectedCards = new List<Card>();
    private List<Card> botSelectedCards = new List<Card>();

    private Camera mainCamera;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        mainCamera = Camera.main;
    }

    private void Start()
    {
        deck.InitializeDeck();
        StartCoroutine(DealCardsRoutine());
    }

    private void Update()
    {
        if (currentState != GameState.PlayerSelection) return;

        if (Pointer.current != null)
        {
            Vector2 pointerPosition = Pointer.current.position.ReadValue();
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(pointerPosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
            if (hit.collider != null)
            {
                if (Pointer.current.press.wasPressedThisFrame)
                {
                    if (hit.collider.GetComponent<Card>() != null)
                    {
                        Card clickedCard = hit.collider.GetComponent<Card>();
                        OnCardClicked(clickedCard);
                    }

                    if (hit.collider.name == "Bell" && _nextCoroutine != null)
                    {
                        StartCoroutine(_nextCoroutine);
                        _nextCoroutine = null;
                    }

                    if (hit.collider.name == "Board")
                    {
                        if (_zoomedIn)
                            StartCoroutine(Zoom(3, 5, mainCamera.transform.position, mainCamera.transform.position - new Vector3(0, -1.5f, 0), 0.01f, 0));
                        else
                            StartCoroutine(Zoom(5, 3, mainCamera.transform.position, mainCamera.transform.position + new Vector3(0,-1.5f, 0), 0, 0.01f));
                    }
                }
            }
        }
    }


    private IEnumerator Zoom(float cameraStart, float cameraStop, Vector3 startPosition, Vector3 stopPosition, float blurStart, float blurStop)
    {
        float elapsed = 0f;
        while (elapsed < 1)
        {
            foreach (SpriteRenderer sprite in objectsToBlur)
            {
                sprite.material.SetFloat("_BlurAmount", Mathf.Lerp(blurStart, blurStop, elapsed / 1));
            }
            mainCamera.orthographicSize = Mathf.Lerp(cameraStart, cameraStop, elapsed / 1);
            mainCamera.transform.position = Vector3.Lerp(startPosition, stopPosition, elapsed / 1);
            elapsed += Time.deltaTime;
            yield return null;
        }
        mainCamera.orthographicSize = cameraStop;
        mainCamera.transform.position = stopPosition;
        _zoomedIn = !_zoomedIn;
    }

    private IEnumerator DealCardsRoutine()
    {
        currentState = GameState.Dealing;
        for (int i = 0; i < initialDealCount; i++)
        {
            DealCardTo(playerHand, true);
            yield return new WaitForSeconds(0.15f);
            DealCardTo(botHand, false);
            yield return new WaitForSeconds(0.15f);
        }
        for (int i = 0; i < playerHand.cards.Count; i++)
        {
            playerHand.cards[i].collider.enabled = true;
        }
        currentState = GameState.PlayerSelection;
    }

    private void DealCardTo(CardAnchor targetHand, bool openFace)
    {
        Card card = deck.DrawCard(deck.transform.position);
        if (card == null) return;
        card.SetOpened(openFace);
        targetHand.cards.Add(card);
        targetHand.UpdateLayout();
    }

    public void OnCardClicked(Card card)
    {
        if (!playerHand.cards.Contains(card)) return;

        if (playerSelectedCards.Contains(card))
        {
            playerSelectedCards.Remove(card);
            card.transform.localScale = Vector3.one;
            card.transform.position += Vector3.down;
        }
        else if (playerSelectedCards.Count < cardsToKeep)
        {
            playerSelectedCards.Add(card);
            card.transform.localScale = Vector3.one * 1.2f;
            card.transform.position += Vector3.up;
        }

        if (playerSelectedCards.Count == cardsToKeep)
            _nextCoroutine = ProcessSelectionRoutine();
        else
            _nextCoroutine = null;
    }

    private IEnumerator ProcessSelectionRoutine()
    {
        currentState = GameState.BotTurn;
        yield return new WaitForSeconds(0.5f);

        for (int i = playerHand.cards.Count - 1; i >= 0; i--)
        {
            Card card = playerHand.cards[i];

            if (!playerSelectedCards.Contains(card))
            {
                playerHand.cards.RemoveAt(i);
                card.collider.enabled = false;
                card.SetOpened(false);
                playerDiscardPile.cards.Add(card);
            }
        }

        playerHand.UpdateLayout();
        playerDiscardPile.UpdateLayout();
        StartCoroutine(BotTurnRoutine());
    }

    private IEnumerator BotTurnRoutine()
    {
        yield return new WaitForSeconds(1f);

        botSelectedCards.Add(botHand.cards[0]);
        botSelectedCards.Add(botHand.cards[1]);

        for (int i = botHand.cards.Count - 1; i >= 0; i--)
        {
            Card card = botHand.cards[i];

            if (!botSelectedCards.Contains(card))
            {
                botHand.cards.RemoveAt(i);
                card.SetOpened(false);
                botDiscardPile.cards.Add(card);
            }
        }

        botHand.UpdateLayout();
        botDiscardPile.UpdateLayout();

        yield return new WaitForSeconds(1f);

        StartCoroutine(ResolutionAndTablePhaseRoutine());
    }

    private IEnumerator ResolutionAndTablePhaseRoutine()
    {
        currentState = GameState.Resolution;

        yield return new WaitForSeconds(2f);

        currentState = GameState.TablePhase;

        foreach (Card c in playerDiscardPile.cards) c.SetOpened(false);
        foreach (Card c in botDiscardPile.cards) c.SetOpened(false);

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < 6; i++)
        {
            Card card = deck.DrawCard(deck.transform.position);
            if (card != null)
            {
                card.SetOpened(false);
                tableAnchor.cards.Add(card);
            }
        }

        tableAnchor.UpdateLayout();
        yield return new WaitForSeconds(0.5f);

        List<Card> leftToRightCards = new List<Card>(tableAnchor.cards);
        leftToRightCards.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));

        for (int i = 0; i < leftToRightCards.Count; i++)
        {
            leftToRightCards[i].SetOpened(true);
            yield return new WaitForSeconds(0.15f);
        }

        yield return new WaitForSeconds(2f);

        foreach (Card c in tableAnchor.cards) c.SetOpened(false);
        yield return new WaitForSeconds(0.5f);

        Vector3 centerPos = tableAnchor.transform.position;
        foreach (Card c in tableAnchor.cards)
        {
            c.MoveTo(centerPos, 0.3f);
        }

        yield return new WaitForSeconds(0.4f);

        for (int i = 0; i < tableAnchor.cards.Count; i++)
        {
            int randomIndex = Random.Range(i, tableAnchor.cards.Count);
            Card temp = tableAnchor.cards[i];
            tableAnchor.cards[i] = tableAnchor.cards[randomIndex];
            tableAnchor.cards[randomIndex] = temp;
        }

        tableAnchor.UpdateLayout();

        yield return new WaitForSeconds(0.6f);

        for (int i = 0; i < 3; i++)
        {
            if (tableAnchor.cards.Count > 0)
            {
                Card card = tableAnchor.cards[0];
                tableAnchor.cards.RemoveAt(0);
                commonDiscardPile.cards.Add(card);
            }
        }

        tableAnchor.UpdateLayout();
        commonDiscardPile.UpdateLayout();

        yield return new WaitForSeconds(0.5f);

        if (tableAnchor.cards.Count > 0)
        {
            int randomCardIndex = Random.Range(0, tableAnchor.cards.Count);
            tableAnchor.cards[randomCardIndex].SetOpened(true);
        }
    }

}
