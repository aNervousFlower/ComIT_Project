using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState
{
    public Text pennyCountText;
    public int pennyCount = 10;
    private PlayerHand playerHand;
    private PlayerTable playerTable;

    public PlayerState(PlayerHand playerHand)
    {
        this.pennyCountText = GameObject.Find("PlayerPenniesCount").GetComponent<Text>();
        this.pennyCountText.text = this.pennyCount.ToString();
        this.playerHand = playerHand;
        this.playerTable = playerHand.playerTable;
    }

    public bool SpendPenny()
    {
        if (this.pennyCount > 0)
        {
            this.pennyCount--;
            this.pennyCountText.text = this.pennyCount.ToString();
            return true;
        }
        else
        {
            return false;
        }
    }

    public List<string> GetPlayedTypes()
    {
        return this.playerTable.setTypes;
    }

    public int GetPlayedWilds()
    {
        return this.playerTable.GetTotalWilds();
    }

    public int GetPlayedNaturals()
    {
        return this.playerTable.GetTotalNaturals();
    }
}
