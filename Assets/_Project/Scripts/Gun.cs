using Unity.VisualScripting;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum GunType { Type1, Type2, Type3 }

    [Header("Gun Settings")]
    [SerializeField] private GunType gunType = GunType.Type1;
    [SerializeField] private int damagePerShot = 3; // points of damage per bullet
    [SerializeField] private int firingRate = 3; // bullets per second

    [Header("Shooting")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private ParticleSystem muzzleFlash;
    private float maxRange = 100f;

    private bool canShoot = false;
    private float shootCooldown = 0;

    [Header("Testing Input (non-VR)")]
    public KeyCode testHoldKey = KeyCode.Mouse0; // assign different keys for left/right guns

    private void Update()
    {
        if (GameManager.Instance.IsGameOver)
        {
            canShoot = false;
            return;
        }

        // fallback test input
        if (Input.GetKey(testHoldKey)) StartFire();
        else if (Input.GetKeyUp(testHoldKey)) StopFire();

        if(canShoot)
        {
            shootCooldown -= Time.deltaTime;
            if (shootCooldown <= 0f)
            {
                Shoot();
                shootCooldown = 1f / firingRate; // Reset cooldown based on firing rate
            }
        }
    }

    public void StartFire()
    {
        if (canShoot) return;
        canShoot = true;
        shootCooldown = 0f; // allow immediate shot
    }


    public void StopFire()
    {
        canShoot = false;
    }

    private void Shoot()
    {
        // Play muzzle flash
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        Ray ray = new Ray(firePoint.position, firePoint.forward);

        // Draw the ray in the Scene view for debugging (red line for 1 second)
        Debug.DrawRay(ray.origin, ray.direction * maxRange, Color.red, 0.1f);

        RaycastHit hit;
        // Raycast to detect hit
        if (Physics.Raycast(ray, out hit, maxRange, enemyLayer))
        {
            // Check if the hit object has an Enemy component
            Enemy enemy = hit.collider.GetComponentInParent<Enemy>(); // Enemy Script is on parent object hence use GetComponentInParent
            if (enemy != null)
            {
                enemy.TakeDamage(damagePerShot, hit.point, hit.normal);
                // TODO : Add hit effects or sounds here
            }
        }
    }


}
