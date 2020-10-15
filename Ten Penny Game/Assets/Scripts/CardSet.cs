using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSet
{
    public string type;
    public int naturals;
    public int wilds;
    public CardSet(string type, int naturals, int wilds = 0)
    {
        this.type = type;
        this.naturals = naturals;
        this.wilds = wilds;
    }

    public bool AddNaturals(string type, int naturals)
    {
        if (this.type == type)
        {
            this.naturals += naturals;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool AddWilds(int wilds)
    {
        if (wilds <= this.naturals - this.wilds)
        {
            this.wilds += wilds;
            return true;
        }
        else
        {
            return false;
        }
    }
}
