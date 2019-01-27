using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    public int health = 100;
    public PlayerController Player;



    private void Start()
    {

    }

    public void TakeDamage(int damage)
    {

        health -= damage;

        if (health <= 0)
        {
            Player.Die();
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
