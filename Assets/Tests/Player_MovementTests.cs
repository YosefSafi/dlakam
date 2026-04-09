using UnityEngine;
using NUnit.Framework;
using StickmanBrainrot.Player;

namespace StickmanBrainrot.Tests
{
    public class Player_MovementTests
    {
        private GameObject playerObj;
        private Player_Controller playerController;

        [SetUp]
        public void Setup()
        {
            playerObj = new GameObject("Player");
            // Add necessary components for the test (PlayerInput can be mocked if needed)
            // But we can test the ChangeLane logic directly
            playerController = playerObj.AddComponent<Player_Controller>();
        }

        [TearDown]
        public void Teardown()
        {
            Object.DestroyImmediate(playerObj);
        }

        [Test]
        public void Player_InitialLane_IsMiddle()
        {
            Assert.AreEqual(0, playerController.CurrentLane);
        }

        [Test]
        public void Player_CanChangeLane_Right()
        {
            playerController.ChangeLane(1);
            Assert.AreEqual(1, playerController.CurrentLane);
        }

        [Test]
        public void Player_CanChangeLane_Left()
        {
            playerController.ChangeLane(-1);
            Assert.AreEqual(-1, playerController.CurrentLane);
        }

        [Test]
        public void Player_Lane_IsClamped_AtRight()
        {
            playerController.ChangeLane(1);
            playerController.ChangeLane(1); // Try to move past rightmost lane
            Assert.AreEqual(1, playerController.CurrentLane);
        }

        [Test]
        public void Player_Lane_IsClamped_AtLeft()
        {
            playerController.ChangeLane(-1);
            playerController.ChangeLane(-1); // Try to move past leftmost lane
            Assert.AreEqual(-1, playerController.CurrentLane);
        }
    }
}
