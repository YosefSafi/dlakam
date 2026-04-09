using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace StickmanBrainrot.Events
{
    [System.Serializable]
    public class BrainrotEvent
    {
        public string eventName;
        public float duration;
        public bool isActive;
    }

    /// <summary>
    /// Manages the triggering and lifecycle of random "Brainrot" events.
    /// </summary>
    public class System_EventManager : MonoBehaviour
    {
        public static System_EventManager Instance { get; private set; }

        [Header("Event Settings")]
        [SerializeField] private float eventCheckInterval = 10f; // Score interval
        [SerializeField] private int maxActiveEvents = 2;
        [SerializeField] private List<BrainrotEvent> availableEvents = new List<BrainrotEvent>();

        private float lastTriggerScore = 0f;
        private List<BrainrotEvent> activeEvents = new List<BrainrotEvent>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Update()
        {
            if (StickmanBrainrot.Systems.System_ScoreManager.Instance == null) return;

            float currentScore = StickmanBrainrot.Systems.System_ScoreManager.Instance.CurrentScore;

            // Trigger every X score points
            if (currentScore >= lastTriggerScore + eventCheckInterval)
            {
                TriggerRandomEvent();
                lastTriggerScore = currentScore;
            }
        }

        public void TriggerRandomEvent()
        {
            if (activeEvents.Count >= maxActiveEvents) return;

            // Pick a random event (placeholder: just logging for now)
            Debug.Log("BRAINROT EVENT TRIGGERED!");
            
            // In a real scenario, we'd pick from availableEvents
            // and call a specific event script.
            StartCoroutine(PlaceholderEventRoutine("RANDOM_CHAOS", 5f));
        }

        private IEnumerator PlaceholderEventRoutine(string name, float duration)
        {
            BrainrotEvent newEvent = new BrainrotEvent { eventName = name, duration = duration, isActive = true };
            activeEvents.Add(newEvent);

            Debug.Log("Event Started: " + name);

            yield return new WaitForSeconds(duration);

            newEvent.isActive = false;
            activeEvents.Remove(newEvent);
            Debug.Log("Event Ended: " + name);
        }
    }
}
