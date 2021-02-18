using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class OpponentState
{
    public Text opponentScoreText;
    public int opponentScore = 0;
    public OpponentHand opponentHand {get;}
    public OpponentTable opponentTable {get;}
    
    public OpponentState(OpponentHand opponentHand)
    {
        this.opponentScoreText = GameObject.Find("OpponentScoreText").GetComponent<Text>();
        this.opponentHand = opponentHand;
        this.opponentTable = opponentHand.opponentTable;
    }

    public void NewRound()
    {
        TallyPlayerScore();
        this.opponentHand.NewRound();
        this.opponentTable.NewRound();
    }

    public void TallyPlayerScore()
    {
        int positivePoints = 0;
        int negativePoints = 0;
        foreach(string card in this.opponentTable.cardList)
        {
            positivePoints += PlayerState.GetCardPoints(card);
        }

        foreach(string card in this.opponentHand.cardList)
        {
            negativePoints += PlayerState.GetCardPoints(card);
        }
        
        this.opponentScore += positivePoints - negativePoints;
        this.opponentScoreText.text = "Score\n" + this.opponentScore;
    }
}
