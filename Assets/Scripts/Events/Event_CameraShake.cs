using UnityEngine;
using StickmanBrainrot.Core;

namespace StickmanBrainrot.Events
{
    /// <summary>
    /// Event that violently shakes the camera for the duration of the event.
    /// </summary>
    public class Event_CameraShake : Event_Base
    {
        [Header("Shake Settings")]
        [SerializeField] private float shakeMagnitude = 0.5f;

        private System_CameraFollow cameraFollow;

        private void Awake()
        {
            eventName = "CAMERA SHAKE";
            cameraFollow = Object.FindAnyObjectByType<System_CameraFollow>();
        }

        protected override void OnEventStart()
        {
            if (cameraFollow == null) cameraFollow = Object.FindAnyObjectByType<System_CameraFollow>();
            
            if (cameraFollow != null)
            {
                // Trigger a continuous or very long shake.
                // The duration passed here matches the event duration.
                cameraFollow.TriggerShake(defaultDuration, shakeMagnitude);
            }
        }

        protected override void OnEventEnd()
        {
            // CameraFollow script automatically stops shaking when duration reaches 0.
            // We could manually zero it out here if we tracked duration exactly.
        }
    }
}
