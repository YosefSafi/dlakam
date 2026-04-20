using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -9.81f;

    [Header("Input References")]
    [SerializeField] private InputActionAsset inputActions;
    
    private CharacterController controller;
    private Vector2 moveInput;
    private Vector3 velocity;
    private bool isGrounded;

    private InputAction moveAction;
    private InputAction jumpAction;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        
        if (inputActions == null)
        {
            Debug.LogError("InputActionAsset is not assigned on PlayerController!");
            return;
        }

        var playerMap = inputActions.FindActionMap("Player");
        if (playerMap == null)
        {
            Debug.LogError("Player Action Map not found!");
            return;
        }

        moveAction = playerMap.FindAction("Move");
        jumpAction = playerMap.FindAction("Jump");
    }

    private void OnEnable()
    {
        moveAction?.Enable();
        jumpAction?.Enable();
    }

    private void OnDisable()
    {
        moveAction?.Disable();
        jumpAction?.Disable();
    }

    private void Update()
    {
        if (controller == null) return;

        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Movement
        if (moveAction != null)
        {
            moveInput = moveAction.ReadValue<Vector2>();
            Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
            controller.Move(move * Time.deltaTime * moveSpeed);
            
            // Basic rotation to face movement direction if moving
            if (move != Vector3.zero)
            {
                transform.forward = move;
            }
        }

        // Jump
        if (jumpAction != null && jumpAction.triggered && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
