using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 10;

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
        Destroy(gameObject);
        var health = other.gameObject.GetComponent<Health>();
        if(health != null)
        {
            health.Damage(damage);
        }
    }
}
