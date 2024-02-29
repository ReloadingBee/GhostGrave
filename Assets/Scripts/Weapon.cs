using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;

    public int ammo;
    public int maxAmmo = 10;
    public int clipAmmo;
    public int clipSize;

    public float reloadTime = 3f;
    public bool isReloading;
    public bool isAutoFire;
    public int bulletsPerShot;

    public float shootInterval = 0.5f;
    float shootCooldown = 0.5f;

    public float maxSpreadAngle = 5f;

    public UnityEvent onRightClick;
    public UnityEvent onShoot;
    public UnityEvent onReload;


    private void Start()
    {
        if (clipAmmo == 0) ammo = maxAmmo;
        if (bulletsPerShot == 0) bulletsPerShot = 1;
    }

    private void Update()
    {
        shootCooldown -= Time.deltaTime;
    }

    public void Shoot()
    {
        if (isReloading) return;
        if (shootCooldown > 0) return;

        // Auto Reload
        if (clipAmmo <= 0)
        {
            Reload();
            return;
        }

        clipAmmo--;
        shootCooldown = shootInterval;

        for (int i = 0; i < bulletsPerShot; i++)
        {
            var bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            var offsetX = Random.Range(-maxSpreadAngle, maxSpreadAngle);
            var offsetY = Random.Range(-maxSpreadAngle, maxSpreadAngle);
            bullet.transform.eulerAngles += new Vector3(offsetX, offsetY, 0);
        }

        onShoot.Invoke();
    }

    public async void Reload()
    {
        if (isReloading) return;
        isReloading = true;

        onReload.Invoke();

        await new WaitForSeconds(reloadTime);

        //ammo = maxAmmo;
        var ammoToReload = Mathf.Min(ammo, clipSize);
        ammo -= ammoToReload;
        clipAmmo += ammoToReload;

        isReloading = false;
    }
}
