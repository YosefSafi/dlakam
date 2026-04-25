using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace StickmanBrainrot.Events
{
    /// <summary>
    /// Manages the triggering and lifecycle of random "Brainrot" events.
    /// </summary>
    public class System_EventManager : MonoBehaviour
    {
        public static System_EventManager Instance { get; private set; }

        [Header("Event Settings")]
        [SerializeField] private float eventCheckInterval = 15f; // Score interval
        [SerializeField] private int maxActiveEvents = 2;
        
        [Header("Available Events")]
        [SerializeField] private List<Event_Base> availableEvents = new List<Event_Base>();

        private float lastTriggerScore = 0f;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            // Auto-gather events attached to this object or its children if the list is empty
            if (availableEvents.Count == 0)
            {
                availableEvents.AddRange(GetComponentsInChildren<Event_Base>());
            }
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
            if (availableEvents == null || availableEvents.Count == 0) return;

            int activeCount = availableEvents.Count(e => e.IsActive);
            if (activeCount >= maxActiveEvents) return;

            // Pick a random event that is not currently active
            List<Event_Base> inactiveEvents = availableEvents.Where(e => !e.IsActive).ToList();
            
            if (inactiveEvents.Count > 0)
            {
                Event_Base chosenEvent = inactiveEvents[Random.Range(0, inactiveEvents.Count)];
                chosenEvent.StartEvent();
            }
        }
    }
}
