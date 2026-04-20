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
        [SerializeField] private GameObject coinPrefab;
        [SerializeField] private Transform playerTransform;
        [SerializeField] private float spawnDistanceAhead = 50f;
        [SerializeField] private float initialSpawnDistance = 20f;
        [SerializeField] private float spawnInterval = 3f;
        [SerializeField] private float itemSpawnChance = 0.5f; // 50% chance to spawn coin instead of obstacle? 
                                                              // Actually let's just spawn both.

        [Header("Lane Settings")]
        [SerializeField] private float laneDistance = 3f;

        private float nextSpawnTime;
        private List<GameObject> activeObjects = new List<GameObject>();
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
                SpawnRandomAtDistance(initialSpawnDistance + (i * 15f));
            }
        }

        private void Update()
        {
            if (playerTransform == null) return;

            // 1. Time-based Spawning
            if (Time.time >= nextSpawnTime)
            {
                SpawnRandomAtDistance(spawnDistanceAhead);
                nextSpawnTime = Time.time + spawnInterval;
            }

            // 2. Cleanup
            CleanupObjects();
        }

        private void SpawnRandomAtDistance(float distance)
        {
            // Randomly decide between obstacle or coin
            if (Random.value > 0.3f)
            {
                SpawnObstacle(distance);
            }
            else
            {
                SpawnCoin(distance);
            }
        }

        private void SpawnObstacle(float distance)
        {
            if (obstaclePrefabs.Length == 0) return;

            int lane = Random.Range(-1, 2);
            Vector3 spawnPosition = new Vector3(lane * laneDistance, 0, playerTransform.position.z + distance);

            int prefabIndex = Random.Range(0, obstaclePrefabs.Length);
            GameObject newObstacle = Instantiate(obstaclePrefabs[prefabIndex], spawnPosition, Quaternion.identity);
            
            activeObjects.Add(newObstacle);
        }

        private void SpawnCoin(float distance)
        {
            if (coinPrefab == null) return;

            int lane = Random.Range(-1, 2);
            Vector3 spawnPosition = new Vector3(lane * laneDistance, 1.0f, playerTransform.position.z + distance); // Coins slightly floating

            GameObject newCoin = Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
            activeObjects.Add(newCoin);
        }

        private void CleanupObjects()
        {
            for (int i = activeObjects.Count - 1; i >= 0; i--)
            {
                if (activeObjects[i] == null)
                {
                    activeObjects.RemoveAt(i);
                    continue;
                }

                if (activeObjects[i].transform.position.z < playerTransform.position.z - despawnDistanceBehind)
                {
                    Destroy(activeObjects[i]);
                    activeObjects.RemoveAt(i);
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
