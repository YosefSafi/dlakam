using UnityEngine;

namespace StickmanBrainrot.Systems
{
    /// <summary>
    /// Handles collision detection between the player and other objects (obstacles, coins).
    /// </summary>
    public class System_Collision : MonoBehaviour
    {
        [Header("Collision Settings")]
        [SerializeField] private string obstacleTag = "Obstacle";
        [SerializeField] private string coinTag = "Coin";

        private void OnTriggerEnter(Collider other)
        {
            // 1. Check for Obstacle Collision
            if (other.CompareTag(obstacleTag))
            {
                OnHitObstacle(other.gameObject);
            }

            // 2. Check for Coin Collision
            if (other.CompareTag(coinTag))
            {
                OnHitCoin(other.gameObject);
            }
        }

        private void OnHitObstacle(GameObject obstacle)
        {
            Debug.Log("PLAYER COLLIDED WITH OBSTACLE!");

            // Trigger GameOver via GameManager
            if (System_GameManager.Instance != null)
            {
                System_GameManager.Instance.GameOver();
            }

            // Stop movement? Handled in Player_Controller based on GameManager state?
        }

        private void OnHitCoin(GameObject coin)
        {
            // Handled in Step 6 (Coin system)
            Debug.Log("PLAYER COLLECTED COIN!");
            
            // For now:
            Destroy(coin);
        }
    }
}
