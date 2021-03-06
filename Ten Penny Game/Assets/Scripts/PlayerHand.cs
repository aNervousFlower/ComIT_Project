﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerHand : MonoBehaviour
{
    public List<string> cardList {get;}
    public List<GameObject> cardObjectList {get;}
    public List<GameObject> selectedCards {get;}
    public GameObject cardPrefab;
    public PlayerTable playerTable;

    void Start()
    {
        this.playerTable = FindObjectOfType<PlayerTable>();
    }
    
    public PlayerHand()
    {
        this.cardList = new List<string>();
        this.cardObjectList = new List<GameObject>();
        this.selectedCards = new List<GameObject>();
    }

    public void DisplayHand()
    {
        float yOffset = 0;
        float xOffset = (this.cardList.Count - 1) * 0.15f;
        float zOffset = 0.03f;
        foreach (string card in this.cardList)
        {
            Vector3 vector = new Vector3(
                this.transform.position.x + xOffset,
                this.transform.position.y - yOffset,
                this.transform.position.z + zOffset);
            GameObject newCard = Instantiate(this.cardPrefab, vector,
                Quaternion.identity, this.transform);
            newCard.name = card;
            newCard.tag = "PlayerHand";
            newCard.GetComponent<Selectable>().faceUp = true;
            this.cardObjectList.Add(newCard);

            xOffset -= 0.3f;
            zOffset += 0.03f;
        }
    }

    public void DestroyHand()
    {
        foreach (GameObject card in this.cardObjectList)
        {
            this.selectedCards.Remove(card);
            Destroy(card);
        }
    }

    public void RefreshHand()
    {
        DestroyHand();
        DisplayHand();
    }
    
    public void AddCard(string card)
    {
        this.cardList.Add(card);
    }

    public void RemoveCard(string card)
    {
        this.cardList.Remove(card);
        RefreshHand();
    }

    public void NewRound()
    {
        DestroyHand();
        this.cardList.Clear();
        this.cardObjectList.Clear();
        this.selectedCards.Clear();
    }

    public void SortCards()
    {
        this.cardList.Sort(CompareCards);
    }

    public static int CompareCards(string c1, string c2)
    {
        // returns 0 if the cards are equal
        // returns positive int if c1 sorts BEFORE c2
        // returns negative int if c1 sorts AFTER c2
        // overall sort is reversed because they are displayed in reverse order
        if (c1.Substring(1) == "JOKER")
        {
            return 1;
        }
        else if (c2.Substring(1) == "JOKER")
        {
            return -1;
        }

        int c1value = Int32.Parse(c1.Substring(1));
        int c2value = Int32.Parse(c2.Substring(1));

        if (c1value == c2value)
        {
            return c1.Substring(0,1).CompareTo(c2.Substring(0,1));
        }
        else if (c1value == 1)
        {
            return -1;
        }
        else if (c2value == 1)
        {
            return 1;
        }
        return c2value - c1value;
    }

    public bool SelectCard(GameObject card)
    {
        if (this.selectedCards.Contains(card) == false)
        {
            this.selectedCards.Add(card);
            return true;
        }
        else
        {
            this.selectedCards.Remove(card);
            return false;
        }
    }

    public List<string> GetSelectedCardsList()
    {
        List<string> selectedList = new List<string>();
        foreach (GameObject card in this.selectedCards)
        {
            selectedList.Add(card.name);
        }
        selectedList.Sort(CompareCards);
        return selectedList;
    }

    public void PlaySelectedCards(GameRound round)
    {
        List<string> selectedCardList = GetSelectedCardsList();
        this.playerTable.PlayCards(selectedCardList, round);
        foreach (string card in selectedCardList)
        {
            this.cardList.Remove(card);
        }
        RefreshHand();
    }
}
