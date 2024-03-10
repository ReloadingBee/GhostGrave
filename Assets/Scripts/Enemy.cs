using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Player target;

    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (!target) target = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        if (!target.isAlive)
        {
            agent.speed = 0f;
            return;
        }
        agent.destination = target.gameObject.transform.position;
    }
}
