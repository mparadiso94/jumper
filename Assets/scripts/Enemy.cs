using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour {
    public Transform enemy;
    public Transform explosion;
    private static Vector3 SpawnPosition;
    public double lifeLength;

    private double timer;
    private double speed = 10;
    private double horizontalOffset = 20;
    private double verticalOffset = 2.10;
    private double collisionDistance = 1;
    private bool jumpedOver;
    private Transform _explosion;
    private bool isCrouchedEnemy;

	// Use this for initialization
	void Start () {
        isCrouchedEnemy = enemy.name.StartsWith("Crouch");
        enemy.position = new Vector3(Player.StartPosition.x + (float)horizontalOffset, Player.StartPosition.y + (enemy.name.StartsWith("Crouch") ? (float)verticalOffset : 0), Player.StartPosition.z);
        jumpedOver = false;
    }
	
	// Update is called once per frame
	void Update () {
        MoveAcrossTheScreen();
        timer += Time.deltaTime;

        if (timer > lifeLength)
        {
            EnemySpawner.ClearEnemy(enemy);
        }
	}

    void MoveAcrossTheScreen()
    {
        if (GameManager.gameState == GameManager.GameState.GameOver)
            return;

        enemy.position = new Vector3(enemy.position.x - ((float)speed * Time.deltaTime), enemy.position.y, enemy.position.z);
        if (PlayerCollision())
        {
            bool shouldEndGame = !(Player.hasPowerUp && Player.powerUpType.Equals(PowerUp.Type.starPower));
            Despawn(shouldEndGame , 2);
        }
        if (!jumpedOver && Player.playerPosition.x > enemy.position.x)
        {
            jumpedOver = true;
            GameManager.score++;
        }
    }

    public void Despawn(bool shouldEndGame, int pointsToAdd)
    {
        Destroy(enemy.gameObject);
        _explosion = Instantiate(explosion, enemy.position, Quaternion.identity);
        _explosion.GetComponent<ParticleSystem>().Play();
        Destroy(_explosion.gameObject, 2);
        EnemySpawner.enemies.Remove(this.transform);
        Destroy(this);

        if (shouldEndGame) 
        {
            GameManager.gameState = GameManager.GameState.GameOver;
        }
        else {
            GameManager.score += pointsToAdd;
        }
    }

    bool PlayerCollision()
    {
        var xDifference = Math.Abs(Player.playerPosition.x - enemy.position.x);
        if (isCrouchedEnemy)
        {
            return (xDifference < 0.7 && !Input.GetKey("s"));
        } else
        {
            var yDifference = Math.Abs(Player.playerPosition.y - enemy.position.y);
            return xDifference < collisionDistance && yDifference < collisionDistance;
        }
    }
}
