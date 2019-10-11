using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    // Player objects
    public Transform player;
    public static Transform playerTransform;
    public static Vector3 playerPosition;
    public static Vector3 StartPosition;
    public static bool isCrouching;
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
    public static bool hasPowerUp = false;
    public static PowerUp.Type powerUpType = PowerUp.Type.none;

    // Use this for initialization
    void Start () {
        playerTransform = player;
        playerState = PlayerState.Resting;
        StartPosition = player.position;
    }
	
	// Update is called once per frame
	void Update () {
        if (GameManager.gameState != GameManager.GameState.Playing)
            return;

        playerPosition = player.position;

        KeyboardControls();
    }

    public static void RecievePowerUp(PowerUp.Type type)
    {
        hasPowerUp = true;
        powerUpType = type;
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

        if (CrouchPressed() && playerState.Equals(PlayerState.Resting))
        {
            player.localScale = crouchScale;
            player.position = new Vector3(player.position.x, StartPosition.y - (float)0.5, StartPosition.z);
            isCrouching = true;
        } else
        {
            isCrouching = false;
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
        return Input.GetKey("s") || Input.GetKey("down");
    }

    private bool CrouchUnpressed()
    {
        return Input.GetKeyUp("s") || Input.GetKeyUp("down");
    }
}
