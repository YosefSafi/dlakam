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
            if (controller == null) return;
            if (System_GameManager.Instance != null && !System_GameManager.Instance.IsPlaying) return;

            // 1. Handle Lane Switching Input
            HandleLaneInput();

            // 2. Calculate Horizontal Movement (Lane Based)
            float targetX = currentLane * laneDistance;
            float newX = Mathf.MoveTowards(transform.position.x, targetX, laneChangeSpeed * Time.deltaTime);
            float xMovement = newX - transform.position.x;

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
            // X: Lane movement, Y: Jump/Gravity (multiplied by dt), Z: Auto-forward
            // Note: CharacterController.Move expects absolute movement vector for the frame.
            Vector3 moveVector = new Vector3(xMovement, velocity.y * Time.deltaTime, forwardSpeed * Time.deltaTime);
            controller.Move(moveVector);
        }

        private void HandleLaneInput()
        {
            if (moveAction == null) return;

            if (moveAction.triggered)
            {
                float moveValue = moveAction.ReadValue<Vector2>().x;
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
            currentLane = Mathf.Clamp(currentLane + direction, -1, 1);
        }

        // Getters for external systems
        public int CurrentLane => currentLane;
        public float ForwardSpeed => forwardSpeed;
    }
}
