using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour {


    public WaveSpawner WaveTimer;
    public Text TimerText;


    // Update is called once per frame
    void Update()
    {

        TimerText.text = "Time Until Next Wave: " + (int)WaveTimer.waveCountdown;
    }
}
