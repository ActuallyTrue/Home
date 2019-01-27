using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    public int health = 100;



    public void TakeDamage(int damage)
    {

        health -= damage;

        if (health <= 0)
        {
            KillPlayer();
        }

    }

    public void Heal(){

        health += 5;
    }

    void KillPlayer()
    {

        Destroy(gameObject);

    }
}
