using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeScript : MonoBehaviour {

    public PlayerHealth player;
    private bool paused;
    public int HomeHealth = 15000;

    public GameObject CurrentHome;
    public GameObject home2;
    public GameObject home3;

    public GameObject border;

    private void Start()
    {
        CurrentHome = gameObject;

    }

    // Update is called once per frame
    void Update () {
        Debug.Log("HomeHealth: " + HomeHealth);
	}

    public void TakeDamage(int damage)
    {

        HomeHealth -= damage;

        if (HomeHealth < 800 && HomeHealth >= 400)
        {
            CurrentHome.transform.localScale = new Vector2(6.34593f, 6.34593f);
        }
        if (HomeHealth < 400 && HomeHealth > 0)
        {
            CurrentHome.transform.localScale = new Vector2(4.180178f, 4.180178f);
        }
        if(HomeHealth <= 0) {

            Destroy(gameObject);
        }

    }


    private void OnTriggerStay2D(Collider2D collision)
    {

        PlayerHealth player = collision.GetComponent<PlayerHealth>();
        if (player != null && !paused)
        {
            player.Heal();
            StartCoroutine(Delay());
        }

    }

    IEnumerator Delay(){

        paused = true;
        yield return new WaitForSecondsRealtime(1.5f);
        paused = false;
    }
}
