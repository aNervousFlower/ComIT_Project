using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState
{
    public Text pennyCountText;
    public int pennyCount = 10;
    private PlayerHand playerHand;

    public PlayerState(PlayerHand playerHand)
    {
        this.pennyCountText = GameObject.Find("PlayerPenniesCount").GetComponent<Text>();
        this.pennyCountText.text = this.pennyCount.ToString();
        this.playerHand = playerHand;
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
}
