using UnityEngine;
using System.Collections;

namespace StickmanBrainrot.Events
{
    /// <summary>
    /// Abstract base class for all brainrot random events.
    /// </summary>
    public abstract class Event_Base : MonoBehaviour
    {
        [Header("Event Settings")]
        [SerializeField] protected string eventName = "Unknown Event";
        [SerializeField] protected float defaultDuration = 5f;
        
        public bool IsActive { get; protected set; } = false;

        /// <summary>
        /// Starts the event execution flow.
        /// </summary>
        public void StartEvent(float durationOverride = -1f)
        {
            if (IsActive) return;

            float duration = durationOverride > 0 ? durationOverride : defaultDuration;
            StartCoroutine(EventRoutine(duration));
        }

        private IEnumerator EventRoutine(float duration)
        {
            IsActive = true;
            Debug.Log($"EVENT STARTED: {eventName}");
            
            OnEventStart();
            
            yield return new WaitForSeconds(duration);
            
            OnEventEnd();
            
            Debug.Log($"EVENT ENDED: {eventName}");
            IsActive = false;
        }

        /// <summary>
        /// Called when the event begins. Override to add logic.
        /// </summary>
        protected abstract void OnEventStart();

        /// <summary>
        /// Called when the event ends. Override to clean up/reset logic.
        /// </summary>
        protected abstract void OnEventEnd();

        public string EventName => eventName;
    }
}
