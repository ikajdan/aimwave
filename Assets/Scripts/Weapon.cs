using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private float bulletSpeed = 100.0f;
    [SerializeField] private float bulletLifetime = 2.0f;
    [SerializeField] private float fireRate = 8.0f;
    [SerializeField] private float weaponSpread = 0.05f;
    [SerializeField] private int ammoPerClip = 30;
    [SerializeField] private float reloadTime = 2.0f;

    private int currentAmmo;
    private float nextFireTime;
    private bool isReloading = false;

    private void Start()
    {
        currentAmmo = ammoPerClip;
    }

    private void Update()
    {
        if (isReloading) return;

        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            if (currentAmmo > 0)
            {
                FireWeapon();
            }
            else
            {
                StartCoroutine(Reload());
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < ammoPerClip)
        {
            StartCoroutine(Reload());
        }
    }

    private void FireWeapon()
    {
        nextFireTime = Time.time + 1 / fireRate;
        currentAmmo--;

        Vector3 spreadDirection = GetSpreadDirection();
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.LookRotation(spreadDirection));

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = spreadDirection * bulletSpeed;

        Destroy(bullet, bulletLifetime);
    }

    private Vector3 GetSpreadDirection()
    {
        float spreadX = Random.Range(-weaponSpread, weaponSpread);
        float spreadY = Random.Range(-weaponSpread, weaponSpread);

        Vector3 direction = playerCamera.transform.forward +
                            playerCamera.transform.right * spreadX +
                            playerCamera.transform.up * spreadY;

        return direction.normalized;
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = ammoPerClip;
        isReloading = false;
    }
}
