using UnityEngine;

namespace StickmanBrainrot.Systems
{
    /// <summary>
    /// Base class for obstacles in the game.
    /// </summary>
    public class System_Obstacle : MonoBehaviour
    {
        [Header("Obstacle Settings")]
        [SerializeField] private bool isDestructible = false;
        [SerializeField] private float damage = 100f; // Instantly kills player by default

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                // Logic for collision will be handled by the System_Collision script
                // but we can trigger visual effects here later.
                Debug.Log("Collision with obstacle: " + name);
            }
        }
    }
}
