using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 10;
    public GameObject explosionPrefab;
    public GameObject hitPrefab;
    public int bounceCount;

    private void Start()
    {
        Destroy(gameObject, 3f);
    }

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision other)
    {
        var health = other.gameObject.GetComponent<Health>();
        if(health != null)
        {
            health.Damage(damage);
        }

        // Wall hit effect
        if (hitPrefab != null)
        {
            var obj = Instantiate(hitPrefab, transform.position, Quaternion.identity);
            obj.transform.forward = other.contacts[0].normal;
        }

        if (bounceCount > 0)
        {
            transform.forward = other.contacts[0].normal;
            bounceCount--;
        }
        else
        {
            Destroy(gameObject);
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
    }
}
