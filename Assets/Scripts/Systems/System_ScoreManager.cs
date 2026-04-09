using UnityEngine;

namespace StickmanBrainrot.Systems
{
    /// <summary>
    /// Manages the player's score and total coins collected.
    /// Drives coin scaling and difficulty progression.
    /// </summary>
    public class System_ScoreManager : MonoBehaviour
    {
        public static System_ScoreManager Instance { get; private set; }

        [Header("Score State")]
        [SerializeField] private float currentScore = 0f;
        [SerializeField] private int totalCoins = 0;
        [SerializeField] private float scoreMultiplier = 1f;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            if (System_GameManager.Instance != null && !System_GameManager.Instance.IsPlaying) return;

            // Increase score based on time/distance (simplifying to time * multiplier)
            currentScore += Time.deltaTime * scoreMultiplier;

            // Trigger hooks for other systems if needed (events etc.)
        }

        public void AddCoins(int amount)
        {
            totalCoins += amount;
            Debug.Log("COINS COLLECTED! Total: " + totalCoins);
        }

        public int CurrentScore => (int)currentScore;
        public int TotalCoins => totalCoins;

        public void ResetScore()
        {
            currentScore = 0;
        }
    }
}
