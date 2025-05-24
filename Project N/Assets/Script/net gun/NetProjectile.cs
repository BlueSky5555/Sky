using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetProjectile : MonoBehaviour
{
    private Vector3 moveDirection;
    private float speed;
    private float lifetime;
    private float timer;

    public void Initialize(Vector3 direction, float speed, float lifetime)
    {
        this.moveDirection = direction.normalized;
        this.speed = speed;
        this.lifetime = lifetime;
        timer = 0f;
    }

    void Update()
    {
        transform.position += moveDirection * speed * Time.deltaTime;

        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Hit enemy!");

            Animator anim = other.GetComponent<Animator>();
            if (anim != null)
            {
                anim.SetTrigger("Fall");
            }

            Destroy(gameObject); // ทำลาย Net เมื่อโดน
        }
    }
}