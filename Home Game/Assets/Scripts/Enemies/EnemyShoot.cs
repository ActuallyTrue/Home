using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour {

    public Transform firePoint;
    public GameObject bulletPrefab;
    private float timer;
    public float timeBtwShots;

    private void Start()
    {
        timer = timeBtwShots;
    }
    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Shoot();
           timer = timeBtwShots;
        }


    }

    void Shoot()
    {
        //Shooting logic
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        

    }
}
