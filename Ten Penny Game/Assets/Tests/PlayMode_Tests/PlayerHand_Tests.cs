using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class PlayerHand_Tests
    {
        [Test]
        public void DealPlayerHand_Test()
        {
            PlayerHand hand = new GameObject().AddComponent<PlayerHand>();
            List<string> cardList = hand.cardList;
            hand.AddCard("D3");
            hand.AddCard("C11");
            hand.SortCards();
            Assert.AreEqual(cardList[0], "C11");
            Assert.AreEqual(cardList[1], "D3");
            cardList.RemoveRange(0, 2);
            
            hand.AddCard("D3");
            hand.AddCard("C1");
            hand.SortCards();
            Assert.AreEqual(cardList[0], "C1");
            Assert.AreEqual(cardList[1], "D3");
            cardList.RemoveRange(0, 2);
            
            hand.AddCard("H4");
            hand.AddCard("D4");
            hand.SortCards();
            Assert.AreEqual(cardList[0], "D4");
            Assert.AreEqual(cardList[1], "H4");
            cardList.RemoveRange(0, 2);
            
            hand.AddCard("1JOKER");
            hand.AddCard("H4");
            hand.SortCards();
            Assert.AreEqual(cardList[0], "H4");
            Assert.AreEqual(cardList[1], "1JOKER");
            cardList.RemoveRange(0, 2);
            
            hand.AddCard("H1");
            hand.AddCard("S4");
            hand.AddCard("D2");
            hand.AddCard("S7");
            hand.AddCard("S3");
            hand.AddCard("C4");
            hand.AddCard("2JOKER");
            hand.AddCard("C5");
            hand.AddCard("H8");
            hand.AddCard("H10");
            hand.AddCard("H5");
            hand.SortCards();
            Assert.AreEqual(cardList[0], "H1");
            Assert.AreEqual(cardList[1], "H10");
            Assert.AreEqual(cardList[2], "H8");
            Assert.AreEqual(cardList[3], "S7");
            Assert.AreEqual(cardList[4], "C5");
            Assert.AreEqual(cardList[5], "H5");
            Assert.AreEqual(cardList[6], "C4");
            Assert.AreEqual(cardList[7], "S4");
            Assert.AreEqual(cardList[8], "S3");
            Assert.AreEqual(cardList[9], "D2");
            Assert.AreEqual(cardList[10], "2JOKER");
        }

        [Test]
        public void SelectCard_Test()
        {
            PlayerHand hand = new GameObject().AddComponent<PlayerHand>();
            List<GameObject> selectedCards = hand.selectedCards;

            GameObject cardC1 = new GameObject();
            GameObject cardS4 = new GameObject();
            GameObject cardH11 = new GameObject();

            Assert.AreEqual(Color.yellow, hand.SelectCard(cardC1));
            Assert.AreEqual(selectedCards.Count, 1);
            Assert.AreEqual(selectedCards[0], cardC1);

            Assert.AreEqual(Color.yellow, hand.SelectCard(cardS4));
            Assert.AreEqual(selectedCards.Count, 2);
            Assert.AreEqual(selectedCards[0], cardC1);
            Assert.AreEqual(selectedCards[1], cardS4);

            Assert.AreEqual(Color.white, hand.SelectCard(cardC1));
            Assert.AreEqual(selectedCards.Count, 1);
            Assert.AreEqual(selectedCards[0], cardS4);

            Assert.AreEqual(Color.yellow, hand.SelectCard(cardH11));
            Assert.AreEqual(selectedCards.Count, 2);
            Assert.AreEqual(selectedCards[0], cardS4);
            Assert.AreEqual(selectedCards[1], cardH11);

            Assert.AreEqual(Color.yellow, hand.SelectCard(cardC1));
            Assert.AreEqual(selectedCards.Count, 3);
            Assert.AreEqual(selectedCards[0], cardS4);
            Assert.AreEqual(selectedCards[1], cardH11);
            Assert.AreEqual(selectedCards[2], cardC1);

            Assert.AreEqual(Color.white, hand.SelectCard(cardH11));
            Assert.AreEqual(selectedCards.Count, 2);
            Assert.AreEqual(selectedCards[0], cardS4);
            Assert.AreEqual(selectedCards[1], cardC1);
        }
    }
}
