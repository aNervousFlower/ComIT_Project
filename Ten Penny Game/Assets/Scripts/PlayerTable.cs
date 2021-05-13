using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PlayerTable : MonoBehaviour
{
    public GameObject cardPrefab;
    public List<string> setTypes;
    public List<CardSet> cardSets;
    public List<GameObject> cardObjectList {get;}
    public List<string> cardList {get;}
    public bool objectiveDone = false;
    
    public PlayerTable()
    {
        this.setTypes = new List<string>();
        this.cardList = new List<string>();
        this.cardObjectList = new List<GameObject>();
        this.cardSets = new List<CardSet>();
    }

    public void PlayCards(List<string> selectedCards, GameRound round)
    {
        SplitIntoSets(selectedCards, round);
        round.UpdatePlayedTypes(this.setTypes);
        DestroySets();
        DisplaySets();
    }

    public void SplitIntoSets(List<string> selectedCards, GameRound round)
    {
        List<string> wildcards = new List<string>();
        foreach (string card in selectedCards)
        {
            if (card.Substring(1) == "2" || card.Substring(1) == "JOKER")
            {
                wildcards.Add(card);
            }
            else
            {
                if (!this.setTypes.Contains(card.Substring(1)))
                {
                    this.setTypes.Add(card.Substring(1));
                    this.cardSets.Add(new CardSet(card.Substring(1)));
                }
                this.cardSets.Find(x => x.type == card.Substring(1)).AddCard(card);
            }
        }
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
                AddWildcardsToSet(set, wildcards, round.setsOf);
                qualifyingSets++;
            }
            // Use wildcards to top up any sets of 2 to ensure that all sets are at least 3 cards
            else if (set.GetSize() <= 2 && !round.playedTypes.Contains(set.type))
            {
                AddWildcardsToSet(set, wildcards, 3);
            }
        }
        this.objectiveDone = true;

        // any remaining wildcards should be split among the sets, starting with the largest
        DistributeRemainingWildcards(this.cardSets, wildcards);
    }

    private void AddWildcardsToSet(CardSet set, List<string> wildcards, int targetSize)
    {
        while (set.GetSize() < targetSize)
        {
            if (wildcards.Count == 0)
            {
                throw new InvalidOperationException("Invalid Card Selection: too few wildcards: " + set.ToString());
            }
            if (!set.AddCard(wildcards[0]))
            {
                throw new InvalidOperationException("Invalied Card Selection: too few natural cards: " + set.ToString());
            }
            wildcards.RemoveAt(0);
        }
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
            xOffset += 0.5f + (set.GetSize() * 0.1f);
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
                newCard.transform.localScale += new Vector3(-0.19f, -0.19f, 0);
                newCard.name = card;
                newCard.tag = "PlayerSets";
                newCard.GetComponent<Selectable>().faceUp = true;
                this.cardObjectList.Add(newCard);
                this.cardList.Add(card);

                xOffset -= 0.2f;
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

    public int GetTotalWilds()
    {
        int totalWilds = 0;
        foreach (CardSet set in this.cardSets)
        {
            totalWilds += set.wilds;
        }
        return totalWilds;
    }

    public int GetTotalNaturals()
    {
        int totalNaturals = 0;
        foreach (CardSet set in this.cardSets)
        {
            totalNaturals += set.naturals;
        }
        return totalNaturals;
    }

    public void NewRound()
    {
        foreach (GameObject card in this.cardObjectList)
        {
            Destroy(card);
        }
        this.setTypes.Clear();
        this.cardSets.Clear();
        this.objectiveDone = false;
    }
}
