using UnityEngine;
using UnityEngine.SceneManagement;

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
                DontDestroyOnLoad(gameObject);
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
            
            // Trigger UI or Restart logic here
            // Time.timeScale = 0f; // Pause game? Or let chaos continue while dead?
        }

        public void RestartGame()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            currentState = GameState.Playing;
        }

        public GameState CurrentState => currentState;
        public bool IsPlaying => currentState == GameState.Playing;
    }
}
