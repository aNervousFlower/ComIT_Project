using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TenPenny : MonoBehaviour
{
    public Sprite[] cardFaces;
    public static string[] suits = new string[] { "C", "D", "H", "S"};
    public List<string> deck;
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
        deck = GenerateDeck();

        foreach (string card in deck)
        {
            print(card);
        }
        Shuffle(deck);
    }

    public static List<string> GenerateDeck()
    {
        int numOfDecks = 2;
        List<string> newDeck = new List<string>();
        for (int deckCount = 0; deckCount < numOfDecks; deckCount++)
        {
            newDeck.Add("JOKER"); // each deck contains two jokers
            newDeck.Add("JOKER");
            foreach (string suit in suits)
            {
                for (int value = 1; value <= 13; value++)
                {
                    newDeck.Add(suit + value);
                }
            }
        }
        return newDeck;
    }

    // shuffle algorithm found at https://stackoverflow.com/questions/273313/randomize-a-listt
    private static void Shuffle<T>(List<T> list)  
    {
        System.Random rng = new System.Random();
        int n = list.Count;  
        while (n > 1)
        {  
            int k = rng.Next(n);
            n--;
            T temp = list[k];  
            list[k] = list[n];  
            list[n] = temp;
        }
    }
}
