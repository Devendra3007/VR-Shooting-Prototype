using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum GunType { Type1, Type2, Type3 }

    [Header("Gun Settings")]
    [SerializeField] private int damagePerShot = 3; // points of damage per bullet
    [SerializeField] private int firingRate = 3; // bullets per second

    [Header("Shooting")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private TrailRenderer bulletTrailRenderer;

    private float maxRange = 100f;
    private float shootCooldown = 0;

    [Header("Trigger Settings")]
    private OVRInput.Axis1D triggerAxis = OVRInput.Axis1D.PrimaryIndexTrigger;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;

    public void SetGunHand(OVRInput.Axis1D triggerAxis)
    {
        this.triggerAxis = triggerAxis;
    }

    private void Update()
    {
        // If the game is over or paused, do not move
        if (GameManager.Instance.CurrentGameState is GameManager.GameState.GameOver or GameManager.GameState.Paused) return;

        // Get trigger value (0 to 1)
        float triggerValue = OVRInput.Get(triggerAxis);

        if (triggerValue > 0.8f) // pressed threshold
        {
            shootCooldown -= Time.deltaTime;
            if (shootCooldown <= 0f)
            {
                Shoot();
                shootCooldown = 1f / Mathf.Max(0.0001f, firingRate);
            }
        }
    }

    private void Shoot()
    {
        // Play muzzle flash
        muzzleFlash.Play();
        audioSource.Play();

        Ray ray = new Ray(firePoint.position, firePoint.forward);
        Vector3 endPoint = ray.origin + ray.direction * maxRange;
        if (Physics.Raycast(ray, out RaycastHit hit, maxRange, enemyLayer))
        {
            endPoint = hit.point;

            // Check if the hit object has an Enemy component
            Enemy enemy = hit.collider.GetComponentInParent<Enemy>(); // Enemy Script is on parent object hence use GetComponentInParent
            if (enemy != null)
            {
                enemy.TakeDamage(damagePerShot, hit.point, hit.normal);
            }
        }

        // Show bullet trail
        TrailRenderer trailRenderer = Instantiate(bulletTrailRenderer, firePoint.position, Quaternion.identity);
        StartCoroutine(SpawnTrail(trailRenderer, hit));
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPosition = trail.transform.position;
        Vector3 endPosition = hit.collider ? hit.point : (firePoint.position + firePoint.forward * maxRange); // If no hit, extend to max range
        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPosition, endPosition, time);
            time += Time.deltaTime / trail.time;
            yield return null;
        }
        trail.transform.position = endPosition;
        Destroy(trail.gameObject, trail.time);
    }

}
