using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameRound
{
    public Text roundInfoText;
    public int round {get;}
    public int numOfSets {get;}
    public int setsOf {get;}
    
    public GameRound(int round, int numOfSets, int setsOf)
    {
        this.round = round;
        this.numOfSets = numOfSets;
        this.setsOf = setsOf;
        this.roundInfoText = GameObject.Find("RoundInfo").GetComponent<Text>();
        SetText();
    }

    public void SetText()
    {
        string set = (this.numOfSets == 1) ? " set of " : " sets of ";
        this.roundInfoText.text = "Round " + this.round.ToString() + "\n" +
            this.numOfSets.ToString() + set + this.setsOf.ToString();
    }
}
