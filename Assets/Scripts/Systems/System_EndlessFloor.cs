using UnityEngine;

namespace StickmanBrainrot.Systems
{
    /// <summary>
    /// Simple script to keep the floor moving with the player to simulate an endless runner.
    /// </summary>
    public class System_EndlessFloor : MonoBehaviour
    {
        private Transform playerTransform;
        [SerializeField] private float zOffset = 50f; // Keep the floor slightly ahead

        private void Start()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
        }

        private void Update()
        {
            if (playerTransform != null)
            {
                // Just move the floor along the Z axis with the player
                transform.position = new Vector3(0, transform.position.y, playerTransform.position.z + zOffset);
            }
        }
    }
}