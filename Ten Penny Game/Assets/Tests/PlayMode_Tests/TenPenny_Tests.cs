using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
// using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    public class TenPenny_Tests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void GenerateDeck_Test()
        {
            // Use the Assert class to test conditions
            string[] compareArray = new string[] {"1JOKER", "2JOKER",
                "C1", "C2", "C3", "C4", "C5", "C6", "C7",
                "C8", "C9", "C10", "C11", "C12", "C13",
                "D1", "D2", "D3", "D4", "D5", "D6", "D7",
                "D8", "D9", "D10", "D11", "D12", "D13",
                "H1", "H2", "H3", "H4", "H5", "H6", "H7",
                "H8", "H9", "H10", "H11", "H12", "H13",
                "S1", "S2", "S3", "S4", "S5", "S6", "S7",
                "S8", "S9", "S10", "S11", "S12", "S13"};
            // TODO: should test passing in invalid numbers,
            // but I still need to add validation and I'm not sure how to do it best
            
            // Test contents of single deck
            List<string> deck = new List<string>();
            deck = TenPenny.GenerateDeck();
            Assert.AreEqual(deck.Count, 54);
            int counter = 0;
            foreach (string card in compareArray)
            {
                Assert.AreEqual(card, deck[counter++]);
            }

            // Test contents of double deck
            List<string> doubleDeck = new List<string>();
            doubleDeck = TenPenny.GenerateDeck(2);
            Assert.AreEqual(doubleDeck.Count, 108);
            counter = 0;
            foreach (string card in compareArray)
            {
                Assert.AreEqual(card, doubleDeck[counter++]);
            }
            counter = 0;
            foreach (string card in compareArray)
            {
                Assert.AreEqual(card, doubleDeck[54 + counter++]);
            }

        }

        [Test]
        public void Shuffle_Test()
        {
            List<string> original = new List<string>();
            original.Add("one");
            original.Add("two");
            original.Add("three");
            original.Add("four");
            original.Add("five");

            List<string> testList = new List<string>();
            testList.Add("one");
            testList.Add("two");
            testList.Add("three");
            testList.Add("four");
            testList.Add("five");

            Assert.AreEqual(original, testList);

            // confirm that all the same members still exist in the List
            // but in a different order
            TenPenny.Shuffle(testList);
            Assert.AreEqual(testList.Count, 5);
            Assert.AreNotEqual(original, testList);
            foreach (string item in original)
            {
                Assert.Contains(item, testList);
            }
        }

        [Test]
        public void SortCards_Test()
        {
            List<string> cards = new List<string>();
            cards.Add("D3");
            cards.Add("C11");
            TenPenny.SortCards(cards);
            Assert.AreEqual(cards[0], "C11");
            Assert.AreEqual(cards[1], "D3");
            cards.Clear();
            
            cards.Add("D3");
            cards.Add("C1");
            TenPenny.SortCards(cards);
            Assert.AreEqual(cards[0], "C1");
            Assert.AreEqual(cards[1], "D3");
            cards.Clear();
            
            cards.Add("H4");
            cards.Add("D4");
            TenPenny.SortCards(cards);
            Assert.AreEqual(cards[0], "D4");
            Assert.AreEqual(cards[1], "H4");
            cards.Clear();
            
            cards.Add("1JOKER");
            cards.Add("H4");
            TenPenny.SortCards(cards);
            Assert.AreEqual(cards[0], "H4");
            Assert.AreEqual(cards[1], "1JOKER");
            cards.Clear();
            
            cards.Add("H1");
            cards.Add("S4");
            cards.Add("D2");
            cards.Add("S7");
            cards.Add("S3");
            cards.Add("C4");
            cards.Add("2JOKER");
            cards.Add("C5");
            cards.Add("H8");
            cards.Add("H10");
            cards.Add("H5");
            TenPenny.SortCards(cards);
            Assert.AreEqual(cards[0], "H1");
            Assert.AreEqual(cards[1], "H10");
            Assert.AreEqual(cards[2], "H8");
            Assert.AreEqual(cards[3], "S7");
            Assert.AreEqual(cards[4], "C5");
            Assert.AreEqual(cards[5], "H5");
            Assert.AreEqual(cards[6], "C4");
            Assert.AreEqual(cards[7], "S4");
            Assert.AreEqual(cards[8], "S3");
            Assert.AreEqual(cards[9], "D2");
            Assert.AreEqual(cards[10], "2JOKER");
            cards.Clear();
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        // [UnityTest]
        // public IEnumerator TenPenny_TestsWithEnumeratorPasses()
        // {
        //     // Use the Assert class to test conditions.
        //     // Use yield to skip a frame.
        //     yield return null;
        // }
    }
}
