using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class TenPenny : MonoBehaviour
{
    public Sprite[] cardFaces;
    private PlayerHand playerHand;
    private OpponentHand opponentHand;
    private GameDeck deck;
    private DiscardPile discardPile;
    private PlayerState playerState;
    private OpponentState opponentState;
    private GameRound gameRound;
    private int roundNum = 1;
    private Button playCardsButton;
    private bool singlePlayer = false;

    void Start()
    {
        this.playCardsButton = GameObject.Find("PlayCardsButton").GetComponent<Button>();
        this.playerHand = FindObjectOfType<PlayerHand>();
        this.opponentHand = FindObjectOfType<OpponentHand>();
        this.discardPile = FindObjectOfType<DiscardPile>();
        this.playerState = new PlayerState(this.playerHand);
        this.opponentState = new OpponentState(this.opponentHand);
        PlayCards(1, 3);
    }

    void Update()
    {
        
    }

    private void PlayCards(int numOfSets, int setsOf)
    {
        this.gameRound = new GameRound(this.roundNum++, numOfSets, setsOf);
        this.deck = new GameDeck(2);

        GameDeck.Shuffle(this.deck.cardList);
        DealAllHands();
        DealDiscardPile();
    }

    private void DealAllHands()
    {
        for (int i = 0; i < 11; i++)
        {
            this.playerHand.AddCard(this.deck.DrawCard());
            this.opponentHand.AddCard(this.deck.DrawCard());
        }
        SortPlayerHand();
        this.opponentHand.RefreshHand();
    }

    private void DealDiscardPile()
    {
        this.discardPile.AddCard(this.deck.DrawCard());
    }

    public void SortPlayerHand()
    {
        this.playerHand.SortCards();
        this.playerHand.RefreshHand();
    }

    public void DrawCardToPlayerHand()
    {
        if (this.playerState.Draw())
        {
            this.playerHand.AddCard(this.deck.DrawCard());
            this.playerHand.RefreshHand();
            CheckAndReplenishDeck();
        }
    }

    public void DiscardCardFromPlayerHand(string card)
    {
        if (this.playerState.Discard())
        {
            this.playerHand.RemoveCard(card);
            this.discardPile.AddCard(card);
            if (this.playerHand.cardList.Count == 0)
            {
                StartNewRound();
            }
            else if (this.singlePlayer)
            {
                DealDiscardPile();
                CheckAndReplenishDeck();
            }
            else
            {
                OpponentTurn();
            }
        }
    }

    public void BuyCard()
    {
        if (this.playerState.Buy())
        {
            if (this.playerState.SpendPenny() == true)
            {
                this.playerHand.AddCard(this.discardPile.DrawCard());
                for (int count = 0; count < 3; count++)
                {
                    this.playerHand.AddCard(this.deck.DrawCard());
                }
                CheckAndReplenishDeck();
                this.playerHand.RefreshHand();
            } 
        }
    }

    public void SelectPlayerCard(GameObject card)
    {
        if (!this.playerState.CanPlay())
        {
            return;
        }
        // remove colour from card if it is deselected
        if (this.playerHand.SelectCard(card) == false)
        {
            card.GetComponent<UpdateSprite>().SetColour(Color.white);
        }

        // colour the selected cards yellow if they are NOT a valid selection
        // to play, and cyan if they ARE a valid selection to play
        bool playable = this.gameRound.CanPlaySelectedCards(
            this.playerHand.GetSelectedCardsList(), this.playerState.GetPlayedWilds(),
            this.playerState.GetPlayedNaturals());
        this.playCardsButton.interactable = playable;
        Color colour = (playable) ? Color.cyan : Color.yellow;
        foreach (GameObject selectedCard in this.playerHand.selectedCards)
        {
            selectedCard.GetComponent<UpdateSprite>().SetColour(colour);
        }
    }

    public void PlaySelectedCards()
    {
        if (this.playerState.Play())
        {
            this.playerHand.PlaySelectedCards(this.gameRound);
            this.playCardsButton.interactable = false;
            if (this.playerHand.cardList.Count == 0 || this.opponentHand.cardList.Count == 0)
            {
                StartNewRound();
            }            
        }
    }

    public void StartNewRound()
    {
        this.playerState.NewRound();
        this.opponentState.NewRound();
        if (this.roundNum <= 8)
        {
            int numOfSets = this.gameRound.numOfSets == 1 ? 2 : 1;
            int setsOf = numOfSets == 1 ? this.gameRound.setsOf + 1 : this.gameRound.setsOf;
            this.discardPile.EmptyDiscardPile();
            PlayCards(numOfSets, setsOf);
        }
        else
        {
            this.gameRound.GameOver();
        }
    }

    private void OpponentTurn()
    {
        StartCoroutine(this.opponentHand.MoveCardToHand(this.ContinueOpponentTurn));
    }

    // split out this logic so game will wait for card to be drawn to opponent hand
    public void ContinueOpponentTurn()
    {
        DrawCardToOpponentHand();
        this.opponentHand.RefreshHand();
        PlayCardsFromOpponentHand();
        if (this.opponentHand.cardList.Count > 0)
        {
            string card = this.opponentHand.DiscardCard();
            this.opponentHand.RefreshHand();
            StartCoroutine(this.opponentHand.MoveCardToDiscard(this.EndOpponentTurn, card));
        }
        else
        {
            EndOpponentTurn();
        }
    }

    // split out this logic so game will wait for card to be discarded
    public void EndOpponentTurn(string card = "")
    {
        if (card != "")
        {
            this.discardPile.AddCard(card);
        }

        if (this.opponentHand.cardList.Count == 0)
        {
            StartNewRound();
        }
        else
        {
            this.opponentHand.RefreshHand();
        }
    }

    private void DrawCardToOpponentHand()
    {
        this.opponentHand.AddCard(this.deck.DrawCard());
        CheckAndReplenishDeck();
    }

    private void PlayCardsFromOpponentHand()
    {
        this.opponentHand.PlayCards(this.gameRound);
    }

    public void CheckAndReplenishDeck()
    {
        if (this.deck.DeckLowOnCards())
        {
            List<string> cards = this.discardPile.RemoveCardsForDeck();
            this.deck.ReplenishDeck(cards);
        }
    }
}
