using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentHand : MonoBehaviour
{
    public List<string> cardList {get;}
    public List<GameObject> cardObjectList {get;}
    public GameObject cardPrefab;
    public OpponentTable opponentTable;
    
    void Start()
    {
        this.opponentTable = FindObjectOfType<OpponentTable>();
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
            newCard.GetComponent<Selectable>().faceUp = false;
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
        this.cardList.Sort(PlayerHand.CompareCards);
        DestroyHand();
        DisplayHand();
    }
    
    public void AddCard(string card)
    {
print("opponent draws " + card);
        this.cardList.Add(card);
    }

    public string DiscardCard()
    {
        List<string> setTypes = new List<string>();
        List<CardSet> cardSets = new List<CardSet>();
        foreach (string card in this.cardList)
        {
            if (card.Substring(1) == "2" || card.Substring(1) == "JOKER")
            {
                continue;
            }
            else
            {
                if (!setTypes.Contains(card.Substring(1)))
                {
                    setTypes.Add(card.Substring(1));
                    cardSets.Add(new CardSet(card.Substring(1)));
                }
                cardSets.Find(x => x.type == card.Substring(1)).AddCard(card);
            }
        }
        cardSets.Sort(CompareSetsForDiscard);

        string cardToDiscard = cardSets[0].cards[0];

print("opponent discards " + cardToDiscard);
        RemoveCard(cardToDiscard);
        return cardToDiscard;
    }

    public static int CompareSetsForDiscard(CardSet s1, CardSet s2)
    {
        if (s1.GetSize() != s2.GetSize())
        {
            return s1.GetSize() - s2.GetSize();
        }
        else
        {
            return PlayerState.GetCardPoints(s1.cards[0]) - PlayerState.GetCardPoints(s2.cards[0]);
        }
    }

    public void RemoveCard(string card)
    {
        this.cardList.Remove(card);
    }

    public void PlayCards(GameRound round)
    {
        List<string> playedCards = this.opponentTable.PlayCards(this.cardList, round);
        foreach (string card in playedCards)
        {
            this.cardList.Remove(card);
        }
    }

    public void NewRound()
    {
        DestroyHand();
        this.cardList.Clear();
        this.cardObjectList.Clear();
    }
}
