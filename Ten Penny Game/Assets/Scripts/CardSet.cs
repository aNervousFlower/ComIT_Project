using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSet
{
    public string type;
    public int naturals = 0;
    public int wilds = 0;
    public List<string> cards;

    public CardSet(string type)
    {
        this.type = type;
        this.cards = new List<string>();
    }

    public bool AddCard(string card)
    {
        if (card.Substring(1) == this.type)
        {
            this.cards.Add(card);
            this.naturals++;
            return true;
        }
        else if (card.Substring(1) == "2" || card.Substring(1) == "JOKER")
        {
            if (this.wilds < this.naturals)
            {
            this.cards.Insert(0, card);
            this.wilds++;
            return true;
            }
        }
        return false;
    }

    public int GetSize()
    {
        return this.cards.Count;
    }

    public override string ToString()
    {
        string s = "";
        foreach (string card in this.cards)
        {
            s += ", " + card;
        }
        return s.Substring(2);
    }
}
