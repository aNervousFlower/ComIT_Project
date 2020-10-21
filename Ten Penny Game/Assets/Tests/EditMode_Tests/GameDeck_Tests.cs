using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class GameDeck_Tests
    {
        [Test]
        public void suits_Test()
        {
            string[] compareArray = new string[] { "C", "D", "H", "S"};
            Assert.AreEqual(compareArray, GameDeck.suits);
        }

        [Test]
        public void Constructor_Test()
        {
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
            GameDeck singleDeck = new GameDeck();
            Assert.AreEqual(singleDeck.cardList.Count, 54);
            int counter = 0;
            foreach (string card in compareArray)
            {
                Assert.AreEqual(card, singleDeck.cardList[counter++]);
            }

            // Test contents of double deck
            GameDeck doubleDeck = new GameDeck(2);
            Assert.AreEqual(doubleDeck.cardList.Count, 108);
            counter = 0;
            foreach (string card in compareArray)
            {
                Assert.AreEqual(card, doubleDeck.cardList[counter++]);
            }
            counter = 0;
            foreach (string card in compareArray)
            {
                Assert.AreEqual(card, doubleDeck.cardList[54 + counter++]);
            }
        }

        [Test]
        public void Shuffle_Test()
        {
            GameDeck originalDeck = new GameDeck();
            GameDeck testDeck = new GameDeck();
            Assert.AreEqual(originalDeck.cardList, testDeck.cardList);

            // confirm that all the same members still exist in the List
            // but in a different order
            GameDeck.Shuffle(testDeck.cardList);
            Assert.AreEqual(testDeck.cardList.Count, 54);
            Assert.AreNotEqual(originalDeck.cardList, testDeck.cardList);
            foreach (string item in originalDeck.cardList)
            {
                Assert.Contains(item, testDeck.cardList);
            }
        }

        [Test]
        public void DrawCard_Test()
        {
            GameDeck gameDeck = new GameDeck(1);
            Assert.AreEqual(gameDeck.cardList.Count, 54);

            // confirm that with each call of DrawCard, the last card is removed
            string topCard = gameDeck.DrawCard();
            Assert.AreEqual(gameDeck.cardList.Count, 53);
            Assert.AreEqual(topCard, "S13");

            topCard = gameDeck.DrawCard();
            Assert.AreEqual(gameDeck.cardList.Count, 52);
            Assert.AreEqual(topCard, "S12");
        }

        [Test]
        public void ReplenishDeck_Test()
        {
            GameDeck gameDeck = new GameDeck(1);
            gameDeck.cardList.Clear();
            gameDeck.cardList.AddRange(new List<string>() {"A", "B", "C"} );
            gameDeck.ReplenishDeck(new List<string>() {"1", "2", "3", "4"} );

            // assert that the initial cards in cardList remain in the same order
            // at the top of the list after the new cards are added
            Assert.AreEqual("A", gameDeck.cardList[4]);
            Assert.AreEqual("B", gameDeck.cardList[5]);
            Assert.AreEqual("C", gameDeck.cardList[6]);

            // assert that the newCards added to the bottom deck were shuffled
            bool inOrder = (gameDeck.cardList[0] == "1" && gameDeck.cardList[1] == "2" &&
                gameDeck.cardList[2] == "3" && gameDeck.cardList[3] == "4");
        }
    }
}
