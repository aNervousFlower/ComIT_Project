using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class OpponentState
{
    public OpponentHand opponentHand {get;}
    public OpponentTable opponentTable {get;}
    
    public OpponentState(OpponentHand opponentHand)
    {
        this.opponentHand = opponentHand;
        this.opponentTable = opponentHand.opponentTable;
    }
}
