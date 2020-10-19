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
    }
}
