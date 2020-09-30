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
    public void Shuffle()  
    {
        System.Random rng = new System.Random();
        int n = this.cardList.Count;  
        while (n > 1)
        {  
            int k = rng.Next(n);
            n--;
            string temp = this.cardList[k];  
            this.cardList[k] = this.cardList[n];  
            this.cardList[n] = temp;
        }
    }

    public string DrawCard()
    {
        string topCard = this.cardList.Last<string>();
        this.cardList.RemoveAt(cardList.Count - 1);
        return topCard;
    }
}
