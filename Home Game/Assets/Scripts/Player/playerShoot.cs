using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerShoot : MonoBehaviour {


    public Transform firePoint;
    public GameObject bulletPrefab;
    public float timeBtwShots = .1f;
	
	
	// Update is called once per frame
	void Update () {
        timeBtwShots -= Time.deltaTime;
        if (Input.GetButton("Fire1") && timeBtwShots < 0){ 
            Shoot();
            timeBtwShots = .1f;
        }


	}

    void Shoot(){
        //Shooting logic
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

    }
}
