using System.Collections;
using System.Collections.Generic;
using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;

namespace Tests
{
    public class OpponentTable_Tests
    {
        [Test]
        public void SplitIntoSets_Test()
        {
            OpponentTable table = new GameObject().AddComponent<OpponentTable>();
            GameObject gameOb = new GameObject();
            gameOb.name = "RoundInfo";
            gameOb.AddComponent<Text>();
            GameRound round1 = new GameRound(1, 1, 3);

            List<string> cards = new List<string>() {"D3", "H3", "C3"};
            table.SplitIntoSets(cards, round1);
            List<CardSet> sets = table.cardSets;
            Assert.AreEqual(1, table.setTypes.Count);
            Assert.Contains("3", table.setTypes);

            Assert.AreEqual(1, sets.Count);
            Assert.AreEqual("3", sets[0].type);
            Assert.AreEqual(3, sets[0].GetSize());
            Assert.AreEqual(3, sets[0].naturals);
            Assert.AreEqual(0, sets[0].wilds);
            Assert.Contains("D3", sets[0].cards);
            Assert.Contains("H3", sets[0].cards);
            Assert.Contains("C3", sets[0].cards);

            // test for bug where, after 1 set of 3 is played (all natural)
            // when playing an addition set of 3 with one wildcard, the wildcard
            // is added to the original set, leaving the a set of two
            cards = new List<string>()
                {"H2", "D5", "C5"};
            table.SplitIntoSets(cards, round1);
            sets = table.cardSets;
            Assert.AreEqual(2, table.setTypes.Count);
            Assert.Contains("3", table.setTypes);
            Assert.Contains("5", table.setTypes);

            Assert.AreEqual(2, sets.Count);
            Assert.AreEqual("3", sets[0].type);
            Assert.AreEqual(3, sets[0].GetSize());
            Assert.AreEqual(3, sets[0].naturals);
            Assert.AreEqual(0, sets[0].wilds);
            Assert.Contains("D3", sets[0].cards);
            Assert.Contains("H3", sets[0].cards);
            Assert.Contains("C3", sets[0].cards);

            Assert.AreEqual("5", sets[1].type);
            Assert.AreEqual(3, sets[1].GetSize());
            Assert.AreEqual(2, sets[1].naturals);
            Assert.AreEqual(1, sets[1].wilds);
            Assert.Contains("H2", sets[1].cards);
            Assert.Contains("D5", sets[1].cards);
            Assert.Contains("C5", sets[1].cards);

            // assert that single natural cards are disregarded
            table.setTypes = new List<string>();
            table.cardSets = new List<CardSet>();
            table.objectiveDone = false;
            cards = new List<string>()
                {"H2",
                "C4",
                "H7", "D7", "C7"};
            table.SplitIntoSets(cards, round1);
            sets = table.cardSets;
            Assert.AreEqual(1, table.setTypes.Count);
            Assert.Contains("7", table.setTypes);

            Assert.AreEqual(1, sets.Count);
            Assert.AreEqual("7", sets[0].type);
            Assert.AreEqual(4, sets[0].GetSize());
            Assert.AreEqual(3, sets[0].naturals);
            Assert.AreEqual(1, sets[0].wilds);
            Assert.Contains("H7", sets[0].cards);
            Assert.Contains("D7", sets[0].cards);
            Assert.Contains("C7", sets[0].cards);
            Assert.Contains("H2", sets[0].cards);
            
            // assert that after the objective is met, natural pairs are
            // disregarded if there are too few wild cards
            table.setTypes = new List<string>();
            table.cardSets = new List<CardSet>();
            table.objectiveDone = false;
            cards = new List<string>()
                {"H2",
                "C4", "S4",
                "C9", "D9",
                "H7", "D7", "C7"};
            table.SplitIntoSets(cards, round1);
            sets = table.cardSets;
            Assert.AreEqual(2, table.setTypes.Count);
            Assert.Contains("7", table.setTypes);
            Assert.Contains("9", table.setTypes);

            Assert.AreEqual(2, sets.Count);
            Assert.AreEqual("7", sets[0].type);
            Assert.AreEqual(3, sets[0].GetSize());
            Assert.AreEqual(3, sets[0].naturals);
            Assert.AreEqual(0, sets[0].wilds);
            Assert.Contains("H7", sets[0].cards);
            Assert.Contains("D7", sets[0].cards);
            Assert.Contains("C7", sets[0].cards);

            Assert.AreEqual("9", sets[1].type);
            Assert.AreEqual(3, sets[1].GetSize());
            Assert.AreEqual(2, sets[1].naturals);
            Assert.AreEqual(1, sets[1].wilds);
            Assert.Contains("H2", sets[1].cards);
            Assert.Contains("D9", sets[1].cards);
            Assert.Contains("C9", sets[1].cards);

            // assert that if objective cannot be met, all members are reset
            GameRound round5 = new GameRound(5, 1, 5);
            table.setTypes = new List<string>();
            table.cardSets = new List<CardSet>();
            table.objectiveDone = false;
            cards = new List<string>()
                {"H2",
                "C4", "S4",
                "C9", "D9",
                "H7", "D7", "C7"};
            table.SplitIntoSets(cards, round5);
            Assert.IsEmpty(table.setTypes);
            Assert.IsEmpty(table.cardSets);
            Assert.IsFalse(table.objectiveDone);

            // assert that objectiveDone is not set to true if all cards in the hand are different types
            cards = new List<string>()
                {"C4", "C9", "H7"};
            table.SplitIntoSets(cards, round5);
            Assert.IsEmpty(table.setTypes);
            Assert.IsEmpty(table.cardSets);
            Assert.IsFalse(table.objectiveDone);

            // assert that if objective isn't met, opponent cannot play a single card that
            // has been played by anyone at the table
            round5.UpdatePlayedTypes(new List<string>() {"11"});
            cards = new List<string>() {"H11"};
            table.SplitIntoSets(cards, round5);
            Assert.IsEmpty(table.setTypes);
            Assert.IsEmpty(table.cardSets);
            Assert.IsFalse(table.objectiveDone);

            // assert that if there are too many wild cards for the possible
            // sets, no error is thrown and the extra wild cards are left in hand
            cards = new List<string>()
                {"H2", "1JOKER", "D2", "C2",
                "H7", "D7", "C7"};
            table.SplitIntoSets(cards, round5);
            round5.UpdatePlayedTypes(new List<string>() {"7"});
            sets = table.cardSets;
            Assert.AreEqual(1, table.setTypes.Count);
            Assert.Contains("7", table.setTypes);

            Assert.AreEqual(1, sets.Count);
            Assert.AreEqual("7", sets[0].type);
            Assert.AreEqual(6, sets[0].GetSize());
            Assert.AreEqual(3, sets[0].naturals);
            Assert.AreEqual(3, sets[0].wilds);
            Assert.Contains("H7", sets[0].cards);
            Assert.Contains("D7", sets[0].cards);
            Assert.Contains("C7", sets[0].cards);
            Assert.Contains("H2", sets[0].cards);
            Assert.Contains("1JOKER", sets[0].cards);
            Assert.Contains("D2", sets[0].cards);
            Assert.IsFalse(sets[0].cards.Contains("C2"));

            // assert that if objective is already met, player can play a single card that
            // has been played by anyone at the table
            cards = new List<string>() {"H11"};
            table.SplitIntoSets(cards, round5);
            sets = table.cardSets;
            Assert.AreEqual(2, table.setTypes.Count);
            Assert.Contains("7", table.setTypes);
            Assert.Contains("11", table.setTypes);

            Assert.AreEqual(2, sets.Count);
            Assert.AreEqual("7", sets[0].type);
            Assert.AreEqual(6, sets[0].GetSize());
            Assert.AreEqual(3, sets[0].naturals);
            Assert.AreEqual(3, sets[0].wilds);
            Assert.Contains("H7", sets[0].cards);
            Assert.Contains("D7", sets[0].cards);
            Assert.Contains("C7", sets[0].cards);
            Assert.Contains("H2", sets[0].cards);
            Assert.Contains("1JOKER", sets[0].cards);
            Assert.Contains("D2", sets[0].cards);
            Assert.IsFalse(sets[0].cards.Contains("C2"));
            
            Assert.AreEqual("11", sets[1].type);
            Assert.AreEqual(1, sets[1].GetSize());
            Assert.AreEqual(1, sets[1].naturals);
            Assert.AreEqual(0, sets[1].wilds);
            Assert.Contains("H11", sets[1].cards);
        }
    }
}
