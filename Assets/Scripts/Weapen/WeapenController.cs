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
        var cam = Camera.main;
        Vector3 dir = (cam != null) ? cam.transform.forward : firePoint.forward;
        var rot = Quaternion.LookRotation(dir);

        var go = Instantiate(bulletPrefab, firePoint.position, rot);
        Debug.Log($"Spawned bullet at {firePoint.position} dir {dir}");
    }
}
