using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] TMP_Text healthText;
    [SerializeField] TMP_Text ammoText;
    [SerializeField] GameObject pickUpText;

    [Header("Components")]
    public Health health;
    public Weapon weapon;
    public LayerMask weaponLayer;
    public Transform hand;

    Camera cam;

    private void Start()
    {
        cam = Camera.main;

        health.onDamage.AddListener(UpdateUI);
        health.onDie.AddListener(Respawn);
        UpdateUI();
    }

    void Update()
    {
        var collided = Physics.Raycast(cam.transform.position, cam.transform.forward, out var hit, 2f, weaponLayer);
        pickUpText.SetActive(!weapon && collided);

        if(Input.GetKeyDown(KeyCode.E))
        {
            if(!weapon && collided)
            {
                Grab(hit.collider.gameObject);
            }
            else
            {
                Drop();
            }
        }

        if (weapon == null) return;

        // Manual fire
        if (!weapon.isAutoFire && Input.GetKeyDown(KeyCode.Mouse0))
        {
            weapon.Shoot();
        }

        // Auto fire
        if (weapon.isAutoFire && Input.GetKey(KeyCode.Mouse0))
        {
            weapon.Shoot();
        }

        // Reload
        if (Input.GetKeyDown(KeyCode.R) && weapon.ammo < weapon.maxAmmo)
        {
            weapon.Reload();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            weapon.onRightClick.Invoke();
        }
    }

    public void Grab(GameObject gun)
    {
        if (weapon) return;

        weapon = gun.GetComponent<Weapon>();
        weapon.GetComponent<Rigidbody>().isKinematic = true;
        weapon.transform.position = hand.position;
        weapon.transform.rotation = hand.rotation;
        weapon.transform.parent = hand;

        weapon.onShoot.AddListener(UpdateUI);
        weapon.onReload.AddListener(GunReload);
        UpdateUI();
    }

    public void Drop()
    {
        if (!weapon)
        {
            print("No Weapon to drop!!!");
            return;
        }

        weapon.onShoot.RemoveListener(UpdateUI);
        weapon.onReload.RemoveListener(GunReload);

        weapon.GetComponent<Rigidbody>().isKinematic = false;
        weapon.transform.parent = null;
        weapon = null;

        UpdateUI();
    }

    void UpdateUI()
    {
        healthText.text = $"HP: {health.health}";
        if(weapon) ammoText.text = $"{weapon.clipAmmo}/{weapon.ammo}";
        else
        {
            ammoText.text = "";
        }
    }

    async void GunReload()
    {
        await new WaitForSeconds(weapon.reloadTime + 0.01f);
        UpdateUI();
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            health.Damage(10);
        }
    }

    void Respawn()
    {
        health.health = health.maxHealth;
        transform.position = Vector3.zero;
        UpdateUI();
    }
}
