using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyType  { Cube, Sphere, Capsule }

    private EnemyType enemyType;

    [Header("Movement Settings")]
    private float speed = 0.5f;
    private int health = 1;
    private int points = 1;

    private Transform playerTransform; // This is to check if thi object is behind the player
    private float destroyZThreshold = -0.5f; // Z position to destroy the enemy
    private GameManager gameManager;

    private ParticleSystem[] damageVFX;


    private void Start()
    {
        gameManager = GameManager.Instance;
        playerTransform =gameManager.PlayerTransform;

        damageVFX = GetComponentsInChildren<ParticleSystem>();
    }

    private void Update()
    {
        // If the game is over or paused, do not move
        if (gameManager.CurrentGameState is GameManager.GameState.GameOver or GameManager.GameState.Paused) return;

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
                health = 6; points = 5; break;
            case EnemyType.Sphere:
                health = 12; points = 10; break;
            case EnemyType.Capsule:
                health = 18; points = 15; break;
        }
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public void TakeDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if(GameManager.Instance.CurrentGameState is GameManager.GameState.GameOver) return;

        health -= damage;
        PlayDamageVFX(hitPoint, hitNormal);

        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log($"{enemyType} Enemy Destroyed! +{points} points");
        gameManager.AddScore(points);
        Destroy(gameObject);
        // TODO: Add particle effects here
    }

    public void PlayDamageVFX(Vector3 hitPoint, Vector3 hitNormal)
    {
        if(damageVFX.Length == 0) return;

        // Small offset so the effect appears slightly above the surface
        Vector3 spawnPos = hitPoint + hitNormal * 0.05f;

        // Move the parent container of the VFX to the hit point
        Transform vfxParent = damageVFX[0].transform.parent;
        vfxParent.position = hitPoint;
        vfxParent.forward = hitNormal; // rotate sparks to face outward

        foreach (var vfx in damageVFX)
        {
            vfx.Play();
        }
    }

    public EnemyType GetEnemyType() => enemyType;
}
