using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    public static HashSet<Transform> enemies;
    public Transform enemyTransform;
    public Transform croutchEnemyTransform;

    public bool spawnWaves; 

    public double spawnTimer;
    private System.Random rand;

    // Use this for initialization
    void Start () {
        enemies = new HashSet<Transform>();
        spawnTimer = 0;
        rand = new System.Random();
    }

    // Update is called once per frame
    void Update () {
        if (GameManager.gameState != GameManager.GameState.Playing || !spawnWaves)
            return;        

        if (spawnTimer <= 0)
            SpawnEnemy();

        spawnTimer -= Time.deltaTime;
	}

    private void SpawnEnemy()
    {
        var randomSpawnLength = rand.Next(60,100);
        spawnTimer = (float)randomSpawnLength / 100.0;
        var enemyType = rand.Next(2);
        if (enemyType == 1)
            enemies.Add(Instantiate(enemyTransform, new Vector3(0, 0, 0), Quaternion.identity));
        else
            enemies.Add(Instantiate(croutchEnemyTransform, new Vector3(0, 10, 0), Quaternion.identity));
    }

    public static void ClearOldEnemies()
    {
        foreach  (Transform enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }
        enemies.Clear();
    }

}
