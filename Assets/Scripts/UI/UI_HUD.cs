using UnityEngine;
using UnityEngine.UI;
using TMPro; // Assuming TextMeshPro is used (Standard in modern Unity)
using StickmanBrainrot.Systems;

namespace StickmanBrainrot.UI
{
    /// <summary>
    /// Manages the main gameplay HUD (Score and Coins).
    /// </summary>
    public class UI_HUD : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI coinText;

        private void Update()
        {
            if (System_ScoreManager.Instance == null) return;

            // Update Score Display
            if (scoreText != null)
            {
                scoreText.text = "SCORE: " + System_ScoreManager.Instance.CurrentScore.ToString("D5");
            }

            // Update Coin Display
            if (coinText != null)
            {
                coinText.text = "BRAINROT COINS: " + System_ScoreManager.Instance.TotalCoins;
            }
        }
    }
}
