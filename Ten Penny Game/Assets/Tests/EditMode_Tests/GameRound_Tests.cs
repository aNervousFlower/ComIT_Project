using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tests
{
    public class GameRound_Tests
    {
        [Test]
        public void SetText_Test()
        {
            GameObject gameOb = new GameObject();
            gameOb.name = "RoundInfo";
            gameOb.AddComponent<Text>();

            GameRound round1 = new GameRound(1, 1, 3);
            Assert.AreEqual(round1.roundInfoText.text,
                "Round 1\n1 set of 3");

            GameRound round4 = new GameRound(4, 2, 4);
            Assert.AreEqual(round1.roundInfoText.text,
                "Round 4\n2 sets of 4");
        }

        [Test]
        public void CanPlaySelectedCards_ObjectivesNotMet_Test()
        {
            GameObject gameOb = new GameObject();
            gameOb.name = "RoundInfo";
            gameOb.AddComponent<Text>();

            // Round 1 requires 1 set of 3
            List<string> selectedCards = new List<string>();
            GameRound round1 = new GameRound(1, 1, 3);

            // testing no cards selected
            Assert.AreEqual(round1.CanPlaySelectedCards(selectedCards,
                new List<string>(), 0, 0), false);

            // testing the initial disqualifiers
            // more wild cards than natural = false
            selectedCards.AddRange(new List<string>() {"S2", "1JOKER", "S3"});
            Assert.AreEqual(round1.CanPlaySelectedCards(selectedCards,
                new List<string>(), 0, 0), false);

            // a single natural card = false
            selectedCards.Clear();
            selectedCards.Add("S3");
            Assert.AreEqual(round1.CanPlaySelectedCards(selectedCards,
                new List<string>(), 0, 0), false);
            
            // 2 threes = false
            selectedCards.Add("C3");
            Assert.AreEqual(round1.CanPlaySelectedCards(selectedCards,
                new List<string>(), 0, 0), false);

            // 3 threes = true
            selectedCards.Add("D3");
            Assert.AreEqual(round1.CanPlaySelectedCards(selectedCards,
                new List<string>(), 0, 0), true);

            // 4 threes = true
            selectedCards.Add("C3");
            Assert.AreEqual(round1.CanPlaySelectedCards(selectedCards,
                new List<string>(), 0, 0), true);

            // 4 threes and 1 four = false
            selectedCards.Add("D4");
            Assert.AreEqual(round1.CanPlaySelectedCards(selectedCards,
                new List<string>(), 0, 0), false);
            
            // 1 wildCard (a deuce) and 2 fours = true
            selectedCards.Clear();
            selectedCards.AddRange(new List<string>() {"D4", "S4", "H2"});
            Assert.AreEqual(round1.CanPlaySelectedCards(selectedCards,
                new List<string>(), 0, 0), true);

            // 2 wildCards (a deuce and a joker) and 1 four = false
            selectedCards.Clear();
            selectedCards.AddRange(new List<string>() {"D4", "2JOKER", "H2"});
            Assert.AreEqual(round1.CanPlaySelectedCards(selectedCards,
                new List<string>(), 0, 0), false);

            // 2 wildCards (a deuce and a joker) and 2 fours = true
            selectedCards.Add("H4");
            Assert.AreEqual(round1.CanPlaySelectedCards(selectedCards,
                new List<string>(), 0, 0), true);

            // 3 fives and 2 fours = false
            selectedCards.Clear();
            selectedCards.AddRange(new List<string>() {"D4", "S4", "H5", "C5", "H5"});
            Assert.AreEqual(round1.CanPlaySelectedCards(selectedCards,
                new List<string>(), 0, 0), false);
        }
        
        [Test]
        public void CanPlaySelectedCards_ObjectivesMet_Test()
        {
            GameObject gameOb = new GameObject();
            gameOb.name = "RoundInfo";
            gameOb.AddComponent<Text>();

            // Round 1 requires 1 set of 3
            List<string> selectedCards = new List<string>();
            List<string> playedTypes = new List<string>();
            GameRound round1 = new GameRound(1, 1, 3);

            // already played 2 fours and 1 deuce, try selecting another four = true
            selectedCards.Add("H4");
            playedTypes.Add("4");
            Assert.IsTrue(round1.CanPlaySelectedCards(selectedCards, playedTypes, 1, 2));

            // already played 2 fours and 1 deuce, try selecting another deuce = true
            selectedCards.Clear();
            selectedCards.Add("H2");
            Assert.IsTrue(round1.CanPlaySelectedCards(selectedCards, playedTypes, 1, 2));

            // already played 2 fours and 1 deuce, try selecting another deuce and a five = false
            selectedCards.Add("C5");
            Assert.IsFalse(round1.CanPlaySelectedCards(selectedCards, playedTypes, 1, 2));

            // already played 2 fours and 1 deuce, try selecting another deuce and 2 fives = true
            selectedCards.Add("D5");
            Assert.IsTrue(round1.CanPlaySelectedCards(selectedCards, playedTypes, 1, 2));
        }
    }
}
