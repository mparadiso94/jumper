using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    public static ArrayList enemies;
    public Transform enemyTransform;
    public Transform croutchEnemyTransform;

    public bool spawnWaves; 

    public double spawnTimer;
    private System.Random rand;

    // Use this for initialization
    void Start () {
        enemies = new ArrayList();
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

    public static void ClearAllEnemies()
    {
        foreach  (Transform enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }
        enemies.Clear();
    }

    public static void ClearEnemy(Transform enemy)
    {
        Destroy(enemy.gameObject);
        enemies.Remove(enemy);

    }

    public static void ShootFirstEnemy(bool shootCrouchingEnemy)
    {

        foreach (Transform enemyPrefab in enemies)
        {
            Enemy e = enemyPrefab.GetComponent<Enemy>();
            if (shootCrouchingEnemy)
            {
                if (e.name.StartsWith("Crouch"))
                {
                    if (e.transform.position.x > Player.playerPosition.x)
                    {
                        bool shouldEndGame = false;
                        e.Despawn(shouldEndGame, 3);
                        break;
                    }
                }
            } else
            {
                if (!e.name.StartsWith("Crouch"))
                {
                    if (e.transform.position.x > Player.playerPosition.x)
                    {
                        bool shouldEndGame = false;
                        e.Despawn(shouldEndGame, 3);
                        break;
                    }
                }
            }
        }
    }
}
