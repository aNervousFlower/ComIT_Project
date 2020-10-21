using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tests
{
    public class PlayerState_Tests
    {
        [Test]
        public void SpendPenny_Test()
        {
            GameObject gameOb = new GameObject();
            gameOb.name = "PlayerPenniesCount";
            gameOb.AddComponent<Text>();
            GameObject gameOb2 = new GameObject();
            gameOb2.name = "playerActionsInfoText";
            gameOb2.AddComponent<Text>();
            GameObject gameOb3 = new GameObject();
            gameOb3.name = "PlayerScoreText";
            gameOb3.AddComponent<Text>();
            PlayerHand hand = new GameObject().AddComponent<PlayerHand>();

            PlayerState state = new PlayerState(hand);
            Assert.AreEqual(10, state.pennyCount);
            Assert.AreEqual("10", state.pennyCountText.text);

            Assert.IsTrue(state.SpendPenny());
            Assert.AreEqual(9, state.pennyCount);
            Assert.AreEqual("9", state.pennyCountText.text);
            
            state.pennyCount = 1;
            state.pennyCountText.text = "1";

            Assert.IsTrue(state.SpendPenny());
            Assert.AreEqual(0, state.pennyCount);
            Assert.AreEqual("0", state.pennyCountText.text);

            Assert.IsFalse(state.SpendPenny());
            Assert.AreEqual(0, state.pennyCount);
            Assert.AreEqual("0", state.pennyCountText.text);
        }

        [Test]
        public void TurnStructure_Test()
        {
            GameObject gameOb = new GameObject();
            gameOb.name = "PlayerPenniesCount";
            gameOb.AddComponent<Text>();
            GameObject gameOb2 = new GameObject();
            gameOb2.name = "playerActionsInfoText";
            gameOb2.AddComponent<Text>();
            GameObject gameOb3 = new GameObject();
            gameOb3.name = "PlayerScoreText";
            gameOb3.AddComponent<Text>();
            PlayerHand hand = new GameObject().AddComponent<PlayerHand>();

            PlayerState state = new PlayerState(hand);
            Assert.AreEqual(2, state.playerActions.Count);
            Assert.Contains("Draw", state.playerActions);
            Assert.Contains("Buy", state.playerActions);
            Assert.AreEqual("Draw or Buy", state.playerActionsInfoText.text);
            Assert.AreEqual(10, state.pennyCount);

            // tests result of Play returning false
            Assert.IsFalse(state.Play());
            Assert.AreEqual(2, state.playerActions.Count);
            Assert.Contains("Draw", state.playerActions);
            Assert.Contains("Buy", state.playerActions);
            Assert.AreEqual("Draw or Buy", state.playerActionsInfoText.text);

            // tests result of Discard returning false
            Assert.IsFalse(state.Discard());
            Assert.AreEqual(2, state.playerActions.Count);
            Assert.Contains("Draw", state.playerActions);
            Assert.Contains("Buy", state.playerActions);
            Assert.AreEqual("Draw or Buy", state.playerActionsInfoText.text);

            // tests result of Draw returning true
            Assert.IsTrue(state.Draw());
            Assert.AreEqual(2, state.playerActions.Count);
            Assert.Contains("Play", state.playerActions);
            Assert.Contains("Discard", state.playerActions);
            Assert.AreEqual("Play or Discard", state.playerActionsInfoText.text);

            // tests result of Draw returning false
            Assert.IsFalse(state.Draw());
            Assert.AreEqual(2, state.playerActions.Count);
            Assert.Contains("Play", state.playerActions);
            Assert.Contains("Discard", state.playerActions);
            Assert.AreEqual("Play or Discard", state.playerActionsInfoText.text);

            // tests result of Buy returning false
            Assert.IsFalse(state.Buy());
            Assert.AreEqual(2, state.playerActions.Count);
            Assert.Contains("Play", state.playerActions);
            Assert.Contains("Discard", state.playerActions);
            Assert.AreEqual("Play or Discard", state.playerActionsInfoText.text);

            // tests result of Discard returning true (with pennies > 0)
            Assert.IsTrue(state.Discard());
            Assert.AreEqual(2, state.playerActions.Count);
            Assert.Contains("Buy", state.playerActions);
            Assert.Contains("Draw", state.playerActions);
            Assert.AreEqual("Draw or Buy", state.playerActionsInfoText.text);

            // tests result of Buy returning true (with pennies > 0)
            Assert.IsTrue(state.Buy());
            Assert.AreEqual(3, state.playerActions.Count);
            Assert.Contains("Buy", state.playerActions);
            Assert.Contains("Play", state.playerActions);
            Assert.Contains("Discard", state.playerActions);
            Assert.AreEqual("Buy, Play or Discard", state.playerActionsInfoText.text);

            // tests result of Play returning true
            Assert.IsTrue(state.Play());
            Assert.AreEqual(2, state.playerActions.Count);
            Assert.Contains("Play", state.playerActions);
            Assert.Contains("Discard", state.playerActions);
            Assert.AreEqual("Play or Discard", state.playerActionsInfoText.text);

            state.Discard();

            state.pennyCount = 1;
            // tests result of Buy returning true (with pennies == 1)
            Assert.IsTrue(state.Buy());
            Assert.AreEqual(2, state.playerActions.Count);
            Assert.Contains("Play", state.playerActions);
            Assert.Contains("Discard", state.playerActions);
            Assert.AreEqual("Play or Discard", state.playerActionsInfoText.text);

            state.pennyCount = 0;
            // tests result of Discard returning true (with pennies == 0)
            Assert.IsTrue(state.Discard());
            Assert.AreEqual(1, state.playerActions.Count);
            Assert.Contains("Draw", state.playerActions);
            Assert.AreEqual("Draw", state.playerActionsInfoText.text);
        }

        [Test]
        public void GetCardPoints_Test()
        {
            Assert.AreEqual(50, PlayerState.GetCardPoints("2JOKER"));
            Assert.AreEqual(20, PlayerState.GetCardPoints("C1"));
            Assert.AreEqual(20, PlayerState.GetCardPoints("H2"));
            Assert.AreEqual(10, PlayerState.GetCardPoints("D13"));
            Assert.AreEqual(10, PlayerState.GetCardPoints("S10"));
            Assert.AreEqual(5, PlayerState.GetCardPoints("D9"));
            Assert.AreEqual(5, PlayerState.GetCardPoints("H3"));
        }

        [Test]
        public void TallyPlayerScore_Test()
        {
            GameObject gameOb = new GameObject();
            gameOb.name = "PlayerPenniesCount";
            gameOb.AddComponent<Text>();
            GameObject gameOb2 = new GameObject();
            gameOb2.name = "playerActionsInfoText";
            gameOb2.AddComponent<Text>();
            GameObject gameOb3 = new GameObject();
            gameOb3.name = "PlayerScoreText";
            gameOb3.AddComponent<Text>();
            PlayerHand hand = new GameObject().AddComponent<PlayerHand>();
            PlayerTable table = new GameObject().AddComponent<PlayerTable>();
            hand.playerTable = table;
            PlayerState state = new PlayerState(hand);

            // assert score starts at 0
            Assert.AreEqual(0, state.playerScore);

            // assert score is calculated correctly
            state.playerTable.cardList.AddRange(new List<string>() {"C2", "D1", "S11", "1JOKER"});
            hand.cardList.AddRange(new List<string>() { "H5", "D13" });
            state.TallyPlayerScore();
            Assert.AreEqual(85, state.playerScore);
            Assert.AreEqual("Score\n85", state.playerScoreText.text);

            // assert subsequent runs of the method add to the previous score
            state.playerTable.cardList.Clear();
            hand.cardList.Clear();
            state.playerTable.cardList.AddRange(new List<string>() {"D5", "H5", "S5", "D2"});
            hand.cardList.AddRange(new List<string>() { "H1" });
            state.TallyPlayerScore();
            Assert.AreEqual(100, state.playerScore);
            Assert.AreEqual("Score\n100", state.playerScoreText.text);

            // assert score can be negative
            state.playerTable.cardList.Clear();
            hand.cardList.Clear();
            state.playerTable.cardList.AddRange(new List<string>() {"H4"});
            hand.cardList.AddRange(new List<string>() { "1JOKER", "2JOKER", "S1"});
            state.TallyPlayerScore();
            Assert.AreEqual(-15, state.playerScore);
            Assert.AreEqual("Score\n-15", state.playerScoreText.text);
        }
    }
}
