using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum GameState
{
    Dealing,
    PlayerSelection,
    BotTurn,
    Resolution,
    TablePhase
}

public class GameManager : Manager<GameManager>
{
    public GameState CurrentState;
    public CardAnchor PlayerHand;
    public CardAnchor BotHand;
    public CardAnchor PlayerDiscardPile;
    public CardAnchor BotDiscardPile;
    public CardAnchor TableAnchor;
    public CardAnchor CommonDiscardPile;
    public Deck Deck;

    public int InitialDealCount = 6;
    public int CardsToKeep = 2;
    private List<Card> _playerSelectedCards = new();
    private List<Card> _botSelectedCards = new();

    private Camera _mainCamera;

    protected override void OnInit()
    {
        _mainCamera = Camera.main;
    }

    private void Start()
    {
        Deck.InitializeDeck();
        StartCoroutine(DealCardsRoutine());
    }

    private void Update()
    {
        if (CurrentState != GameState.PlayerSelection) return;

        if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
        {
            Vector2 pointerPosition = Pointer.current.position.ReadValue();
            Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(pointerPosition);
            
            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

            if (hit.collider != null)
            {
                Card clickedCard = hit.collider.GetComponent<Card>();
                if (clickedCard != null)
                {
                    OnCardClicked(clickedCard);
                }
            }
        }
    }

    private IEnumerator DealCardsRoutine()
    {
        CurrentState = GameState.Dealing;
        
        for (int i = 0; i < InitialDealCount; i++)
        {
            DealCardTo(PlayerHand, true);
            yield return new WaitForSeconds(0.15f);
            DealCardTo(BotHand, false);
            yield return new WaitForSeconds(0.15f);
        }
        
        CurrentState = GameState.PlayerSelection;
    }

    private void DealCardTo(CardAnchor targetHand, bool openFace)
    {
        Card card = Deck.DrawCard(Deck.transform.position);
        
        if (card == null) return;
        
        card.SetOpened(openFace);
        targetHand.cards.Add(card);
        targetHand.UpdateLayout(); 
    }

    public void OnCardClicked(Card card)
    {
        if (!PlayerHand.cards.Contains(card)) return;

        if (_playerSelectedCards.Contains(card))
        {
            _playerSelectedCards.Remove(card);
            card.transform.localScale = Vector3.one; 
        }
        else if (_playerSelectedCards.Count < CardsToKeep)
        {
            _playerSelectedCards.Add(card);
            card.transform.localScale = Vector3.one * 1.2f; 
        }

        if (_playerSelectedCards.Count == CardsToKeep)
        {
            StartCoroutine(ProcessSelectionRoutine());
        }
    }

    private IEnumerator ProcessSelectionRoutine()
    {
        CurrentState = GameState.BotTurn;
        yield return new WaitForSeconds(0.5f);

        for (int i = PlayerHand.cards.Count - 1; i >= 0; i--)
        {
            Card card = PlayerHand.cards[i];
            card.transform.localScale = Vector3.one;

            if (!_playerSelectedCards.Contains(card))
            {
                PlayerHand.cards.RemoveAt(i);
                card.SetOpened(false);
                PlayerDiscardPile.cards.Add(card);
            }
        }

        PlayerHand.UpdateLayout();
        PlayerDiscardPile.UpdateLayout();
        
        StartCoroutine(BotTurnRoutine());
    }

    private IEnumerator BotTurnRoutine()
    {
        yield return new WaitForSeconds(1f);

        _botSelectedCards.Add(BotHand.cards[0]);
        _botSelectedCards.Add(BotHand.cards[1]);

        for (int i = BotHand.cards.Count - 1; i >= 0; i--)
        {
            Card card = BotHand.cards[i];
            
            if (!_botSelectedCards.Contains(card))
            {
                BotHand.cards.RemoveAt(i);
                card.SetOpened(false);
                BotDiscardPile.cards.Add(card);
            }
        }

        BotHand.UpdateLayout();
        BotDiscardPile.UpdateLayout();

        yield return new WaitForSeconds(1f);
        
        StartCoroutine(ResolutionAndTablePhaseRoutine());
    }
    
    private IEnumerator ResolutionAndTablePhaseRoutine()
    {
        CurrentState = GameState.Resolution;

        yield return new WaitForSeconds(2f);
        
        CurrentState = GameState.TablePhase;

        foreach (Card c in PlayerDiscardPile.cards) c.SetOpened(false);
        foreach (Card c in BotDiscardPile.cards) c.SetOpened(false);

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < 6; i++) 
        {
            Card card = Deck.DrawCard(Deck.transform.position);
            if (card != null)
            {
                card.SetOpened(false);
                TableAnchor.cards.Add(card);
            }
        }

        TableAnchor.UpdateLayout();
        yield return new WaitForSeconds(0.5f);

        List<Card> leftToRightCards = new List<Card>(TableAnchor.cards);
        leftToRightCards.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));

        for (int i = 0; i < leftToRightCards.Count; i++)
        {
            leftToRightCards[i].SetOpened(true);
            yield return new WaitForSeconds(0.15f);
        }

        yield return new WaitForSeconds(2f);

        foreach (Card c in TableAnchor.cards) c.SetOpened(false);
        yield return new WaitForSeconds(0.5f);

        Vector3 centerPos = TableAnchor.transform.position;
        foreach (Card c in TableAnchor.cards)
        {
            c.MoveTo(centerPos, 0.3f);
        }
        
        yield return new WaitForSeconds(0.4f);

        for (int i = 0; i < TableAnchor.cards.Count; i++)
        {
            int randomIndex = Random.Range(i, TableAnchor.cards.Count);
            Card temp = TableAnchor.cards[i];
            TableAnchor.cards[i] = TableAnchor.cards[randomIndex];
            TableAnchor.cards[randomIndex] = temp;
        }
        
        TableAnchor.UpdateLayout();
        
        yield return new WaitForSeconds(0.6f);

        for (int i = 0; i < 3; i++)
        {
            if (TableAnchor.cards.Count > 0)
            {
                Card card = TableAnchor.cards[0];
                TableAnchor.cards.RemoveAt(0);
                CommonDiscardPile.cards.Add(card);
            }
        }

        TableAnchor.UpdateLayout();
        CommonDiscardPile.UpdateLayout();

        yield return new WaitForSeconds(0.5f);

        if (TableAnchor.cards.Count > 0)
        {
            int randomCardIndex = Random.Range(0, TableAnchor.cards.Count);
            TableAnchor.cards[randomCardIndex].SetOpened(true);
        }
    }
}