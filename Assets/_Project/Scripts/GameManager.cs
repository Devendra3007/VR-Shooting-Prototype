using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState { WaitingToStart, Playing, Paused, GameOver }
    public static GameManager Instance { get; private set; }

    public event Action<GameState> OnGameStateChanged;
    public event Action<Gun.GunType> OnGunChanged;

    public Transform PlayerTransform; // Reference to player Transform

    private GameState gameState = GameState.WaitingToStart;

    private int score = 0;
    private bool gunChangeMenuActive = false;
    private Gun.GunType currentGunType = Gun.GunType.Type1;
    private AudioSource audioSource; // For playing game over sound

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

        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (gameState is GameState.GameOver) return;

        // Press Y to toggle gun change menu
        if (OVRInput.Get(OVRInput.RawButton.Y)) 
        {
            gunChangeMenuActive = !gunChangeMenuActive;
            ShowGunsMenu(gunChangeMenuActive);
        }
    }

    private void ShowGunsMenu(bool visible)
    {
        gameState = gunChangeMenuActive ? GameState.Paused : GameState.Playing;
        UIManager.Instance.ToggleGunChangePanel(gunChangeMenuActive);
        OnGameStateChanged?.Invoke(gameState);
    }

    public void SetCurrentSelectedGun(Gun.GunType gunType)
    {
        currentGunType = gunType;
        OnGunChanged?.Invoke(gunType);

        gameState = GameState.Playing;
        OnGameStateChanged?.Invoke(gameState);
    }

    public void GameOver()
    {
        UIManager.Instance.ShowGameOverPanel(score);
        gameState = GameState.GameOver;
        OnGameStateChanged?.Invoke(gameState);
        audioSource.Play();
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
