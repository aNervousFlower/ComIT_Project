using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class OpponentTable : MonoBehaviour
{
    public GameObject cardPrefab;
    public List<string> setTypes;
    public List<CardSet> cardSets;
    public List<GameObject> cardObjectList {get;}
    public List<string> cardList {get;}
    public bool objectiveDone = false;
    
    public OpponentTable()
    {
        this.setTypes = new List<string>();
        this.cardList = new List<string>();
        this.cardObjectList = new List<GameObject>();
        this.cardSets = new List<CardSet>();
    }

    public List<string> PlayCards(List<string> opponentHand, GameRound round)
    {
        List<string> startingCardList = new List<string>(this.cardList);
        SplitIntoSets(opponentHand, round);
        if (this.cardSets.Count != 0)
        {
            DestroySets();
            DisplaySets();
        }

        List<string> currentCardList = new List<string>(this.cardList);
        foreach (string card in startingCardList)
        {
            currentCardList.Remove(card);
        }
        return currentCardList;
    }

    public void SplitIntoSets(List<string> opponentHand, GameRound round)
    {
        List<string> wildcards = new List<string>();
        foreach (string card in opponentHand)
        {
            if (card.Substring(1) == "2" || card.Substring(1) == "JOKER")
            {
                wildcards.Add(card);
            }
            else
            {
                if (!this.cardSets.Exists((set) => set.type == card.Substring(1)))
                {
                    this.cardSets.Add(new CardSet(card.Substring(1)));
                }
                this.cardSets.Find(x => x.type == card.Substring(1)).AddCard(card);
            }
        }
        // Disregard single natural cards
        this.cardSets.RemoveAll((set) => set.GetSize() == 1);

        // Sorts the sets largest to smallest so that it compares the larger sets
        // to the round requirements first. This ensures the most efficient distribution
        // of wildcards among the sets
        this.cardSets.Sort((a, b) => b.GetSize() - a.GetSize());

        int qualifyingSets = 0;
        foreach (CardSet set in this.cardSets)
        {
            // Use wildcards to top up the largest sets until the round requirements are met
            if (!this.objectiveDone && qualifyingSets < round.numOfSets)
            {
                if (!TryAddWildcardsToSet(set, wildcards, round.setsOf))
                {
                    this.cardSets.Clear();
                    return;
                }
                else
                {
                    qualifyingSets++;
                }
            }
            // Use wildcards to top up any sets of 2 to ensure that all sets are at least 3 cards
            else if (set.GetSize() == 2)
            {
                TryAddWildcardsToSet(set, wildcards, 3);
            }
        }
        this.objectiveDone = true;

        // any remaining wildcards should be split among the sets, starting with the largest
        DistributeRemainingWildcards(this.cardSets, wildcards);

        // Disregard remaining natural pairs
        this.cardSets.RemoveAll((set) => set.GetSize() == 2);

        foreach (CardSet set in this.cardSets)
        {
            if (!setTypes.Contains(set.type))
            {
                this.setTypes.Add(set.type);
            }
        }
    }

    private bool TryAddWildcardsToSet(CardSet set, List<string> wildcards, int targetSize)
    {
        while (set.GetSize() < targetSize)
        {
            if (wildcards.Count == 0)
            {
                return false;
            }
            if (!set.AddCard(wildcards[0]))
            {
                return false;
            }
            wildcards.RemoveAt(0);
        }
        return true;
    }

    private void DistributeRemainingWildcards(List<CardSet> setsList, List<string> wildcards)
    {
        int index = 0;
        while (wildcards.Count > 0)
        {
            if (setsList[index].AddCard(wildcards[0]))
            {
                wildcards.RemoveAt(0);
            }
            else
            {
                if (++index == setsList.Count)
                {
                    throw new InvalidOperationException("Invalid Card Selection: too many wildcards");
                }
            }
        }
    }

    private void DisplaySets()
    {
        float yOffset = 0;
        float xOffset = -0.5f;
        foreach (CardSet set in this.cardSets)
        {
            xOffset += 0.5f + (set.GetSize() * 0.09f);
        }
        float zOffset = 0.03f;
        foreach (CardSet set in this.cardSets)
        {
            foreach (string card in set.cards)
            {
                Vector3 vector = new Vector3(
                    this.transform.position.x + xOffset,
                    this.transform.position.y - yOffset,
                    this.transform.position.z + zOffset);
                GameObject newCard = Instantiate(this.cardPrefab, vector,
                    Quaternion.identity, this.transform);
                newCard.transform.localScale += new Vector3(-0.12f, -0.12f, 0);
                newCard.name = card;
                newCard.tag = "OpponentSets";
                newCard.GetComponent<Selectable>().faceUp = true;
                this.cardObjectList.Add(newCard);
                this.cardList.Add(card);

                xOffset -= 0.18f;
                zOffset += 0.03f;
            }
            xOffset -= 1;
        }
    }

    private void DestroySets()
    {
        this.cardList.Clear();
        foreach (GameObject card in this.cardObjectList)
        {
            Destroy(card);
        }
    }
}
