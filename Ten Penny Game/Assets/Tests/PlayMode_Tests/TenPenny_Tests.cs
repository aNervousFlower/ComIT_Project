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
