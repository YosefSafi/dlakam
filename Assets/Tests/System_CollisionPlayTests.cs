using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using StickmanBrainrot.Systems;
using StickmanBrainrot.Player;

namespace StickmanBrainrot.Tests
{
    public class System_CollisionPlayTests
    {
        private GameObject gameManagerObj;
        private GameObject playerObj;
        private GameObject obstacleObj;

        [SetUp]
        public void Setup()
        {
            // Create GameManager
            gameManagerObj = new GameObject("_GameManager");
            gameManagerObj.AddComponent<System_GameManager>();
            gameManagerObj.AddComponent<System_ScoreManager>();

            // Create Player with Collision script
            playerObj = new GameObject("Player");
            playerObj.tag = "Player";
            playerObj.AddComponent<SphereCollider>().isTrigger = true;
            playerObj.AddComponent<Rigidbody>().isKinematic = true;
            playerObj.AddComponent<System_Collision>();
            playerObj.transform.position = Vector3.zero;

            // Create Obstacle
            obstacleObj = new GameObject("Obstacle");
            obstacleObj.tag = "Obstacle";
            obstacleObj.AddComponent<BoxCollider>().isTrigger = true;
            obstacleObj.transform.position = new Vector3(0, 0, 5); // 5 units ahead
        }

        [TearDown]
        public void Teardown()
        {
            Object.DestroyImmediate(gameManagerObj);
            Object.DestroyImmediate(playerObj);
            Object.DestroyImmediate(obstacleObj);
        }

        [UnityTest]
        public IEnumerator Player_HitsObstacle_TriggersGameOver()
        {
            // Verify initial state
            Assert.AreEqual(GameState.Playing, System_GameManager.Instance.CurrentState);

            // Move player into obstacle
            playerObj.transform.position = obstacleObj.transform.position;

            // Wait a frame for physics/trigger to process
            yield return new WaitForFixedUpdate();
            yield return null;

            // Verify Game Over state
            Assert.AreEqual(GameState.GameOver, System_GameManager.Instance.CurrentState, "Game state should be GameOver after hitting obstacle");
        }

        [UnityTest]
        public IEnumerator Player_HitsCoin_IncreasesScore()
        {
            // Setup Coin
            GameObject coinObj = new GameObject("Coin");
            coinObj.tag = "Coin";
            coinObj.AddComponent<SphereCollider>().isTrigger = true;
            coinObj.AddComponent<System_Coin>();
            coinObj.transform.position = new Vector3(0, 0, 2);

            int initialCoins = System_ScoreManager.Instance.TotalCoins;

            // Move player into coin
            playerObj.transform.position = coinObj.transform.position;

            // Wait for pickup
            yield return new WaitForFixedUpdate();
            yield return null;

            // Verify coin count increased
            Assert.Greater(System_ScoreManager.Instance.TotalCoins, initialCoins, "Coin count should increase after collection");
            
            // Cleanup coin (it should be destroyed by script, but just in case)
            if (coinObj != null) Object.DestroyImmediate(coinObj);
        }
    }
}
