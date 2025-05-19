using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunawayfromPlayer : MonoBehaviour
{
    public Transform player;
    public float fleeDistance = 10f;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        Vector3 dirToPlayer = transform.position - player.position;
        Vector3 fleePos = transform.position + dirToPlayer.normalized * fleeDistance;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(fleePos, out hit, 5f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
}