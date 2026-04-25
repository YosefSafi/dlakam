using UnityEngine;
using StickmanBrainrot.Player;

namespace StickmanBrainrot.Events
{
    /// <summary>
    /// Event that drastically speeds up the player temporarily.
    /// </summary>
    public class Event_SpeedBurst : Event_Base
    {
        [Header("Burst Settings")]
        [SerializeField] private float speedMultiplier = 2.5f;
        
        private Player_Controller playerController;

        private void Awake()
        {
            eventName = "SPEED BURST";
            playerController = Object.FindAnyObjectByType<Player_Controller>();
        }

        protected override void OnEventStart()
        {
            if (playerController == null) playerController = Object.FindAnyObjectByType<Player_Controller>();
            
            if (playerController != null)
            {
                playerController.ModifySpeed(speedMultiplier);
            }
        }

        protected override void OnEventEnd()
        {
            if (playerController != null)
            {
                playerController.ResetSpeed();
            }
        }
    }
}
