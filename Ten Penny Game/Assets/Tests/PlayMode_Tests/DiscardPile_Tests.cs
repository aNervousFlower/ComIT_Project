using System.Collections;
using System.Collections.Generic;
using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class NewTestScript
    {
        [Test]
        public void RemoveCardsForDeck_Test()
        {
            DiscardPile discards = new GameObject().AddComponent<DiscardPile>();
            discards.cardList.AddRange(new List<string>() {"1", "2", "3"} );

            // assert an error is thrown if there are less than 4 cards in the
            // discard pile
            InvalidOperationException e = Assert.Throws<InvalidOperationException>(
                () => discards.RemoveCardsForDeck());
            Assert.AreEqual(e.Message, "Discard Pile must contain " +
                "at least 4 cards to add them back to the deck. " +
                "Pile currently contains 3 cards.");
            
            // assert that the first six cards are returned and that
            // the last 3 remain in the discard pile
            discards.cardList.AddRange(new List<string>() {"4", "5", "6", "7", "8", "9"} );
            List<string> results = discards.RemoveCardsForDeck();
            Assert.AreEqual(3, discards.cardList.Count);
            Assert.AreEqual("7", discards.cardList[0]);
            Assert.AreEqual("8", discards.cardList[1]);
            Assert.AreEqual("9", discards.cardList[2]);
            
            Assert.AreEqual(6, results.Count);
            Assert.AreEqual("1", results[0]);
            Assert.AreEqual("2", results[1]);
            Assert.AreEqual("3", results[2]);
            Assert.AreEqual("4", results[3]);
            Assert.AreEqual("5", results[4]);
            Assert.AreEqual("6", results[5]);
        }
    }
}
