using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameDeck
{
    public List<string> cardList {get;}
    public static string[] suits = new string[] { "C", "D", "H", "S"};
    public GameDeck(int numOfDecks = 1)
    {
        this.cardList = new List<string>();
        for (int deckCount = 0; deckCount < numOfDecks; deckCount++)
        {
            this.cardList.Add("1JOKER"); // each deck contains two jokers
            this.cardList.Add("2JOKER");
            foreach (string suit in GameDeck.suits)
            {
                for (int value = 1; value <= 13; value++)
                {
                    this.cardList.Add(suit + value);
                }
            }
        }
    }

    // shuffle algorithm found at https://stackoverflow.com/questions/273313/randomize-a-listt
    public static void Shuffle(List<string> cardList)  
    {
        System.Random rng = new System.Random();
        int n = cardList.Count;  
        while (n > 1)
        {  
            int k = rng.Next(n);
            n--;
            string temp = cardList[k];  
            cardList[k] = cardList[n];  
            cardList[n] = temp;
        }
    }

    public string DrawCard()
    {
        string topCard = this.cardList.Last<string>();
        this.cardList.RemoveAt(cardList.Count - 1);
        return topCard;
    }

    public bool DeckLowOnCards()
    {
        return this.cardList.Count < 4;
    }

    public void ReplenishDeck(List<string> newCards)
    {
        Shuffle(newCards);
        this.cardList.InsertRange(0, newCards);
    }
}
