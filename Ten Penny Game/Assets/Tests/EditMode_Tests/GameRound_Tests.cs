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
            Assert.AreEqual(round1.CanPlaySelectedCards(selectedCards, 0, 0), false);

            // testing the initial disqualifiers
            // more wild cards than natural = false
            selectedCards.AddRange(new List<string>() {"S2", "1JOKER", "S3"});
            Assert.AreEqual(round1.CanPlaySelectedCards(selectedCards, 0, 0), false);

            // a single natural card = false
            selectedCards.Clear();
            selectedCards.Add("S3");
            Assert.AreEqual(round1.CanPlaySelectedCards(selectedCards, 0, 0), false);
            
            // 2 threes = false
            selectedCards.Add("C3");
            Assert.AreEqual(round1.CanPlaySelectedCards(selectedCards, 0, 0), false);

            // 3 threes = true
            selectedCards.Add("D3");
            Assert.AreEqual(round1.CanPlaySelectedCards(selectedCards, 0, 0), true);

            // 4 threes = true
            selectedCards.Add("C3");
            Assert.AreEqual(round1.CanPlaySelectedCards(selectedCards, 0, 0), true);

            // 4 threes and 1 four = false
            selectedCards.Add("D4");
            Assert.AreEqual(round1.CanPlaySelectedCards(selectedCards, 0, 0), false);
            
            // 1 wildCard (a deuce) and 2 fours = true
            selectedCards.Clear();
            selectedCards.AddRange(new List<string>() {"D4", "S4", "H2"});
            Assert.AreEqual(round1.CanPlaySelectedCards(selectedCards, 0, 0), true);

            // 2 wildCards (a deuce and a joker) and 1 four = false
            selectedCards.Clear();
            selectedCards.AddRange(new List<string>() {"D4", "2JOKER", "H2"});
            Assert.AreEqual(round1.CanPlaySelectedCards(selectedCards, 0, 0), false);

            // 2 wildCards (a deuce and a joker) and 2 fours = true
            selectedCards.Add("H4");
            Assert.AreEqual(round1.CanPlaySelectedCards(selectedCards, 0, 0), true);

            // 3 fives and 2 fours = false
            selectedCards.Clear();
            selectedCards.AddRange(new List<string>() {"D4", "S4", "H5", "C5", "H5"});
            Assert.AreEqual(round1.CanPlaySelectedCards(selectedCards, 0, 0), false);
        }
        
        [Test]
        public void CanPlaySelectedCards_ObjectivesMet_Test()
        {
            GameObject gameOb = new GameObject();
            gameOb.name = "RoundInfo";
            gameOb.AddComponent<Text>();

            // Round 1 requires 1 set of 3
            List<string> selectedCards = new List<string>();
            List<string> types = new List<string>();
            GameRound round1 = new GameRound(1, 1, 3);

            // no selected cards should return false so that button stays uninteractable
            types.Add("4");
            round1.UpdatePlayedTypes(types);
            Assert.IsFalse(round1.CanPlaySelectedCards(selectedCards, 1, 2));

            // already played 2 fours and 1 deuce, try selecting another four = true
            selectedCards.Add("H4");
            Assert.IsTrue(round1.CanPlaySelectedCards(selectedCards, 1, 2));

            // already played 2 fours and 1 deuce, try selecting another deuce = true
            selectedCards.Clear();
            selectedCards.Add("H2");
            Assert.IsTrue(round1.CanPlaySelectedCards(selectedCards, 1, 2));

            // already played 2 fours and 1 deuce, try selecting another deuce and a five = false
            selectedCards.Add("C5");
            Assert.IsFalse(round1.CanPlaySelectedCards(selectedCards, 1, 2));

            // already played 2 fours and 1 deuce, try selecting another deuce and 2 fives = true
            selectedCards.Add("D5");
            Assert.IsTrue(round1.CanPlaySelectedCards(selectedCards, 1, 2));

            // Round 2 requires 2 set of 3
            selectedCards = new List<string>();
            types = new List<string>();
            GameRound round2 = new GameRound(2, 2, 3);

            // already played 3 Kings and 4 sevens, try selecting 3 nines = true
            selectedCards.AddRange(new List<string>() {"D9", "S9", "H9"});
            types.AddRange(new List<string>() {"13", "7"});
            round2.UpdatePlayedTypes(types);
            Assert.IsTrue(round2.CanPlaySelectedCards(selectedCards, 0, 7));

            // Round 8 requires 2 set of 6
            selectedCards = new List<string>();
            types = new List<string>();
            GameRound round8 = new GameRound(2, 2, 3);

            // already played 2 sets of 6, try selecting 4 tens, 3 fives, and 2 threes = false
            selectedCards.AddRange(new List<string>()
                {"C10", "C10", "H10", "S10",
                "H5", "C5", "D5",
                "D3", "H3"});
            types.AddRange(new List<string>() {"12", "8"});
            round8.UpdatePlayedTypes(types);
            Assert.IsFalse(round8.CanPlaySelectedCards(selectedCards, 3, 9));
        }

        [Test]
        public void UpdatePlayedTypes_Test()
        {
            GameObject gameOb = new GameObject();
            gameOb.name = "RoundInfo";
            gameOb.AddComponent<Text>();

            GameRound round1 = new GameRound(1, 1, 3);
            Assert.IsEmpty(round1.playedTypes);

            List<string> types = new List<string> {"3", "7"};
            round1.UpdatePlayedTypes(types);
            Assert.AreEqual(2, round1.playedTypes.Count);
            Assert.Contains("3", round1.playedTypes);
            Assert.Contains("7", round1.playedTypes);

            types = new List<string> {"3", "5", "7"};
            round1.UpdatePlayedTypes(types);
            Assert.AreEqual(3, round1.playedTypes.Count);
            Assert.Contains("3", round1.playedTypes);
            Assert.Contains("7", round1.playedTypes);
            Assert.Contains("5", round1.playedTypes);
        }
    }
}
