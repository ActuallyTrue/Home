﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemyMovement : MonoBehaviour
{

    private Rigidbody2D rb2d;
    public Transform Player;
    public float minDist;
    public float MaxDist;
    public float moveSpeed;


    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        Player = FindObjectOfType<PlayerController>().transform;

    }

    // Update is called once per frame
    void Update()
    {
        float distanceToTarget = Vector3.Distance(transform.position, Player.position);

        if(distanceToTarget < MaxDist){

            Vector3 targetDir = Player.position - transform.position;
            float zAngle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - 90f;
            Quaternion aimangle = Quaternion.AngleAxis(zAngle, Vector3.forward);
            transform.rotation = Quaternion.Euler(0,0,zAngle);
            transform.Translate(Vector3.up * Time.deltaTime * moveSpeed);
        }


        /*Quaternion rotation = Quaternion.LookRotation(Player.transform.position - transform.position, transform.TransformDirection(Vector3.up));
        transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);

        if (Vector3.Distance(transform.position, Player.position) >= minDist)
        {

            transform.position = Vector2.MoveTowards(transform.position, Player.position, moveSpeed * Time.deltaTime);

        }

        if (Vector3.Distance(transform.position, Player.position) <= MaxDist)
        {

        }*/
    }

    //protected void LateUpdate()
   //{
    //    transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z);
    //}
}
