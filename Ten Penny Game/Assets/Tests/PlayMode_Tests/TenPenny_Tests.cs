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
