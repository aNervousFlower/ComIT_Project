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
    }
}
