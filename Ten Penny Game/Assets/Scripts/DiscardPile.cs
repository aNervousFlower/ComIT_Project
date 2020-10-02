using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardPile : MonoBehaviour
{
    public List<string> cardList {get;}
    public GameObject cardPrefab;
    
    public DiscardPile()
    {
        this.cardList = new List<string>();
    }
    
    public void AddCard(string card)
    {
        this.cardList.Add(card);
        
        Vector3 vector = new Vector3(
            this.transform.position.x,
            this.transform.position.y,
            this.transform.position.z);
        GameObject newCard = Instantiate(this.cardPrefab, vector,
            Quaternion.identity, this.transform);
        newCard.name = card;
        newCard.tag = "DiscardPile";
        newCard.GetComponent<Selectable>().faceUp = true;
    }
}
