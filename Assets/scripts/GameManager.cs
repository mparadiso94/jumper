using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour {
    public static int score;

    public static GameState gameState;
    public enum GameState
    {
        Menu,
        Playing,
        GameOver
    }

    // Use this for initialization
    void Start () {
        gameState = GameState.Menu;
        score = 0;
    }

    public void StartGame()
    {
        gameState = GameState.Playing;
    }

    // Update is called once per frame
    void Update () {
        if (gameState == GameState.GameOver)
            return;
        if (Input.GetKeyDown("e"))        
            EndGame(); 
    }

    private void EndGame() {
        gameState = GameState.GameOver;
    }

    public void Retry()
    {
        EnemySpawner.ClearOldEnemies();
        score = 0;
        gameState = GameState.Playing;
    }
}
