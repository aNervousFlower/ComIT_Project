using System.Collections;
using System.Collections.Generic;
using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;

namespace Tests
{
    public class PlayerTable_Tests
    {
        [Test]
        public void SplitIntoSets_Test()
        {
            PlayerTable table = new GameObject().AddComponent<PlayerTable>();
            GameObject gameOb = new GameObject();
            gameOb.name = "RoundInfo";
            gameOb.AddComponent<Text>();
            GameRound round1 = new GameRound(1, 1, 3);

            List<string> cards = new List<string>() {"D3", "H3", "C3"};
            table.SplitIntoSets(cards, round1);
            List<CardSet> sets = table.cardSets;
            Assert.AreEqual(table.setTypes.Count, 1);
            Assert.Contains("3", table.setTypes);

            Assert.AreEqual(sets.Count, 1);
            Assert.AreEqual(sets[0].type, "3");
            Assert.AreEqual(sets[0].GetSize(), 3);
            Assert.AreEqual(sets[0].naturals, 3);
            Assert.AreEqual(sets[0].wilds, 0);
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
            Assert.AreEqual(table.setTypes.Count, 2);
            Assert.Contains("3", table.setTypes);
            Assert.Contains("5", table.setTypes);

            Assert.AreEqual(sets.Count, 2);
            Assert.AreEqual(sets[0].type, "3");
            Assert.AreEqual(sets[0].GetSize(), 3);
            Assert.AreEqual(sets[0].naturals, 3);
            Assert.AreEqual(sets[0].wilds, 0);
            Assert.Contains("D3", sets[0].cards);
            Assert.Contains("H3", sets[0].cards);
            Assert.Contains("C3", sets[0].cards);

            Assert.AreEqual(sets[1].type, "5");
            Assert.AreEqual(sets[1].GetSize(), 3);
            Assert.AreEqual(sets[1].naturals, 2);
            Assert.AreEqual(sets[1].wilds, 1);
            Assert.Contains("H2", sets[1].cards);
            Assert.Contains("D5", sets[1].cards);
            Assert.Contains("C5", sets[1].cards);

            // InvalidOperationException e;
            // assert that an error is thrown if too many wildcards are in the selection
            table.setTypes = new List<string>();
            table.cardSets = new List<CardSet>();
            table.objectiveDone = false;
            cards = new List<string>() 
                {"H2", "D2", "2JOKER",
                "D3", "H3"};
            InvalidOperationException e = Assert.Throws<InvalidOperationException>(
                () => table.SplitIntoSets(cards, round1));
            Assert.AreEqual(e.Message, "Invalid Card Selection: too many wildcards");

            // assert that sets are sorted correctly so that the deuce (wildcard)
            // is added to the set of sevens instead of the set of threes
            GameRound round5 = new GameRound(5, 1, 5);
            table.setTypes = new List<string>();
            table.cardSets = new List<CardSet>();
            table.objectiveDone = false;
            cards = new List<string>()
                {"H2",
                "D3", "H3", "C3",
                "H7", "D7", "C7", "S7"};
            table.SplitIntoSets(cards, round5);
            sets = table.cardSets;
            Assert.AreEqual(table.setTypes.Count, 2);
            Assert.Contains("3", table.setTypes);
            Assert.Contains("7", table.setTypes);

            Assert.AreEqual(sets.Count, 2);
            Assert.AreEqual(sets[0].type, "7");
            Assert.AreEqual(sets[0].GetSize(), 5);
            Assert.AreEqual(sets[0].naturals, 4);
            Assert.AreEqual(sets[0].wilds, 1);
            Assert.Contains("H7", sets[0].cards);
            Assert.Contains("D7", sets[0].cards);
            Assert.Contains("C7", sets[0].cards);
            Assert.Contains("S7", sets[0].cards);
            Assert.Contains("H2", sets[0].cards);

            Assert.AreEqual(sets[1].type, "3");
            Assert.AreEqual(sets[1].GetSize(), 3);
            Assert.AreEqual(sets[1].naturals, 3);
            Assert.AreEqual(sets[1].wilds, 0);
            Assert.Contains("D3", sets[1].cards);
            Assert.Contains("H3", sets[1].cards);
            Assert.Contains("C3", sets[1].cards);
            
            // assert that wildcards are distributed as intended, in this case:
                // the first is attached to the set of sevens to ensure the round requirements
                // the second is attached to the set of threes to ensure the set contains at least 3 cards
                // the next three are attached to the set of sevens, until the number of wildcards matches the number of naturals
                // the last defaults to the set of sixes
            table.setTypes = new List<string>();
            table.cardSets = new List<CardSet>();
            table.objectiveDone = false;
            cards = new List<string>()
                {"H2", "D2", "C2", "S2", "1JOKER", "2JOKER",
                "D6", "H6", "C6",
                "D3", "H3",
                "H7", "D7", "C7", "S7"};
            table.SplitIntoSets(cards, round5);
            sets = table.cardSets;
            Assert.AreEqual(table.setTypes.Count, 3);
            Assert.Contains("3", table.setTypes);
            Assert.Contains("6", table.setTypes);
            Assert.Contains("7", table.setTypes);

            Assert.AreEqual(sets.Count, 3);
            Assert.AreEqual(sets[0].type, "7");
            Assert.AreEqual(sets[0].GetSize(), 8);
            Assert.AreEqual(sets[0].naturals, 4);
            Assert.AreEqual(sets[0].wilds, 4);
            Assert.Contains("H7", sets[0].cards);
            Assert.Contains("D7", sets[0].cards);
            Assert.Contains("C7", sets[0].cards);
            Assert.Contains("S7", sets[0].cards);
            Assert.Contains("H2", sets[0].cards);
            Assert.Contains("C2", sets[0].cards);
            Assert.Contains("S2", sets[0].cards);
            Assert.Contains("1JOKER", sets[0].cards);
            
            Assert.AreEqual(sets[1].type, "6");
            Assert.AreEqual(sets[1].GetSize(), 4);
            Assert.AreEqual(sets[1].naturals, 3);
            Assert.AreEqual(sets[1].wilds, 1);
            Assert.Contains("D6", sets[1].cards);
            Assert.Contains("H6", sets[1].cards);
            Assert.Contains("C6", sets[1].cards);
            Assert.Contains("2JOKER", sets[1].cards);
            
            Assert.AreEqual(sets[2].type, "3");
            Assert.AreEqual(sets[2].GetSize(), 3);
            Assert.AreEqual(sets[2].naturals, 2);
            Assert.AreEqual(sets[2].wilds, 1);
            Assert.Contains("D3", sets[2].cards);
            Assert.Contains("H3", sets[2].cards);
            Assert.Contains("D2", sets[2].cards);
            
            // assert that after the round objectives are met, it doesn't require
            // objective sets to be a valid selection 
            Assert.IsTrue(table.objectiveDone);
            cards = new List<string>()
                {"D9", "H9", "C9"};
            table.SplitIntoSets(cards, round5);
            sets = table.cardSets;
            Assert.AreEqual(table.setTypes.Count, 4);
            Assert.Contains("3", table.setTypes);
            Assert.Contains("6", table.setTypes);
            Assert.Contains("7", table.setTypes);
            Assert.Contains("9", table.setTypes);
            Assert.AreEqual(table.cardSets.Count, 4);
            
            Assert.AreEqual(sets[3].type, "9");
            Assert.AreEqual(sets[3].GetSize(), 3);
            
            // assert that after the round objectives are met,
            // cards are just added to existing sets if possible
            Assert.IsTrue(table.objectiveDone);
            cards = new List<string>()
                {"S6", "S3", "C3", "1JOKER"};
            table.SplitIntoSets(cards, round5);
            sets = table.cardSets;
            Assert.AreEqual(table.setTypes.Count, 4);
            Assert.Contains("3", table.setTypes);
            Assert.Contains("6", table.setTypes);
            Assert.Contains("7", table.setTypes);
            Assert.Contains("9", table.setTypes);
            Assert.AreEqual(table.cardSets.Count, 4);

            Assert.AreEqual(sets[0].type, "7");
            Assert.AreEqual(sets[0].GetSize(), 8);
            Assert.AreEqual(sets[0].naturals, 4);
            Assert.AreEqual(sets[0].wilds, 4);
            
            Assert.AreEqual(sets[1].type, "6");
            Assert.AreEqual(sets[1].GetSize(), 6);
            Assert.AreEqual(sets[1].naturals, 4);
            Assert.AreEqual(sets[1].wilds, 2);
            Assert.Contains("D6", sets[1].cards);
            Assert.Contains("H6", sets[1].cards);
            Assert.Contains("C6", sets[1].cards);
            Assert.Contains("S6", sets[1].cards);
            Assert.Contains("2JOKER", sets[1].cards);
            Assert.Contains("1JOKER", sets[1].cards);
            
            Assert.AreEqual(sets[2].type, "3");
            Assert.AreEqual(sets[2].GetSize(), 5);
            Assert.AreEqual(sets[2].naturals, 4);
            Assert.AreEqual(sets[2].wilds, 1);
            Assert.Contains("D3", sets[2].cards);
            Assert.Contains("H3", sets[2].cards);
            Assert.Contains("S3", sets[2].cards);
            Assert.Contains("C3", sets[2].cards);
            Assert.Contains("D2", sets[2].cards);
            
            Assert.AreEqual(sets[3].type, "9");
            Assert.AreEqual(sets[3].GetSize(), 3);
            Assert.AreEqual(sets[3].naturals, 3);
            Assert.AreEqual(sets[3].wilds, 0);

            // assert that an error is thrown if too few wildcards are in the selection
            table.setTypes = new List<string>();
            table.cardSets = new List<CardSet>();
            table.objectiveDone = false;
            cards = new List<string>() 
                {"H2",
                "H7", "D7", "C7"};
            e = Assert.Throws<InvalidOperationException>(
                () => table.SplitIntoSets(cards, round5));
            Assert.AreEqual(e.Message, "Invalid Card Selection: too few wildcards: H2, H7, D7, C7");
            
            // assert that an error is thrown if there are too few natural cards to reach the required sets for the round
            table.setTypes = new List<string>();
            table.cardSets = new List<CardSet>();
            table.objectiveDone = false;
            cards = new List<string>() 
                {"H2", "D2", "1JOKER",
                "H7", "D7"};
            e = Assert.Throws<InvalidOperationException>(
                () => table.SplitIntoSets(cards, round5));
            Assert.AreEqual(e.Message, "Invalied Card Selection: too few natural cards: D2, H2, H7, D7");
            
            // assert that an error is thrown if there are too few natural cards to reach a minimum set size of three (ie: any single cards)
            table.setTypes = new List<string>();
            table.cardSets = new List<CardSet>();
            table.objectiveDone = false;
            cards = new List<string>() 
                {"H2", "D2", "C2",
                "S3",
                "H7", "D7", "C7", "S7"};
            e = Assert.Throws<InvalidOperationException>(
                () => table.SplitIntoSets(cards, round5));
            Assert.AreEqual(e.Message, "Invalied Card Selection: too few natural cards: D2, S3");
        }
    }
}
