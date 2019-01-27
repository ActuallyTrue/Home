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

    public Transform[] spawnPoints;

    public float timeBetweenWaves = 15f;
    public float waveCountdown = 0f;

    private float searchCountdown = 1f;

    public SpawnState state = SpawnState.counting;

    public GameObject border;


    private void Start()
    {
        waveCountdown = timeBetweenWaves;
        border.SetActive(false);
    }

    private void Update()
    {
        if(state == SpawnState.waiting){

            if(!IsEnemyAlive()){
                // Begin a new round
                RoundStart();
                return;
            }

            else{
                return;
            }

        }

        if(waveCountdown <= 0){

            if(state !=  SpawnState.spawning)
            {
                //Start spawning wave
                border.SetActive(true);
                StartCoroutine(SpawnWave(waves[nextWave]));
            }       
        }
        else{
            waveCountdown -= Time.deltaTime;
        }
    }

    void RoundStart(){
        Debug.Log("Wave Completed");

        state = SpawnState.counting;
        border.SetActive(false);
        waveCountdown = timeBetweenWaves;

        if(nextWave + 1 > waves.Length - 1){
            nextWave = 0;
            Debug.Log("All Waves Complete!  Looping...");
        }
        else{
            nextWave++;
        }

    }

    bool IsEnemyAlive()
    {

        searchCountdown -= Time.deltaTime;

        if (searchCountdown <= 0) {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
    }
        return true;

    }

    IEnumerator SpawnWave (Wave _wave)
    {
        state = SpawnState.spawning;

        //Spawn
        for (int i = 0; i < _wave.count; i++)
        {
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds(1f / _wave.spawnRate);

        }

        state = SpawnState.waiting;

        yield break;
    }

    void SpawnEnemy(Transform _enemy)
    {
        if(spawnPoints.Length == 0){
            Debug.LogError("No spawn points");
        }
        Debug.Log("Spawning Enemy: " + _enemy.name);
        Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(_enemy, _sp.position, _sp.rotation);

    }

}
