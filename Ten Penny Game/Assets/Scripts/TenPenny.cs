using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class TenPenny : MonoBehaviour
{
    public Sprite[] cardFaces;
    public GameObject cardPrefab;
    public GameObject playerHandPos;
    public static string[] suits = new string[] { "C", "D", "H", "S"};
    public List<string> playerHand = new List<string>();
    public GameDeck deck;
    // Start is called before the first frame update
    void Start()
    {
        PlayCards();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayCards()
    {
        this.deck = new GameDeck(2);

        // foreach (string card in deck)
        // {
        //     print(card);
        // }
        this.deck.Shuffle();
        DealPlayerHand();
    }

    void DealPlayerHand()
    {
        for (int i = 0; i < 11; i++)
        {
            playerHand.Add(this.deck.DrawCard());
        }
        DisplayPlayerHand();
    }

    public void SortPlayerHand()
    {
        DestroyPlayerHand();
        SortCards(playerHand);
        DisplayPlayerHand();
    }

    public static void SortCards(List<string> cards)
    {
        cards.Sort(CompareCards);
    }

    public static int CompareCards(string c1, string c2)
    {
        // returns 0 if the cards are equal
        // returns positive int if c1 sorts BEFORE c2
        // returns negative int if c1 sorts AFTER c2
        // overall sort is reversed because they are displayed in reverse order
        if (c1.Substring(1) == "JOKER")
        {
            return 1;
        }
        else if (c2.Substring(1) == "JOKER")
        {
            return -1;
        }

        int c1value = Int32.Parse(c1.Substring(1));
        int c2value = Int32.Parse(c2.Substring(1));

        if (c1value == c2value)
        {
            return c1.Substring(0,1).CompareTo(c2.Substring(0,1));
        }
        else if (c1value == 1)
        {
            return -1;
        }
        else if (c2value == 1)
        {
            return 1;
        }
        return c2value - c1value;
    }

    private void DisplayPlayerHand()
    {
        float yOffset = 0;
        float xOffset = 0;
        float zOffset = 0.03f;
        foreach (string card in playerHand)
        {
            Vector3 vector = new Vector3(playerHandPos.transform.position.x + xOffset,
                playerHandPos.transform.position.y - yOffset, playerHandPos.transform.position.z + zOffset);
            GameObject newCard = Instantiate(cardPrefab, vector, Quaternion.identity, playerHandPos.transform);
            newCard.name = card;
            newCard.tag = "PlayerHand";
            newCard.GetComponent<Selectable>().faceUp = true;

            xOffset -= 0.3f;
            zOffset += 0.03f;
        }
    }

    private void DestroyPlayerHand()
    {
        GameObject[] hand = GameObject.FindGameObjectsWithTag("PlayerHand");
        foreach (GameObject card in hand)
        {
            Destroy(card);
        }
    }

    public void DrawCardToPlayerHand()
    {
        playerHand.Add(this.deck.DrawCard());
        DestroyPlayerHand();
        DisplayPlayerHand();
    }
}
