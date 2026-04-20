using UnityEngine;
using StickmanBrainrot.Player;
using StickmanBrainrot.Systems;
using StickmanBrainrot.Core;

namespace StickmanBrainrot.Core
{
    /// <summary>
    /// Helper script to automatically build the scene for testing.
    /// Drag this script onto an Empty GameObject in a new scene and press Play!
    /// </summary>
    public class System_Bootstrap : MonoBehaviour
    {
        [Header("Setup Options")]
        public bool autoBuildOnStart = true;

        private void Start()
        {
            if (autoBuildOnStart) BuildWorld();
        }

        public void BuildWorld()
        {
            Debug.Log("BOOTSTRAP: Building the Brainrot world...");

            // 1. Create Ground
            GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
            floor.name = "FLOOR";
            floor.transform.localScale = new Vector3(20, 1, 1000); // Very long road
            floor.transform.position = new Vector3(0, 0, 500);

            // 2. Create Managers
            GameObject managers = new GameObject("_MANAGERS");
            managers.AddComponent<System_GameManager>();
            managers.AddComponent<System_ScoreManager>();
            managers.AddComponent<System_SpawnManager>();
            managers.AddComponent<StickmanBrainrot.Events.System_EventManager>();

            // 3. Create Player
            GameObject player = GameObject.CreatePrimitive(PrimitiveType.Cube);
            player.name = "PLAYER_CUBE";
            player.tag = "Player";
            player.transform.position = new Vector3(0, 1, 0);
            
            // Add Player Logic
            player.AddComponent<Player_Controller>();
            player.AddComponent<Player_SwipeHandler>();
            player.AddComponent<System_Collision>();
            player.AddComponent<UnityEngine.InputSystem.PlayerInput>();
            
            // Add Physics for triggers
            Rigidbody rb = player.AddComponent<Rigidbody>();
            rb.isKinematic = true;

            // 4. Setup Camera
            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                var follow = mainCam.gameObject.AddComponent<System_CameraFollow>();
                follow.SetTarget(player.transform);
            }

            Debug.Log("BOOTSTRAP COMPLETE: Use A/D to change lanes and watch the console for Brainrot Events!");
            Destroy(this.gameObject); // Cleanup bootstrap
        }
    }
}
