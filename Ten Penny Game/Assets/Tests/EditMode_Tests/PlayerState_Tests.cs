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
            PlayerHand hand = new GameObject().AddComponent<PlayerHand>();

            PlayerState state = new PlayerState(hand);
            Assert.AreEqual(state.pennyCount, 10);
            Assert.AreEqual(state.pennyCountText.text, "10");

            Assert.IsTrue(state.SpendPenny());
            Assert.AreEqual(state.pennyCount, 9);
            Assert.AreEqual(state.pennyCountText.text, "9");
            
            state.pennyCount = 1;
            state.pennyCountText.text = "1";

            Assert.IsTrue(state.SpendPenny());
            Assert.AreEqual(state.pennyCount, 0);
            Assert.AreEqual(state.pennyCountText.text, "0");

            Assert.IsFalse(state.SpendPenny());
            Assert.AreEqual(state.pennyCount, 0);
            Assert.AreEqual(state.pennyCountText.text, "0");
        }
    }
}
