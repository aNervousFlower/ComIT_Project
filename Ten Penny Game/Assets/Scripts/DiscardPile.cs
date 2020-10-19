using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
}
