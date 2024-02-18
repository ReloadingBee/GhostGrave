using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;

    public int ammo;
    public int maxAmmo = 10;
    public float reloadTime = 3f;
    public bool isReloading;
    public bool isAutoFire;

    public float shootInterval = 0.5f;
    float shootCooldown = 0.5f;

    private void Start()
    {
        if (ammo == 0) ammo = maxAmmo;
    }

    private void Update()
    {
        // Manual fire
        if(!isAutoFire && Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }

        if(isAutoFire && Input.GetKey(KeyCode.Mouse0))
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R) && ammo < maxAmmo)
        {
            Reload();
        }

        shootCooldown -= Time.deltaTime;
    }

    async void Shoot()
    {
        if (isReloading) return;
        if (ammo <= 1)
        {
            Reload();
            return;
        }

        if (shootCooldown > 0) return;

        shootCooldown = shootInterval;
        ammo--;
        Instantiate(bulletPrefab, transform.position, transform.rotation);
    }

    async void Reload()
    {
        if (isReloading) return;

        isReloading = true;
        await new WaitForSeconds(reloadTime);
        ammo = maxAmmo;
        isReloading = false;
    }
}
