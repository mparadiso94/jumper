﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    // Player objects
    public Transform player;
    public static Vector3 playerPosition;
    public static Vector3 StartPosition;
    private PlayerState playerState;
    private enum PlayerState
    {
        Resting,
        Jumping,
        Falling
    }

    // Jump vars
    private double JumpLength = 0.25;
    private double JumpTimer;
    private double JumpSpeed = 12;

    // Crouch vars
    private Vector3 crouchScale = new Vector3((float)1.45, (float)0.8,1);
    private Vector3 standingScale = new Vector3((float)1.189451, (float)1.227341, 1);

    // Speed
    private double HorizontalSpeed = 5;

    // Power Up vars
    private static double timer = 0;
    public static double starPowerLength = 8;
    private static bool starPowerBool = false;
    public static bool hasPowerUp = false;
    public static PowerUpSpawner.Type powerUpType = PowerUpSpawner.Type.none;
    // Use this for initialization
    void Start () {
        playerState = PlayerState.Resting;
        StartPosition = player.position;
    }
	
	// Update is called once per frame
	void Update () {
        if (GameManager.gameState != GameManager.GameState.Playing)
            return;

        playerPosition = player.position;

        KeyboardControls();

        if (hasPowerUp)
        {
            PowerUpControls();
        }
    }

    public static void RecievePowerUp(PowerUpSpawner.Type type)
    {
        hasPowerUp = true;
        powerUpType = type;
    }

    private void PowerUpControls()
    {
        switch (powerUpType)
        {
            case PowerUpSpawner.Type.starPower:
                if (starPowerLength < 0)
                {
                    hasPowerUp = false;
                    starPowerLength = 10;
                    player.gameObject.GetComponent<Renderer>().material.color = Color.white;
                    break;
                }
                starPowerLength -= Time.deltaTime;


                // player changing colors controls
                timer += Time.deltaTime;
                if (timer > 0.5)
                {
                    player.gameObject.GetComponent<Renderer>().material.color = starPowerBool ? Color.white : Color.red;
                    starPowerBool = !starPowerBool;
                    timer = 0;
                }
                break;
            case PowerUpSpawner.Type.blank:
                break;
        }
    }

    private void KeyboardControls()
    {
        if (JumpPressed())
        {
            playerState = PlayerState.Jumping;
        }

        if (LeftPressed() || RightPressed())
        {
            MoveHorizontally(LeftPressed(), RightPressed());
        }

        if (CrouchPressed())
        {
            player.localScale = crouchScale;
            player.position = new Vector3(player.position.x, StartPosition.y - (float)0.5, StartPosition.z);
        }

        if (CrouchUnpressed())
        {
            player.localScale = standingScale;
            player.position = new Vector3(player.position.x, StartPosition.y, StartPosition.z); ;
        }

        switch (playerState)
        {
            case PlayerState.Resting:
                return;
            case PlayerState.Jumping:
                MoveVertically(true);
                return;
            case PlayerState.Falling:
                MoveVertically(false);
                return;
        }

    }

    private void MoveHorizontally(bool left, bool right)
    {
        if ((left && right) || (!left && !right))
            return;
        float offSet = (float)(Time.deltaTime * HorizontalSpeed);
        if (left)
            player.position = new Vector3(player.position.x - offSet, player.position.y, player.position.z);

        if (right)
            player.position = new Vector3(player.position.x + offSet, player.position.y, player.position.z);

        return;
    }

    private void MoveVertically(bool vertical)
    {        
        player.position = new Vector3(player.position.x, player.position.y + ( (vertical ? 1 : -1) * (float)JumpSpeed * Time.deltaTime), player.position.z);

        JumpTimer += Time.deltaTime;
        if (JumpTimer > JumpLength)
        {
            playerState = PlayerState.Falling;
        }
        if (JumpTimer > JumpLength * 2)
        {
            player.position = new Vector3(player.position.x, StartPosition.y, StartPosition.z);
            playerState = PlayerState.Resting;
            JumpTimer = 0;
        }
        return;
    }

    private bool JumpPressed() {
        return Input.GetKeyDown("w") || Input.GetKeyDown("up");
    }

    private bool LeftPressed()
    {
        return Input.GetKey("a") || Input.GetKey("left");
    }

    private bool RightPressed()
    {
        return Input.GetKey("d") || Input.GetKey("right");
    }

    private bool CrouchPressed()
    {
        return Input.GetKeyDown("s") || Input.GetKeyDown("down");
    }

    private bool CrouchUnpressed()
    {
        return Input.GetKeyUp("s") || Input.GetKeyUp("down");
    }
}
