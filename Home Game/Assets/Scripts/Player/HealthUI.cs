using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour {

    public PlayerHealth player;
    public Text healthText;
    public float health;
    

    // Update is called once per frame
    void Update () {

        healthText.text = "Player Health: " + player.health;
	}
}
