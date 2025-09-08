using System.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{


    [Header("Player Transform")]
    private Transform playerTransform; // Fetched from gameManager at start

    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject cubeEnemyPrefab;
    [SerializeField] private GameObject sphereEnemyPrefab;
    [SerializeField] private GameObject capsuleEnemyPrefab;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnPositionZ = 20f; // Z position to spawn enemies
    [SerializeField] private float spawnPositionY = 0.5f; // Y position to spawn enemies
    [SerializeField] private float corridorHalfWidth = 6f; // Half the width of the corridor (to spawn within -width to +width)
    [SerializeField] private float minSpawnInterval = 0.5f; // Minimum time between spawns
    [SerializeField] private float maxSpawnInterval = 2f; // Maximum time between spawns
    [SerializeField] private float minSpeed = 0.005f; // Minimum enemy speed
    [SerializeField] private float maxSpeed = 0.5f; // Maximum enemy speed

    private bool isRunning = false;
    private Coroutine spawnCoroutine;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        playerTransform = gameManager.PlayerTransform;
        isRunning = true;
        spawnCoroutine = StartCoroutine(SpawnEnemyRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (isRunning)
        {
            float wait = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(wait);
            if (gameManager.CurrentGameState is GameManager.GameState.GameOver or GameManager.GameState.Paused) continue;
            SpawnRandomEnemy();
        }
    }

    void SpawnRandomEnemy()
    {
        int r = Random.Range(0, 3);
        GameObject prefab = cubeEnemyPrefab;
        Enemy.EnemyType type = Enemy.EnemyType.Cube;

        switch (r)
        {
            case 0: prefab = cubeEnemyPrefab; type = Enemy.EnemyType.Cube; break;
            case 1: prefab = sphereEnemyPrefab; type = Enemy.EnemyType.Sphere; break;
            case 2: prefab = capsuleEnemyPrefab; type = Enemy.EnemyType.Capsule; break;
        }

        float x = Random.Range(-corridorHalfWidth, corridorHalfWidth);
        Vector3 pos = new Vector3(x, spawnPositionY, playerTransform.position.z + spawnPositionZ);

        var go = Instantiate(prefab, pos, Quaternion.identity);
        var enemy = go.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.SetEnemySettings(type, Random.Range(minSpeed, maxSpeed));
        }
    }

}
