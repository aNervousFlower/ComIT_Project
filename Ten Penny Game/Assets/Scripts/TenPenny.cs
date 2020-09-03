using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TenPenny : MonoBehaviour
{
    public Sprite[] cardFaces;
    public GameObject cardPrefab;
    public GameObject playerHandPos;
    public static string[] suits = new string[] { "C", "D", "H", "S"};
    public List<string> playerHand = new List<string>();
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
        deck = GenerateDeck(2);

        // foreach (string card in deck)
        // {
        //     print(card);
        // }
        Shuffle(deck);
        DealHand();
    }

    public static List<string> GenerateDeck(int numOfDecks = 1)
    {
        List<string> newDeck = new List<string>();
        for (int deckCount = 0; deckCount < numOfDecks; deckCount++)
        {
            newDeck.Add("1JOKER"); // each deck contains two jokers
            newDeck.Add("2JOKER");
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
    public static void Shuffle<T>(List<T> list)  
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

    void DealHand()
    {
        for (int i = 0; i < 11; i++)
        {
            playerHand.Add(deck.Last<string>());
            deck.RemoveAt(deck.Count - 1);
        }

        float yOffset = 0;
        float xOffset = 0;
        float zOffset = 0.03f;
        foreach (string card in playerHand)
        {
            Vector3 vector = new Vector3(playerHandPos.transform.position.x + xOffset,
                playerHandPos.transform.position.y - yOffset, playerHandPos.transform.position.z + zOffset);
            GameObject newCard = Instantiate(cardPrefab, vector, Quaternion.identity, playerHandPos.transform);
            newCard.name = card;
            newCard.GetComponent<Selectable>().faceUp = true;

            xOffset -= 0.3f;
            zOffset += 0.03f;
        }
    }
}
