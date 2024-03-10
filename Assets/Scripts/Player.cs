using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] TMP_Text healthText;
    [SerializeField] TMP_Text ammoText;
    [SerializeField] GameObject pickUpText;
    [SerializeField] GameObject winScreen;
    [SerializeField] GameObject loseScreen;
    [SerializeField] AudioClip gameOverSound;

    [Header("Components")]
    public Health health;
    public float trueHealth;
    public Weapon weapon;
    public LayerMask weaponLayer;
    public Transform hand;

    Camera cam;

    public AudioClip gunEquipSound;
    public AudioClip gunDropSound;
    public AudioClip gunReloadSound;
    public AudioClip gunshotSound;

    public Spawner spawner;

    public bool isAlive = true;

    void Start()
    {
        loseScreen.SetActive(false);
        winScreen.SetActive(false);
        cam = Camera.main;

        trueHealth = health.health;

        health.onDamage.AddListener(UpdateUI);
        health.onDie.AddListener(Respawn);
        UpdateUI();
        
        spawner.onWavesCleared.AddListener(AllWavesCleared);
    }

    void Update()
    {
        if (!isAlive) return;
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
        weapon.onShoot.AddListener(ShootSound);
        weapon.onReload.AddListener(GunReload);
        UpdateUI();

        AudioSystem.Play(gunEquipSound);
    }

    public void Drop()
    {
        if (!weapon)
        {
            return;
        }

        weapon.onShoot.RemoveListener(UpdateUI);
        weapon.onShoot.RemoveListener(ShootSound);
        weapon.onReload.RemoveListener(GunReload);

        weapon.GetComponent<Rigidbody>().isKinematic = false;
        weapon.transform.parent = null;
        weapon = null;

        UpdateUI();
        AudioSystem.Play(gunDropSound);
    }

    void UpdateUI()
    {
        healthText.text = $"HP: {health.health}";
        ammoText.text = weapon ? $"{weapon.clipAmmo}/{weapon.ammo}" : "";
    }

    async void GunReload()
    {
        AudioSystem.Play(gunReloadSound);
        await new WaitForSeconds(weapon.reloadTime + 0.01f);
        UpdateUI();
    }

    void ShootSound()
    {
        AudioSystem.Play(gunshotSound);
    }

    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            trueHealth -= 10 * Time.deltaTime;
            health.health = Mathf.CeilToInt(trueHealth);
            health.Damage(0);
        }
    }

    void Respawn()
    {
        if (!isAlive) return;
        LOSE();
    }
    
    async void AllWavesCleared()
    {
        spawner.onWavesCleared.RemoveListener(AllWavesCleared);
        await new WaitUntil (() => GameObject.FindGameObjectsWithTag("Enemy").Length == 0);
        WIN();
    }

    public void LOSE()
    {
        isAlive = false;
        healthText.text = "";
        pickUpText.SetActive(false);
        ammoText.text = "";
        loseScreen.SetActive(true);
        AudioSystem.Play(gameOverSound, 10f);
        weapon.gameObject.SetActive(false);
        weapon = null;
        spawner.onWavesCleared.RemoveListener(AllWavesCleared);
        spawner.enemiesPerWave.Clear();
        spawner.enemiesLeft = 0;
        spawner.gameObject.SetActive(false);
        ammoText.gameObject.SetActive(false);
        healthText.gameObject.SetActive(false);
    }

    public void WIN()
    {
        isAlive = false;
        healthText.text = "";
        pickUpText.SetActive(false);
        ammoText.text = "";
        winScreen.SetActive(true);
        AudioSystem.Play(gameOverSound, 10f);
        weapon.gameObject.SetActive(false);
        weapon = null;
        spawner.gameObject.SetActive(false);
        ammoText.gameObject.SetActive(false);
        healthText.gameObject.SetActive(false);
    }
}
