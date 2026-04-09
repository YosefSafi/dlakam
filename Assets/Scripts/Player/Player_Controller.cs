using UnityEngine;
using UnityEngine.InputSystem;

namespace StickmanBrainrot.Player
{
    /// <summary>
    /// Handles player auto-running and lane-based movement.
    /// Lanes: -1 (Left), 0 (Middle), 1 (Right)
    /// </summary>
    public class Player_Controller : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float forwardSpeed = 5f;
        [SerializeField] private float laneDistance = 3f;
        [SerializeField] private float laneChangeSpeed = 15f;

        [Header("Lane State")]
        [SerializeField] private int currentLane = 0; // -1, 0, 1
        private Vector3 targetPosition;

        [Header("Input Controls")]
        private PlayerInput playerInput;
        private InputAction moveAction;

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
            if (playerInput == null)
            {
                Debug.LogError("PlayerInput component is missing on the Player prefab!");
                return;
            }

            // Using "Move" action for lane switching (X axis for left/right)
            moveAction = playerInput.actions["Move"];
        }

        private void Update()
        {
            // 1. Handle Auto Forward Movement
            MoveForward();

            // 2. Handle Lane Switching Input
            HandleLaneInput();

            // 3. Smoothly Transition to Lane
            UpdateLanePosition();
        }

        private void MoveForward()
        {
            transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);
        }

        private void HandleLaneInput()
        {
            if (moveAction == null) return;

            // Simplified: Change lane on single press/swipe (detected via input system)
            float moveValue = moveAction.ReadValue<Vector2>().x;

            if (moveAction.triggered)
            {
                if (moveValue < -0.1f)
                {
                    ChangeLane(-1);
                }
                else if (moveValue > 0.1f)
                {
                    ChangeLane(1);
                }
            }
        }

        public void ChangeLane(int direction)
        {
            int targetLane = currentLane + direction;

            // Clamp lane between -1 and 1
            if (targetLane < -1 || targetLane > 1) return;

            currentLane = targetLane;
        }

        private void UpdateLanePosition()
        {
            // Target X depends on current lane
            targetPosition = new Vector3(currentLane * laneDistance, transform.position.y, transform.position.z);

            // Lerp towards the target position on the X axis only
            Vector3 nextPosition = Vector3.Lerp(transform.position, targetPosition, laneChangeSpeed * Time.deltaTime);
            
            // Maintain original Y and current Z (since forward movement is handled separately or we update Z here)
            // To avoid conflicts with MoveForward translate, we only update X here if we use translate for Z.
            // Better: Set the position directly including the forward progress.
            
            transform.position = new Vector3(nextPosition.x, transform.position.y, transform.position.z);
        }

        // Getters for external systems
        public int CurrentLane => currentLane;
        public float ForwardSpeed => forwardSpeed;
    }
}
