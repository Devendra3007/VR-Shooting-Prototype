using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState { WaitingToStart, Playing, Paused, GameOver }
    public static GameManager Instance { get; private set; }

    public Transform PlayerTransform; // Reference to player Transform

    private GameState gameState = GameState.WaitingToStart;

    private int score = 0;
    private bool gunChangeMenuActive = false;

    public GameState CurrentGameState => gameState;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
       /* float moveX = 0f;

        // Check for input (A/D or Left/Right arrow keys)
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            moveX = -1f; // Move left
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            moveX = 1f; // Move right
        }

        // Apply movement (you can adjust speed as needed)
        float speed = 5f;
        PlayerTransform.Translate(Vector3.right * moveX * speed * Time.deltaTime);*/


        if (gameState is GameState.GameOver) return;

        // Press Y to toggle gun change menu
        if (OVRInput.Get(OVRInput.RawButton.Y)) //Input.GetKeyDown(KeyCode.Space)
        {
            gunChangeMenuActive = !gunChangeMenuActive;
            gameState = gunChangeMenuActive ? GameState.Paused : GameState.Playing;
            UIManager.Instance.ToggleGunChangePanel(gunChangeMenuActive);
        }
    }

    public void SetGameState(GameState gameState)
    {
        this.gameState = gameState;
    }

    public void GameOver()
    {
        UIManager.Instance.ShowGameOverPanel(score);
        gameState = GameState.GameOver;
    }

    public void AddScore(int points)
    {
        score += points;
        UIManager.Instance.UpdateScore(score);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
