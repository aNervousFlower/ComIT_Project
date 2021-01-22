using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentHand : MonoBehaviour
{
    public List<string> cardList {get;}
    public List<GameObject> cardObjectList {get;}
    public GameObject cardPrefab;
    public PlayerTable opponentTable;
    void Start()
    {
        
    }
    
    public OpponentHand()
    {
        this.cardList = new List<string>();
        this.cardObjectList = new List<GameObject>();
    }

    public void DisplayHand()
    {
        float yOffset = 0;
        float xOffset = (this.cardList.Count - 1) * 0.10f;
        float zOffset = -0.03f;
        foreach (string card in this.cardList)
        {
            Vector3 vector = new Vector3(
                this.transform.position.x + xOffset,
                this.transform.position.y - yOffset,
                this.transform.position.z + zOffset);
            GameObject newCard = Instantiate(this.cardPrefab, vector,
                Quaternion.identity, this.transform);
            newCard.transform.localScale += new Vector3(-0.1f, -0.1f, 0);
            newCard.name = card;
            newCard.tag = "OpponentHand";
// keep faceUp true during testing, switch to false for production
            newCard.GetComponent<Selectable>().faceUp = true;
            this.cardObjectList.Add(newCard);

            xOffset -= 0.2f;
            zOffset -= 0.03f;
        }
    }

    public void DestroyHand()
    {
        foreach (GameObject card in this.cardObjectList)
        {
            Destroy(card);
        }
    }

    public void RefreshHand()
    {
        DestroyHand();
        DisplayHand();
    }
    
    public void AddCard(string card)
    {
        this.cardList.Add(card);
    }

    public void RemoveCard(string card)
    {
        this.cardList.Remove(card);
        RefreshHand();
    }

    public void NewRound()
    {
        DestroyHand();
        this.cardList.Clear();
        this.cardObjectList.Clear();
    }
}
