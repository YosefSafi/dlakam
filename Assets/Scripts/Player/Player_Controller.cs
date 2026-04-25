using UnityEngine;
using UnityEngine.InputSystem;

namespace StickmanBrainrot.Player
{
    /// <summary>
    /// Handles player auto-running and lane-based movement.
    /// Lanes: -1 (Left), 0 (Middle), 1 (Right)
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class Player_Controller : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float forwardSpeed = 5f;
        [SerializeField] private float laneDistance = 3f;
        [SerializeField] private float laneChangeSpeed = 15f;
        [SerializeField] private float jumpHeight = 2f;
        [SerializeField] private float gravity = -20f;

        [Header("Runtime Multipliers")]
        private float speedMultiplier = 1f;

        [Header("Lane State")]
        [SerializeField] private int currentLane = 0; // -1, 0, 1
        
        private CharacterController controller;
        private Vector3 velocity;
        
        [Header("Input Controls")]
        private PlayerInput playerInput;
        private InputAction moveAction;
        private InputAction jumpAction;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            playerInput = GetComponent<PlayerInput>();
            
            if (playerInput == null)
            {
                Debug.LogError("PlayerInput component is missing on the Player prefab!");
                return;
            }

            moveAction = playerInput.actions["Move"];
            jumpAction = playerInput.actions["Jump"];
        }

        private void Update()
        {
            if (controller == null)
            {
                controller = GetComponent<CharacterController>();
                if (controller == null) return;
            }

            // If there is no GameManager in the scene, we still want to move for testing purposes
            if (System_GameManager.Instance != null && !System_GameManager.Instance.IsPlaying) return;

            // 1. Handle Lane Switching Input
            HandleLaneInput();

            // 2. Calculate Horizontal Movement (Lane Based)
            float targetX = currentLane * laneDistance;
            float currentX = transform.position.x;
            float newX = Mathf.MoveTowards(currentX, targetX, laneChangeSpeed * Time.deltaTime);
            float xMovement = newX - currentX;

            // 3. Handle Jump & Gravity
            bool isGrounded = controller.isGrounded;
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            if (jumpAction != null && jumpAction.triggered && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            velocity.y += gravity * Time.deltaTime;

            // 4. Combine Movement
            float currentForwardSpeed = forwardSpeed * speedMultiplier;
            Vector3 moveVector = new Vector3(xMovement, velocity.y * Time.deltaTime, currentForwardSpeed * Time.deltaTime);
            controller.Move(moveVector);
        }

        private void HandleLaneInput()
        {
            if (moveAction == null) return;

            // Use WasPressedThisFrame style check for discrete lane switching
            if (moveAction.triggered)
            {
                Vector2 moveValue = moveAction.ReadValue<Vector2>();
                if (moveValue.x < -0.1f)
                {
                    ChangeLane(-1);
                }
                else if (moveValue.x > 0.1f)
                {
                    ChangeLane(1);
                }
            }
        }

        public void ChangeLane(int direction)
        {
            currentLane = Mathf.Clamp(currentLane + direction, -1, 1);
        }

        // Methods for external systems
        public void ModifySpeed(float multiplier)
        {
            speedMultiplier = multiplier;
        }

        public void ResetSpeed()
        {
            speedMultiplier = 1f;
        }

        public int CurrentLane => currentLane;
        public float ForwardSpeed => forwardSpeed * speedMultiplier;
    }
}
