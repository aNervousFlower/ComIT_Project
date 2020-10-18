using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class CardSets_Tests
    {
        [Test]
        public void AddCard_Test()
        {
            CardSet set1 = new CardSet("5");
            Assert.AreEqual(set1.type, "5");
            Assert.AreEqual(set1.naturals, 0);
            Assert.AreEqual(set1.wilds, 0);
            Assert.AreEqual(set1.cards.Count, 0);

            // Natural card of the correct type CAN be added
            Assert.IsTrue(set1.AddCard("C5"));
            Assert.AreEqual(set1.naturals, 1);
            Assert.AreEqual(set1.wilds, 0);
            Assert.AreEqual(set1.cards.Count, 1);
            Assert.IsTrue(set1.cards.Contains("C5"));

            // Natural card of the wrong type CANNOT be added
            Assert.IsFalse(set1.AddCard("H6"));
            Assert.AreEqual(set1.naturals, 1);
            Assert.AreEqual(set1.wilds, 0);
            Assert.AreEqual(set1.cards.Count, 1);
            Assert.IsFalse(set1.cards.Contains("H6"));

            // Deuce (wildcard) CAN be added when there are less wildcard than natural
            Assert.IsTrue(set1.AddCard("S2"));
            Assert.AreEqual(set1.naturals, 1);
            Assert.AreEqual(set1.wilds, 1);
            Assert.AreEqual(set1.cards.Count, 2);
            Assert.IsTrue(set1.cards.Contains("C5"));
            Assert.IsTrue(set1.cards.Contains("S2"));

            // Joker (wildcard) CANNOT be added because the set cannot support more wildcards
            Assert.IsFalse(set1.AddCard("1JOKER"));
            Assert.AreEqual(set1.naturals, 1);
            Assert.AreEqual(set1.wilds, 1);
            Assert.AreEqual(set1.cards.Count, 2);
            Assert.IsFalse(set1.cards.Contains("1JOKER"));

            Assert.IsTrue(set1.AddCard("D5"));
            Assert.AreEqual(set1.naturals, 2);
            Assert.AreEqual(set1.wilds, 1);
            Assert.AreEqual(set1.cards.Count, 3);
            Assert.IsTrue(set1.cards.Contains("C5"));
            Assert.IsTrue(set1.cards.Contains("S2"));
            Assert.IsTrue(set1.cards.Contains("D5"));

            // Joker (wildcard) CAN be added when there are less wildcard than natural
            Assert.IsTrue(set1.AddCard("1JOKER"));
            Assert.AreEqual(set1.naturals, 2);
            Assert.AreEqual(set1.wilds, 2);
            Assert.AreEqual(set1.cards.Count, 4);
            Assert.IsTrue(set1.cards.Contains("C5"));
            Assert.IsTrue(set1.cards.Contains("S2"));
            Assert.IsTrue(set1.cards.Contains("D5"));
            Assert.IsTrue(set1.cards.Contains("1JOKER"));
        }
    }
}
