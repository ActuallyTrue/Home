using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{

    public float speed = 20f;
    public int damage = 20;
    public Rigidbody2D rb2d;
    public float TimeUntilDestroy = 2;

    // Use this for initialization
    void Start()
    {
        rb2d.velocity = transform.up * speed;


    }

    private void Update()
    {
        TimeUntilDestroy -= Time.deltaTime;

        if (TimeUntilDestroy < 0)
        {
            Object.Destroy(gameObject);
            TimeUntilDestroy = 2;
        }
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        PlayerHealth player = hitInfo.GetComponent<PlayerHealth>();
        HomeScript home = hitInfo.GetComponent<HomeScript>();
        if (player != null)
        {

            player.TakeDamage(damage);
            Object.Destroy(gameObject);
        }

        if (home != null)
        {
            home.TakeDamage(damage);
        }
    }

}
