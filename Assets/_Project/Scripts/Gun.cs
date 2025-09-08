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
    private float maxRange = 100f;
    private float shootCooldown = 0;

    [Header("Trigger Settings")]
    private OVRInput.Controller controller; // LTouch or RTouch
    private OVRInput.Axis1D triggerButton = OVRInput.Axis1D.PrimaryIndexTrigger;

    public void SetGunHand(OVRInput.Controller controller, OVRInput.Axis1D triggerButton)
    {
        this.controller = controller;
        this.triggerButton = triggerButton;
    }

    private void Update()
    {
        // If the game is over or paused, do not move
        if (GameManager.Instance.CurrentGameState is GameManager.GameState.GameOver or GameManager.GameState.Paused) return;

        // Get trigger value (0 to 1)
        float triggerValue = OVRInput.Get(triggerButton, controller);

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

        Ray ray = new Ray(firePoint.position, firePoint.forward);;
        if (Physics.Raycast(ray, out RaycastHit hit, maxRange, enemyLayer))
        {
            // Check if the hit object has an Enemy component
            Enemy enemy = hit.collider.GetComponentInParent<Enemy>(); // Enemy Script is on parent object hence use GetComponentInParent
            if (enemy != null)
            {
                enemy.TakeDamage(damagePerShot, hit.point, hit.normal);
            }
        }
    }


}
