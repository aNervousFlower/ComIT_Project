using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState
{
    public Text pennyCountText;
    public Text playerActionsInfoText;
    public int pennyCount = 10;
    public List<string> playerActions {get;}
    private PlayerHand playerHand;
    private PlayerTable playerTable;

    public PlayerState(PlayerHand playerHand)
    {
        this.pennyCountText = GameObject.Find("PlayerPenniesCount").GetComponent<Text>();
        this.pennyCountText.text = this.pennyCount.ToString();
        this.playerHand = playerHand;
        this.playerTable = playerHand.playerTable;
        this.playerActionsInfoText = GameObject.Find("PlayerActionsInfo").GetComponent<Text>();
        this.playerActions = new List<string>();
        this.playerActions.AddRange(new List<string> {"Buy", "Draw"});
        SetActionText();
    }

    private void SetActionText()
    {
        List<string> split = new List<string>() {"", " or ", ", "};
        string text = "";
        for (int index = 0; index < this.playerActions.Count; index++)
        {
            text = this.playerActions[index] + split[index] + text;
        }
        this.playerActionsInfoText.text = text;
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

    public void NewRound()
    {
        this.playerActions.Clear();
        if (this.pennyCount > 0)
        {
            this.playerActions.Add("Buy");
        }
        this.playerActions.Add("Draw");
        this.playerTable.NewRound();
        SetActionText();
    }

    public bool Draw()
    {
        if (!this.playerActions.Contains("Draw"))
        {
            return false;
        }
        else
        {
            this.playerActions.Clear();
            this.playerActions.Add("Discard");
            this.playerActions.Add("Play");
            SetActionText();
            return true;
        }
    }

    public bool Buy()
    {
        if (!this.playerActions.Contains("Buy"))
        {
            return false;
        }
        else
        {
            this.playerActions.Clear();
            this.playerActions.Add("Discard");
            this.playerActions.Add("Play");
            if (this.pennyCount > 1)
            {
                this.playerActions.Add("Buy");
            }
            SetActionText();
            return true;
        }
    }

    public bool CanPlay()
    {
        return this.playerActions.Contains("Play");
    }

    public bool Play()
    {
        if (!CanPlay())
        {
            return false;
        }
        else
        {
            this.playerActions.Clear();
            this.playerActions.Add("Discard");
            this.playerActions.Add("Play");
            SetActionText();
            return true;
        }
    }

    public bool Discard()
    {
        if (!this.playerActions.Contains("Discard"))
        {
            return false;
        }
        else
        {
            this.playerActions.Clear();
            if (this.pennyCount > 0)
            {
                this.playerActions.Add("Buy");
            }
            this.playerActions.Add("Draw");
            SetActionText();
            return true;
        }
    }
}
