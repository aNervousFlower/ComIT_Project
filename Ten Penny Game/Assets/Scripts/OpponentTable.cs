using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentTable : MonoBehaviour
{
    public GameObject cardPrefab;
    public List<string> setTypes;
    public List<CardSet> cardSets;
    public List<GameObject> cardObjectList {get;}
    public List<string> cardList {get;}
    public bool objectiveDone = false;
    
    public OpponentTable()
    {
        this.setTypes = new List<string>();
        this.cardList = new List<string>();
        this.cardObjectList = new List<GameObject>();
        this.cardSets = new List<CardSet>();
    }
}
