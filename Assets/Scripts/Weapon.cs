using UnityEngine;
using TMPro;
using System.Collections;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private float bulletSpeed = 250.0f;
    [SerializeField] private float bulletLifetime = 2.0f;
    [SerializeField] private float fireRate = 2.0f;
    [SerializeField] private float weaponSpread = 0.01f;
    [SerializeField] private int ammoPerClip = 10;
    [SerializeField] private float reloadTime = 2.0f;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private GameObject muzzleFlash;
    private Animator animator;

    private int currentAmmo;
    private float nextFireTime;
    private bool isReloading = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        currentAmmo = ammoPerClip;
        UpdateAmmoUI();
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
        muzzleFlash.GetComponent<ParticleSystem>().Play();
        animator.SetTrigger("Fire");
        SoundManager.Instance.gunShot.Play();

        nextFireTime = Time.time + 1 / fireRate;
        currentAmmo--;
        Logger.Instance.LogEvent("Weapon fired. Ammo left: " + currentAmmo, "Weapon");
        UpdateAmmoUI();

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
        animator.SetTrigger("Reload");
        SoundManager.Instance.reload.Play();
        Logger.Instance.LogEvent("Reloading started.", "Weapon");

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = ammoPerClip;
        isReloading = false;
        Logger.Instance.LogEvent("Reload complete. Ammo refilled to " + currentAmmo, "Weapon");
        UpdateAmmoUI();
    }

    private void UpdateAmmoUI()
    {
        ammoText.text = currentAmmo.ToString();
    }
}
