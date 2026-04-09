using UnityEngine;
using System.Collections.Generic;

namespace StickmanBrainrot.Systems
{
    /// <summary>
    /// Spawns obstacles and items in random lanes ahead of the player.
    /// Manages object pooling or cleanup to keep memory usage low on iOS.
    /// </summary>
    public class System_SpawnManager : MonoBehaviour
    {
        [Header("Spawn Settings")]
        [SerializeField] private GameObject[] obstaclePrefabs;
        [SerializeField] private Transform playerTransform;
        [SerializeField] private float spawnDistanceAhead = 50f;
        [SerializeField] private float initialSpawnDistance = 20f;
        [SerializeField] private float spawnInterval = 3f;
        
        [Header("Lane Settings")]
        [SerializeField] private float laneDistance = 3f;

        private float nextSpawnTime;
        private List<GameObject> activeObstacles = new List<GameObject>();
        private float despawnDistanceBehind = 10f;

        private void Start()
        {
            if (playerTransform == null)
            {
                playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            }

            // Initial spawning to populate the path
            for (int i = 0; i < 5; i++)
            {
                SpawnObstacle(initialSpawnDistance + (i * 15f));
            }
        }

        private void Update()
        {
            if (playerTransform == null) return;

            // 1. Time-based Spawning
            if (Time.time >= nextSpawnTime)
            {
                SpawnObstacle(spawnDistanceAhead);
                nextSpawnTime = Time.time + spawnInterval;
            }

            // 2. Cleanup
            CleanupObstacles();
        }

        private void SpawnObstacle(float distance)
        {
            if (obstaclePrefabs.Length == 0) return;

            // Randomize lane (-1, 0, 1)
            int lane = Random.Range(-1, 2);
            Vector3 spawnPosition = new Vector3(lane * laneDistance, 0, playerTransform.position.z + distance);

            // Select random prefab
            int prefabIndex = Random.Range(0, obstaclePrefabs.Length);
            GameObject newObstacle = Instantiate(obstaclePrefabs[prefabIndex], spawnPosition, Quaternion.identity);
            
            activeObstacles.Add(newObstacle);
        }

        private void CleanupObstacles()
        {
            for (int i = activeObstacles.Count - 1; i >= 0; i--)
            {
                if (activeObstacles[i] == null)
                {
                    activeObstacles.RemoveAt(i);
                    continue;
                }

                // If obstacle is too far behind the player, destroy it
                if (activeObstacles[i].transform.position.z < playerTransform.position.z - despawnDistanceBehind)
                {
                    Destroy(activeObstacles[i]);
                    activeObstacles.RemoveAt(i);
                }
            }
        }

        // External hook for dynamic difficulty adjustment (scaling spawn interval)
        public void UpdateSpawnInterval(float newInterval)
        {
            spawnInterval = Mathf.Max(0.5f, newInterval);
        }
    }
}
