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

    public float timeBetweenWaves = 15f;
    public float waveCountdown = 0f;

    private float searchCountdown = 1f;

    public SpawnState state = SpawnState.counting;

    private void Start()
    {
        waveCountdown = timeBetweenWaves;
    }

    private void Update()
    {
        if(state == SpawnState.waiting){

            if(!IsEnemyAlive()){
                // Begin a new round
            }

            else{
                return;
            }

        }

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

    bool IsEnemyAlive()
    {

        searchCountdown -= timeBetweenWaves.deltaTime;

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
        Debug.Log("Spawning Enemy: " + _enemy.name);
    }

}
