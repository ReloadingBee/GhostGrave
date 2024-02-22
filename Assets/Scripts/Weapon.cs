using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;

    public int ammo;
    public int maxAmmo = 10;
    public float reloadTime = 3f;
    public bool isReloading;
    public bool isAutoFire;
    public int bulletsPerFire;

    public float shootInterval = 0.5f;
    float shootCooldown = 0.5f;

    public float maxSpreadAngle = 5f;


    private void Start()
    {
        if (ammo == 0) ammo = maxAmmo;
        if (bulletsPerFire == 0) bulletsPerFire = 1;
    }

    private void Update()
    {
        // Manual fire
        if(!isAutoFire && Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }

        // Auto fire
        if(isAutoFire && Input.GetKey(KeyCode.Mouse0))
        {
            Shoot();
        }

        // Reload
        if (Input.GetKeyDown(KeyCode.R) && ammo < maxAmmo)
        {
            Reload();
        }

        shootCooldown -= Time.deltaTime;
    }

    async void Shoot()
    {
        if (isReloading) return;
        if (shootCooldown > 0) return;
        shootCooldown = shootInterval;


        for (int i = 0; i < bulletsPerFire; i++)
        {
            // Bullet Spread
            Quaternion spreadRotation = Quaternion.Euler(Random.Range(-maxSpreadAngle, maxSpreadAngle),
                                 Random.Range(-maxSpreadAngle, maxSpreadAngle),
                                 Random.Range(-maxSpreadAngle, maxSpreadAngle));
            Instantiate(bulletPrefab, transform.position, transform.rotation * spreadRotation);

            ammo--;
            // Auto Reload
            if (ammo <= 0)
            {
                Reload();
                return;
            }
        }
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
