using UnityEngine;

namespace StickmanBrainrot.Systems
{
    /// <summary>
    /// Handles coin behavior and value scaling based on current score.
    /// </summary>
    public class System_Coin : MonoBehaviour
    {
        [Header("Coin Rotation")]
        [SerializeField] private float rotationSpeed = 100f;

        private void Update()
        {
            // Simple visual effect
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Collect();
            }
        }

        private void Collect()
        {
            // Calculate value based on score
            int value = GetCoinValue();

            if (System_ScoreManager.Instance != null)
            {
                System_ScoreManager.Instance.AddCoins(value);
            }

            // Destroy coin on collect
            Destroy(gameObject);
        }

        private int GetCoinValue()
        {
            if (System_ScoreManager.Instance == null) return 1;

            int score = System_ScoreManager.Instance.CurrentScore;

            // RULE from AI SPEC:
            // Score 1–30   → value = 1
            // Score 30–60  → value = 2
            // Score 60–99  → value = 3
            // Score 100+   → value = 5

            if (score >= 100) return 5;
            if (score >= 60) return 3;
            if (score >= 30) return 2;
            return 1;
        }
    }
}
