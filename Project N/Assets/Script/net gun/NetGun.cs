using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetGun : MonoBehaviour
{
    public GameObject netPrefab;
    public Transform firePoint;
    public float fireForce = 500f;
    public int maxAmmo = 5;
    public float reloadTime = 2f;

    private int currentAmmo;
    private bool isReloading;

    void Start()
    {
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        if (isReloading)
            return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButtonDown("Fire1") && currentAmmo > 0)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject net = Instantiate(netPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = net.GetComponent<Rigidbody>();

        if (rb == null)
        {
            rb = net.AddComponent<Rigidbody>();
        }

        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.AddForce(firePoint.forward * fireForce, ForceMode.Impulse);

        currentAmmo--;
    }

    System.Collections.IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
    }
}

// Net projectile script (3D)
public class Net3D : MonoBehaviour
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
