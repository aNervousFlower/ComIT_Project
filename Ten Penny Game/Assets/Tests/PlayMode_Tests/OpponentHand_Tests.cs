using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class OpponentHand_Tests
    {
        [Test]
        public void DiscardCard_Test()
        {
            OpponentHand hand = new GameObject().AddComponent<OpponentHand>();
            List<string> cardList = hand.cardList;

            hand.AddCard("D3");
            hand.AddCard("H4");
            hand.AddCard("C4");
            Assert.AreEqual("D3", hand.DiscardCard());
            Assert.IsFalse(cardList.Contains("D3"));
            
            hand.AddCard("S5");
            Assert.AreEqual("S5", hand.DiscardCard());
            Assert.IsFalse(cardList.Contains("S5"));

            hand.AddCard("1JOKER");
            Assert.AreEqual("H4", hand.DiscardCard());
            Assert.IsFalse(cardList.Contains("H4"));

            hand.AddCard("S13");
            hand.AddCard("H3");
            hand.AddCard("S4");
            hand.AddCard("D4");
            Assert.AreEqual("H3", hand.DiscardCard());
            Assert.IsFalse(cardList.Contains("H3"));

            hand.AddCard("D13");
            Assert.AreEqual("S13", hand.DiscardCard());
            Assert.IsFalse(cardList.Contains("S13"));
        }
    }
}
