using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class RunawayfromPlayer : MonoBehaviour
{
    public Transform player;
    public float fleeDistance = 20f;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 8f;               // ปรับให้วิ่งเร็วขึ้น
        agent.angularSpeed = 720f;      // หันตัวได้เร็วขึ้น
        agent.acceleration = 50f;       // เร่งความเร็วเร็วขึ้น
        agent.baseOffset = 0.1f;        // ป้องกันจมดิน
        InvokeRepeating(nameof(FleeFromPlayer), 0f, 0.25f); // ปรับ rate การคำนวณ
    }

    void FleeFromPlayer()
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