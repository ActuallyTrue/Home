using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour {

    public enum SpawnState { spawning, waiting, counting };

    [System.Serializable]
    public class Wave{

        public string name;
        public Transform enemy;
        public int count;
        public float spawnRate;

    }

    public Wave[] waves;
    private int nextWave = 0;

    public float timeBetweenWaves = 5f;
    public float waveCountdown = 0f;

    public SpawnState state = SpawnState.counting;

    private void Start()
    {
        waveCountdown = timeBetweenWaves;
    }

    private void Update()
    {
        if(waveCountdown <= 0){

            if(state !=  SpawnState.spawning)
            {
                //Start spawning wave
                StartCoroutine(SpawnWave(waves[nextWave]));
            }       
        }
        else{
            waveCountdown -= Time.deltaTime;
        }
    }

    IEnumerator SpawnWave (Wave _wave)
    {
        state = SpawnState.spawning;

        //Spawn
        for (int i = 0; i < _wave.count; i++)
        {


        }

        state = SpawnState.waiting;

        yield break;
    }

    void SpawnEnemy(Transform _enemy)
    {
        Debug.Log("Spawning Enemy: " + _enemy.name);
    }

}
