using UnityEngine;
using UnityEngine.SceneManagement;
using StickmanBrainrot.UI; // Added reference to UI namespace

namespace StickmanBrainrot.Systems
{
    public enum GameState { Menu, Playing, GameOver }

    /// <summary>
    /// Manages the overall game state, restarts, and game flow.
    /// </summary>
    public class System_GameManager : MonoBehaviour
    {
        public static System_GameManager Instance { get; private set; }

        [Header("Game State")]
        [SerializeField] private GameState currentState = GameState.Playing; // Start directly in Playing for now

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                // DontDestroyOnLoad(gameObject); // We probably don't want this if we reload the scene and it's in the scene
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void GameOver()
        {
            if (currentState == GameState.GameOver) return;
            
            currentState = GameState.GameOver;
            Debug.Log("GAME OVER! Final Score: " + (System_ScoreManager.Instance != null ? System_ScoreManager.Instance.CurrentScore : 0));
            
            // Pause the game mechanics
            Time.timeScale = 0f; 

            // Show Game Over UI
            UI_GameOver gameOverUI = Object.FindAnyObjectByType<UI_GameOver>();
            if (gameOverUI != null)
            {
                gameOverUI.ShowGameOver();
            }
        }

        public void RestartGame()
        {
            Time.timeScale = 1f;
            // Since we aren't using DontDestroyOnLoad, reload clears the old instance
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public GameState CurrentState => currentState;
        public bool IsPlaying => currentState == GameState.Playing;
    }
}
