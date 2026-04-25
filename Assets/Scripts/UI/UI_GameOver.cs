using UnityEngine;
using TMPro;
using StickmanBrainrot.Systems;

namespace StickmanBrainrot.UI
{
    /// <summary>
    /// Handles the Game Over screen display and restart logic.
    /// </summary>
    public class UI_GameOver : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private TextMeshProUGUI finalScoreText;
        [SerializeField] private TextMeshProUGUI finalCoinsText;

        private void Start()
        {
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(false); // Hide at start
            }
        }

        public void ShowGameOver()
        {
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
            }

            if (System_ScoreManager.Instance != null)
            {
                if (finalScoreText != null)
                {
                    finalScoreText.text = "FINAL SCORE: " + System_ScoreManager.Instance.CurrentScore.ToString("D5");
                }
                
                if (finalCoinsText != null)
                {
                    finalCoinsText.text = "COINS: " + System_ScoreManager.Instance.TotalCoins;
                }
            }
        }

        public void OnRestartClicked()
        {
            if (System_GameManager.Instance != null)
            {
                System_GameManager.Instance.RestartGame();
            }
        }
    }
}
