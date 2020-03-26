using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public List<Transform> enemyTypes;
    public List<Transform> spawnPoints;

    public float timeBetweenWaves = 5f;
    private float countDown = 2f;

    private int waveIndex = 1;
    private List<Transform> activeEnemies;
    private List<Vector2Int> availableCells;
    private GetAllTilePositions tilePositionScript;
    private List<Stack<Vector2>> startingPaths;

    private void Awake()
    {
        tilePositionScript = GetComponent<GetAllTilePositions>();
        availableCells = new List<Vector2Int>();
        startingPaths = new List<Stack<Vector2>>();
        StartCoroutine(Setup());
    }

    private void Update() 
    {
        if(availableCells != null && countDown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countDown = timeBetweenWaves;
        }

        countDown -= Time.deltaTime;
    }

    IEnumerator SpawnWave()
    {
        /*waveIndex++;*/
        for(int i = 0; i < waveIndex; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);
        }
    }

    void SpawnEnemy()
    {
        //Transform virus = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        int index = (int)Random.Range(0, spawnPoints.Count - 0.01f);
        var transF = spawnPoints[index];
        Transform virus = Instantiate(enemyTypes[(int)Random.Range(0, enemyTypes.Count - 0.01f)], transF.position, transF.rotation);
        virus.parent = transF;
        var pathInp = new Stack<Vector2>(startingPaths[index]);
        virus.GetComponent<VirusMovement>().SetPath(pathInp);
    }

    // Using a coroutine because the other class may have not been loaded up, i think
    IEnumerator Setup()
    {
        availableCells = tilePositionScript.getAvailableCells();

        while (availableCells == null)
        {
            yield return new WaitForSeconds(0.05f);
            availableCells = tilePositionScript.getAvailableCells();
        }

        foreach (var item in spawnPoints)
        {
            var reverse = new Stack<Vector2>(tilePositionScript.getMoves(availableCells, item.transform.position));

            startingPaths.Add(reverse);
        }
    }
}
