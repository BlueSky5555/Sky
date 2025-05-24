using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetGun : MonoBehaviour
{
    public Camera fpsCam;
    public GameObject netPrefab;
    public Transform firePoint;
    public float netSpeed = 30f;
    public float netLifetime = 3f;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            ShootNet();
        }
    }

    void ShootNet()
    {
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 shootDirection = ray.direction;

        GameObject net = Instantiate(netPrefab, firePoint.position, Quaternion.LookRotation(shootDirection));
        NetProjectile projectile = net.GetComponent<NetProjectile>();
        if (projectile != null)
        {
            projectile.Initialize(shootDirection, netSpeed, netLifetime);
        }
    }
}