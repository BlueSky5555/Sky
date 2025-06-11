using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController3D : MonoBehaviour
{
    public GameObject ragdollPrefab;
    public float fleeDistance = 10f;
    public float fleeSpeed = 3.5f;

    private Rigidbody rb;
    private bool isTrapped = false;
    private NavMeshAgent agent;
    private Transform player;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            agent.speed = fleeSpeed;
        }

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (isTrapped || player == null || agent == null) return;

        Vector3 dirToPlayer = transform.position - player.position;
        Vector3 fleeTarget = transform.position + dirToPlayer.normalized * fleeDistance;

        agent.SetDestination(fleeTarget);
    }

    public void Trap()
    {
        if (isTrapped) return;
        isTrapped = true;

        if (ragdollPrefab != null)
        {
            GameObject ragdoll = Instantiate(ragdollPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else
        {
            if (agent != null) agent.enabled = false;
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.isKinematic = true;
            }
            Debug.Log("Prisoner trapped in 3D!");
        }
    }
}
