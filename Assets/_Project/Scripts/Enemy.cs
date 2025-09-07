using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyType  { Cube, Sphere, Capsule }

    private EnemyType enemyType;

    [Header("Movement Settings")]
    private float speed = 6f;
    private int health = 1;
    private int points = 1;

    private Transform playerTransform; // This is to check if thi object is behind the player
    private float destroyZThreshold = -0.5f; // Z position to destroy the enemy
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        playerTransform =gameManager.PlayerTransform;
    }

    private void Update()
    {
        if(gameManager.IsGameOver) return;

        transform.Translate(Vector3.back * speed * Time.deltaTime);

        if(transform.position.z <= playerTransform.position.z + destroyZThreshold)
        {
           gameManager.GameOver();
        }
    }

    public void SetEnemySettings(EnemyType enemyType, float speed)
    {
        SetEnemyType(enemyType);
        SetSpeed(speed);
    }

    public void SetEnemyType(EnemyType enemyType)
    {
        this.enemyType = enemyType;

        switch (enemyType)
        {
            case EnemyType.Cube:
                health = 6; points = 10; break;
            case EnemyType.Sphere:
                health = 12; points = 20; break;
            case EnemyType.Capsule:
                health = 18; points = 40; break;
        }
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public void TakeDamage(int damage)
    {
        if(GameManager.Instance.IsGameOver) return;
        health -= damage;

        if(health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
       gameManager.AddScore(points);
        Destroy(gameObject);
        // TODO: Add particle effects here
    }

    public EnemyType GetEnemyType() => enemyType;
}
