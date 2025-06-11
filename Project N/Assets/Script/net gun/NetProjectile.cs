using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetProjectile : MonoBehaviour
{
    public float lifeTime = 5f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Prisoner"))
        {
            EnemyController3D enemy = other.GetComponent<EnemyController3D>();
            if (enemy != null)
            {
                enemy.Trap();
                Destroy(gameObject);
            }
        }
    }
}