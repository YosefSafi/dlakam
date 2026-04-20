using UnityEngine;

namespace StickmanBrainrot.Core
{
    /// <summary>
    /// Smoothly follows the player with a slight delay for a "floaty" feeling.
    /// Includes support for camera shake effects.
    /// </summary>
    public class System_CameraFollow : MonoBehaviour
    {
        [Header("Follow Settings")]
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new Vector3(0, 5, -7);
        [SerializeField] private float smoothSpeed = 0.125f;
        [SerializeField] private bool followX = false; // Only follow on Y and Z by default for a fixed lane look?
                                                       // Actually, for "brainrot" we might want it to follow X too.

        [Header("Shake Settings")]
        private float shakeDuration = 0f;
        private float shakeMagnitude = 0.1f;
        private float dampingSpeed = 1.0f;
        private Vector3 initialPosition;

        private void Start()
        {
            if (target == null)
            {
                // Try to find player by tag if not assigned
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null) target = player.transform;
            }
        }

        private void LateUpdate()
        {
            if (target == null) return;

            // 1. Follow Logic
            FollowTarget();

            // 2. Shake Logic (if active)
            HandleShake();
        }

        private void FollowTarget()
        {
            // Calculate desired position
            Vector3 desiredPosition = target.position + offset;

            // If we don't want to follow X (lane changes), keep desiredPosition.x as the initial offset X
            if (!followX)
            {
                desiredPosition.x = offset.x;
            }

            // Smoothly move towards desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            
            transform.position = smoothedPosition;

            // Always look at the player slightly ahead
            transform.LookAt(target.position + Vector3.up * 1.5f);
        }

        private void HandleShake()
        {
            if (shakeDuration > 0)
            {
                transform.localPosition += Random.insideUnitSphere * shakeMagnitude;
                shakeDuration -= Time.deltaTime * dampingSpeed;
            }
        }

        /// <summary>
        /// Call this to trigger a camera shake effect.
        /// </summary>
        public void TriggerShake(float duration = 0.5f, float magnitude = 0.15f)
        {
            shakeDuration = duration;
            shakeMagnitude = magnitude;
        }

        // Setters for external systems
        public void SetTarget(Transform newTarget) => target = newTarget;
        public void SetFollowX(bool value) => followX = value;
    }
}
