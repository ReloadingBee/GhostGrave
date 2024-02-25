using UnityEngine;

public class AimTrainer : MonoBehaviour
{
    // I will update this laters

    [SerializeField] Health health;

    [SerializeField] Vector3 minPoint;
    [SerializeField] Vector3 maxPoint;

    private void Start()
    {
        health.onDamage.AddListener(Teleport);
        health.maxHealth = 100000;
    }

    void Teleport()
    {
        health.health = health.maxHealth;

        float randomX = Random.Range(minPoint.x, maxPoint.x);
        float randomY = Random.Range(minPoint.y, maxPoint.y);
        float randomZ = Random.Range(minPoint.z, maxPoint.z);
        var pos = new Vector3(randomX, randomY, randomZ);

        transform.position = pos;
    }
}
