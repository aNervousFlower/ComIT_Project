using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class DiscardPile : MonoBehaviour
{
    public List<string> cardList {get;}
    private GameObject topCardObject;
    public GameObject cardPrefab;
    
    public DiscardPile()
    {
        this.cardList = new List<string>();
    }
    
    public void AddCard(string card)
    {
        this.cardList.Add(card);
        RefreshTopCard();
    }

    public string DrawCard()
    {
        string topCard = this.cardList.Last<string>();
        this.cardList.RemoveAt(cardList.Count - 1);
        RefreshTopCard();
        return topCard;
    }
    
    public void EmptyDiscardPile()
    {
        this.cardList.Clear();
        RefreshTopCard();
    }

    private void DisplayTopCard()
    {
        string topCard = this.cardList.Last<string>();
        Vector3 vector = new Vector3(
            this.transform.position.x,
            this.transform.position.y,
            this.transform.position.z);
        GameObject newCardObject = Instantiate(this.cardPrefab, vector,
            Quaternion.identity, this.transform);
        newCardObject.name = topCard;
        newCardObject.tag = "DiscardPile";
        newCardObject.GetComponent<Selectable>().faceUp = true;
        this.topCardObject = newCardObject;
    }

    private void DestroyTopCardObject()
    {
        if (this.topCardObject != null)
        {
            Destroy(this.topCardObject);
        }
    }

    public void RefreshTopCard()
    {
        DestroyTopCardObject();
        if (this.cardList.Count > 0)
        {
            DisplayTopCard();
        }
    }

    public List<string> RemoveCardsForDeck()
    {
        int cardsToRemove = this.cardList.Count - 3;
        if (cardsToRemove <= 0)
        {
            throw new InvalidOperationException("Discard Pile must contain " +
                "at least 4 cards to add them back to the deck. " +
                "Pile currently contains " + this.cardList.Count + " cards.");
        }
        List<string> cards = new List<string>();
        for (int index = 0; index < cardsToRemove; index++)
        {
            cards.Add(this.cardList[index]);
        }
        this.cardList.RemoveRange(0, cardsToRemove);
        return cards;
    }
}
