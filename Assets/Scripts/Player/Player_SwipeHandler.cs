using UnityEngine;
using UnityEngine.InputSystem;

namespace StickmanBrainrot.Player
{
    /// <summary>
    /// Helper class to detect swipes and trigger lane changes on the Player_Controller.
    /// Optimized for mobile (iOS).
    /// </summary>
    [RequireComponent(typeof(Player_Controller))]
    public class Player_SwipeHandler : MonoBehaviour
    {
        private Player_Controller playerController;

        [Header("Swipe Settings")]
        [SerializeField] private float minimumSwipeDistance = 50f;
        
        private Vector2 touchStartPosition;
        private bool isSwiping = false;

        private void Awake()
        {
            playerController = GetComponent<Player_Controller>();
        }

        private void Update()
        {
            HandleTouchInput();
        }

        private void HandleTouchInput()
        {
            if (Touchscreen.current == null) return;

            var touch = Touchscreen.current.primaryTouch;

            if (!touch.press.isPressed)
            {
                if (isSwiping)
                {
                    DetectSwipe(touch.position.ReadValue());
                    isSwiping = false;
                }
                return;
            }

            if (touch.press.wasPressedThisFrame)
            {
                touchStartPosition = touch.position.ReadValue();
                isSwiping = true;
            }
        }

        private void DetectSwipe(Vector2 touchEndPosition)
        {
            Vector2 swipeDelta = touchEndPosition - touchStartPosition;

            if (swipeDelta.magnitude < minimumSwipeDistance) return;

            // Horizontal swipe detection
            if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
            {
                if (swipeDelta.x > 0)
                {
                    playerController.ChangeLane(1);
                }
                else
                {
                    playerController.ChangeLane(-1);
                }
            }
        }
    }
}
