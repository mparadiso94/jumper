using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    public Canvas GameOverUI;
    public Canvas MainMenuUI;
    public Canvas GameUI;
    public Text scoreText;
    public Text powerUpTimeLeftText;
    public Transform powerUpTimeLeftTransform;
    public Transform cam;
    public Transform player;

    public double GameOverLength;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        scoreText.text = GameManager.score.ToString();

        switch (GameManager.gameState)
        {
            case GameManager.GameState.Menu:
                GameOverUI.enabled = false;
                MainMenuUI.enabled = true;
                GameUI.enabled = false;
                cam.position = new Vector3(0, 0, -15);
                break;
            case GameManager.GameState.Playing:
                GameOverUI.enabled = false;
                MainMenuUI.enabled = false;
                GameUI.enabled = true;
                cam.position = new Vector3(0, 0, -15);
                break;
            case GameManager.GameState.GameOver:
                GameOverUI.enabled = true;
                MainMenuUI.enabled = false;
                GameUI.enabled = true;
                break;
        }

        if (Player.hasPowerUp)
        {
            powerUpTimeLeftText.enabled = true;
            Vector3 pos = Player.playerPosition;
            powerUpTimeLeftText.GetComponent<Transform>().position= new Vector3(pos.x * (float)(756/6.89) + 975, pos.y * (float)(465/4.35) + 700, pos.z);
            switch (Player.powerUpType)
            {
                case PowerUp.Type.starPower:
                    powerUpTimeLeftText.text = PowerUp.starPowerTimer.ToString("0.0");
                    break;
                case PowerUp.Type.gun:
                    powerUpTimeLeftText.text = PowerUp.bulletsLeft.ToString();
                    break;
            }
        }
        else
        {
            powerUpTimeLeftText.enabled = false;
        }
    }
}
