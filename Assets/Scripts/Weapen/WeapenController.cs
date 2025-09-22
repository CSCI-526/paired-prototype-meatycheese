using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;

    public float fireRate = 0.2f; // 0.2秒一发（更快）
    private float fireCooldown = 0f;

    void Update()
    {
        fireCooldown -= Time.deltaTime;

        if (Input.GetButton("Fire1") && fireCooldown <= 0f)
        {
            Fire();
            fireCooldown = fireRate;
        }
    }

    void Fire()
    {
        Vector3 shootDirection = Camera.main.transform.forward;
        Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(shootDirection));
    }
}
