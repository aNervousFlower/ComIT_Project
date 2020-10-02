using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class TenPenny : MonoBehaviour
{
    public Sprite[] cardFaces;
    public GameObject playerHandPos;
    private PlayerHand playerHand;
    private GameDeck deck;
    // Start is called before the first frame update
    void Start()
    {
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

        // foreach (string card in deck)
        // {
        //     print(card);
        // }
        this.deck.Shuffle();
        DealPlayerHand();
    }

    private void DealPlayerHand()
    {
        for (int i = 0; i < 11; i++)
        {
            this.playerHand.AddCard(this.deck.DrawCard());
        }
        this.playerHand.DisplayHand();
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
}
