using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PowerUp : MonoBehaviour {
    public Transform powerUpTransform;
    public float spawnLength;

    public static Transform powerUp;


    public static Type type;

    private float spawnTimer;
    private System.Random rand;

    private static double timer = 0;

    // star power vars
    public double starPowerLength;
    public static double starPowerTimer;
    private static bool starPowerBool = false;

    // gun vars
    public Transform gunTransform;
    public static Transform gun = null;
    private Vector3 gunOffset = new Vector3((float)0.75, 0, -5);
    public static int bulletsLeft = 0;
    public int bullets;

    public enum Type
    {
        starPower,
        gun,
        extraLife,
        none
    }
    // Use this for initialization
    void Start () {
        spawnTimer = 0;
        rand = new System.Random();
        starPowerTimer = starPowerLength;
    }

    // Update is called once per frame
    void Update () {
        if (GameManager.gameState != GameManager.GameState.Playing)
            return;

        // Spawn a new power up
        if (!Player.hasPowerUp)
        {
            if (powerUp == null)
            {
                spawnTimer += Time.deltaTime;
                if (spawnTimer > spawnLength)
                {
                    var position = (float)(rand.Next(-900, 200) / 100.0);
                    powerUp = Instantiate(powerUpTransform, new Vector3(position, (float)-4.25, -10), Quaternion.Euler(0, 0, 45));
                    spawnTimer = 0;
                    var PowerUpType = rand.Next(2);
                    switch (PowerUpType)
                    {
                        case 0:
                            type = Type.starPower;
                            powerUp.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
                            break;
                        case 1:
                            type = Type.gun;
                            powerUp.gameObject.GetComponent<Renderer>().material.color = Color.black;
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

        if (Player.hasPowerUp)
        {
            PowerUpControls();
        }
    }

    private void Reset()
    {
        Destroy(powerUp.gameObject);
        powerUp = null;
    }

    public void PowerUpControls()
    {
        switch (Player.powerUpType)
        {
            case Type.starPower:
                if (starPowerTimer < 0)
                {
                    Player.hasPowerUp = false;
                    starPowerTimer = starPowerLength;
                    Player.playerTransform.gameObject.GetComponent<Renderer>().material.color = Color.white;
                    break;
                }
                starPowerTimer -= Time.deltaTime;


                // player changing colors controls
                timer += Time.deltaTime;
                if (timer > 0.33)
                {
                    Player.playerTransform.gameObject.GetComponent<Renderer>().material.color = starPowerBool ? Color.white : Color.red;
                    starPowerBool = !starPowerBool;
                    timer = 0;
                }
                break;
            case Type.gun:
                if (gun == null)
                {
                    gun = Instantiate(gunTransform, Player.playerPosition + gunOffset, Quaternion.Euler(-180, -90, -180));
                    bulletsLeft = bullets;
                }
                gun.position = Player.playerPosition + gunOffset;
                break;
            case Type.none:
                break;
        }
    }

    public static void NoMoreBullets()
    {
        Destroy(gun.gameObject);
        gun = null;
        Player.hasPowerUp = false;
    }
}
