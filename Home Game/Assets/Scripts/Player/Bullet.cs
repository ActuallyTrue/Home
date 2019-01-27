using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed = 20f;
    public int damage = 20;
    public Rigidbody2D rb2d;
    public float TimeUntilDestroy = 2;

	// Use this for initialization
	void Start () {
        rb2d.velocity = transform.up * speed;


	}

    private void Update()
    {
        TimeUntilDestroy -= Time.deltaTime;

        if(TimeUntilDestroy < 0){
            Object.Destroy(gameObject);
            TimeUntilDestroy = 2;
        }
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        EnemyHealth enemy = hitInfo.GetComponent<EnemyHealth>();

        if(enemy != null){

            enemy.TakeDamage(damage);

        }

    }

    private void OnTriggerExit2D(Collider2D hitInfo)
    {
        EnemyHealth enemy = hitInfo.GetComponent<EnemyHealth>();
        if (enemy != null)
        {

            Object.Destroy(gameObject);

        }
    }

}
