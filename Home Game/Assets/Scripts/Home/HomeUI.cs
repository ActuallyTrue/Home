using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI : MonoBehaviour {
    public HomeScript home;
    public Text healthText;
    public float health;


    // Update is called once per frame
    void Update()
    {

        healthText.text = "Home Health: " + home.HomeHealth;
    }
}
