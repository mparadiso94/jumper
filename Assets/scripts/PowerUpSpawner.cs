using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PowerUpSpawner : MonoBehaviour {
    public Transform powerUpTransform;
    public float spawnLength;

    public static Transform powerUp;
    public static Type type;

    private float spawnTimer;
    private System.Random rand;
    private Transform powerUpAnimation;

    public enum Type
    {
        starPower,
        blank,
        gun,
        none
    }
    // Use this for initialization
    void Start () {
        spawnTimer = 0;
        rand = new System.Random();
    }

    // Update is called once per frame
    void Update () {
        if (GameManager.gameState != GameManager.GameState.Playing || Player.hasPowerUp)
            return;

        spawnTimer += Time.deltaTime;
        if (powerUp == null)
        {
            if (spawnTimer > spawnLength)
            {
                var position = (float)(rand.Next(-900, 900) / 100.0);
                powerUp = Instantiate(powerUpTransform, new Vector3(position, (float)-4.25, -10), Quaternion.Euler(0, 0, 45));
                spawnTimer = 0;
                var PowerUpType = rand.Next(1);
                switch (PowerUpType)
                {
                    case 0:
                        type = Type.starPower;
                        powerUp.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
                        break;
                    case 1:
                        type = Type.blank;
                        powerUp.gameObject.GetComponent<Renderer>().material.color = Color.red;
                        break;

                }
            }
        }
        else
        {
            var playerDifference = Math.Abs(Player.playerPosition.x - powerUp.position.x);
            if (playerDifference < 1.15)
            {
                Player.RecievePowerUp(type);
                Reset();
            }
        }
    }

    private void Reset()
    {
        Destroy(powerUp.gameObject);
        powerUp = null;
    }
}
