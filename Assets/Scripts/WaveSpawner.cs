using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public Transform enemyPrefab;

    public Transform spawnPoint;

    public float timeBetweenWaves = 5f;
    private float countDown = 2f;

    private int waveIndex = 0;
    private Stack<Vector2> path;
    private void Update() 
    {
        if(countDown <= 0f)
        {
            path = GetComponent<GetAllTilePositions>().moves;
            StartCoroutine(SpawnWave());
            countDown = timeBetweenWaves;
        }

        countDown -= Time.deltaTime;
    }

    IEnumerator SpawnWave()
    {
        waveIndex++;
        for(int i = 0; i < waveIndex; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);
        }
    }

    void SpawnEnemy()
    {
        Transform virus = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        virus.parent = spawnPoint;
        virus.GetComponent<VirusMovement>().SetPath(new Stack<Vector2>(new Stack<Vector2>(path)));
    }
}
