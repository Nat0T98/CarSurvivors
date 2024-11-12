using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firing : MonoBehaviour
{
    public string bulletPool = "CarBulletPool";  
    public Transform firePoint;
    public float bulletSpeed = 20f;
    public float fireRate = 0.2f;
    public float bulletSpread = 0.1f;
    public float damage = 10f;

    private float nextFireTime = 0f;

    void Update()
    {
        /*if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }*/
    }

    void Shoot()
    {
        GameObject bullet = ObjectPooler.Instance.SpawnFromPool(bulletPool, firePoint.position, firePoint.rotation);

        Vector3 spread = new Vector3(Random.Range(-bulletSpread, bulletSpread), Random.Range(-bulletSpread, bulletSpread), 0);
        bullet.transform.forward += spread;

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = bullet.transform.forward * bulletSpeed;

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.damage = damage;
    }

    public void TurretFire()
    {
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }
}
