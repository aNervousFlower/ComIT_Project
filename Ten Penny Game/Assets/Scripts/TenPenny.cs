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
    private GameDeck deck;
    private DiscardPile discardPile;
    private PlayerState playerState;
    private GameRound gameRound;
    private Button playCardsButton;
    // Start is called before the first frame update
    void Start()
    {
        this.playCardsButton = GameObject.Find("PlayCardsButton").GetComponent<Button>();
        PlayCards();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PlayCards()
    {
        this.deck = new GameDeck(2);
        this.playerHand = FindObjectOfType<PlayerHand>();
        this.discardPile = FindObjectOfType<DiscardPile>();
        this.playerState = new PlayerState(this.playerHand);
        this.gameRound = new GameRound(1, 1, 3);

        this.deck.Shuffle();
        DealPlayerHand();
        DealDiscordPile();
    }

    private void DealPlayerHand()
    {
        for (int i = 0; i < 11; i++)
        {
            this.playerHand.AddCard(this.deck.DrawCard());
        }
        this.playerHand.DisplayHand();
    }

    private void DealDiscordPile()
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
        this.playerHand.AddCard(this.deck.DrawCard());
        this.playerHand.RefreshHand();
    }

    public void DiscardCardFromPlayerHand(string card)
    {
        this.playerHand.RemoveCard(card);
        this.discardPile.AddCard(card);
    }

    public void BuyCard()
    {
        if (this.playerState.SpendPenny() == true)
        {
            this.playerHand.AddCard(this.discardPile.DrawCard());
            for (int count = 0; count < 3; count++)
            {
                this.playerHand.AddCard(this.deck.DrawCard());
            }
            this.playerHand.RefreshHand();
        }
    }

    public void SelectPlayerCard(GameObject card)
    {
        // remove colour from card if it is deselected
        if (this.playerHand.SelectCard(card) == false)
        {
            card.GetComponent<UpdateSprite>().SetColour(Color.white);
        }

        // colour the selected cards yellow if they are NOT a valid selection
        // to play, and cyan if they ARE a valid selection to play
        bool playable = this.gameRound.CanPlaySelectedCards(this.playerHand.GetSelectedCardsList());
        this.playCardsButton.interactable = playable;
        Color colour = (playable) ? Color.cyan : Color.yellow;
        foreach (GameObject selectedCard in this.playerHand.selectedCards)
        {
            selectedCard.GetComponent<UpdateSprite>().SetColour(colour);
        }
    }

    public void PlaySelectedCards()
    {
        this.playerHand.PlaySelectedCards(this.gameRound);
        this.playCardsButton.interactable = false;
    }
}
