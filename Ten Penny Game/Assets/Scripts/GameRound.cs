using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class GameRound
{
    public Text roundInfoText;
    public int numOfSets {get;}
    public int setsOf {get;}
    
    public GameRound(int round, int numOfSets, int setsOf)
    {
        this.numOfSets = numOfSets;
        this.setsOf = setsOf;
        this.roundInfoText = GameObject.Find("RoundInfo").GetComponent<Text>();
        SetText(round);
    }

    public void SetText(int round)
    {
        string set = (this.numOfSets == 1) ? " set of " : " sets of ";
        this.roundInfoText.text = "Round " + round.ToString() + "\n" +
            this.numOfSets.ToString() + set + this.setsOf.ToString();
    }

    public bool CanPlaySelectedCards(List<string> selectedCards, List<string> playedTypes,
        int playedWildCards, int playedNaturalCards)
    {
        // if the cards haven't been played already and there aren't
        // enough existing cards to meet the objective of the round
        if (selectedCards.Count < this.numOfSets * this.setsOf &&
            playedWildCards + playedNaturalCards == 0)
        {
            return false;
        }
        
        int wildCards = 0;
        int naturalCards = playedNaturalCards;

        // creates an object of groupings
        // each grouping has a key of the card type and contains all the cards of that type
        var g = selectedCards.GroupBy( i => i.Substring(1) );
        // cardGroups creates a dictionary with a key representing a number of cards
        // and containing all the types of cards with than number of cards
        Dictionary<int, List<string>> cardGroups = new Dictionary<int, List<string>>();
        int biggestSet = 0;

        foreach (var group in g)
        {
            int count = group.Count();
            if (group.Key == "JOKER" || group.Key == "2")
            {
                wildCards += count;
            }
            // types that have already been played do not need to be checked, they are free
            else if (playedTypes.Contains(group.Key))
            {
                naturalCards += count;
            }
            else
            {
                if (!cardGroups.ContainsKey(count))
                {
                    cardGroups.Add(count, new List<string>());
                    if (count > biggestSet)
                    {
                        biggestSet = count;
                    }
                }
                cardGroups[count].Add(group.Key);
                naturalCards += count;
            }
        }

        // initial disqualifiers:
        // if there are any single natural cards
        // if there are more wild cards than natural cards
        if (cardGroups.ContainsKey(1) || wildCards > naturalCards)
        {
            return false;
        }
        
        int qualifyingSets = 0;
        for (int size = biggestSet; size >= 2; size--)
        {
            if (cardGroups.ContainsKey(size))
            {
                if (qualifyingSets >= this.numOfSets || playedNaturalCards + playedWildCards > 0)
                {
                    // if the required number of qualifying sets have been reached
                    // and there are remaining pairs of cards left, make sure there
                    // are enough wildcards remaining to make them all sets of three
                    if (size == 2)
                    {
                        if (wildCards < cardGroups[size].Count())
                        {
                            return false;
                        }
                    }
                    else
                    {
                        break; 
                    }
                }
                // if the set is at least as big as the target size
                else if (size >= this.setsOf)
                {
                    qualifyingSets += cardGroups[size].Count();
                }
                // if the largest set not checked yet is less than half the size of
                // the target size, wildcards cannot be used to make it qualify
                else if (size*2 < this.setsOf)
                {
                    return false;
                }
                // try to use wildcards to increase the size of the sets until they are
                // big enough to qualify
                else
                {
                    int wildCardsNeeded = this.setsOf - size;
                    for (int setCount = cardGroups[size].Count(); setCount > 0; setCount--)
                    {
                        // if the required number of qualifying sets have been reached
                        // and there are remaining pairs of cards left, make sure there
                        // are enough wildcards remaining to make them all sets of three
                        if (qualifyingSets >= this.numOfSets && size == 2)
                        {
                            if (--wildCards < 0)
                            {
                                return false;
                            }
                        }
                        // if the required number of qualifying sets have not yet been reached
                        // and there aren't enough wildcards needed to pad them, return false
                        else if (wildCardsNeeded > wildCards && qualifyingSets < this.numOfSets)
                        {
                            return false;
                        }
                        // subtract the required wildcards to pad the set and increment the number
                        // of qualifying sets
                        else
                        {
                            wildCards -= wildCardsNeeded;
                            qualifyingSets++;
                        }
                    }
                }
            }
        }
        return (qualifyingSets >= this.numOfSets || playedNaturalCards + playedWildCards > 0);
    }
}
