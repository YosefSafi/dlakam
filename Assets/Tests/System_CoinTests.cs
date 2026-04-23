using UnityEngine;
using NUnit.Framework;
using StickmanBrainrot.Systems;

namespace StickmanBrainrot.Tests
{
    public class System_CoinTests
    {
        private GameObject scoreManagerObj;
        private System_ScoreManager scoreManager;
        private GameObject coinObj;
        private System_Coin coin;

        [SetUp]
        public void Setup()
        {
            scoreManagerObj = new GameObject("_ScoreManager");
            scoreManager = scoreManagerObj.AddComponent<System_ScoreManager>();

            coinObj = new GameObject("TestCoin");
            coin = coinObj.AddComponent<System_Coin>();
        }

        [TearDown]
        public void Teardown()
        {
            Object.DestroyImmediate(scoreManagerObj);
            Object.DestroyImmediate(coinObj);
        }

        [Test]
        [TestCase(10, 1)]  // Score 10 -> Value 1
        [TestCase(45, 2)]  // Score 45 -> Value 2
        [TestCase(80, 3)]  // Score 80 -> Value 3
        [TestCase(150, 5)] // Score 150 -> Value 5
        public void CoinValue_ScalesWithScore(int currentScore, int expectedValue)
        {
            // Simulate score
            typeof(System_ScoreManager)
                .GetField("currentScore", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(scoreManager, (float)currentScore);

            // Using reflection to call the private GetCoinValue function to verify logic
            var method = typeof(System_Coin).GetMethod("GetCoinValue", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            int value = (int)method.Invoke(coin, null);

            Assert.AreEqual(expectedValue, value, $"Coin value should be {expectedValue} for score {currentScore}");
        }
    }
}
