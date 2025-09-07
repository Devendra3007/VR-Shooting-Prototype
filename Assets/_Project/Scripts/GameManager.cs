using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Transform PlayerTransform; // Reference to player Transform

    private bool isGameOver = false;
    private int score = 0;

    public bool IsGameOver => isGameOver;

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

    public void GameOver()
    {
        isGameOver = true;
        UIManager.Instance.ShowGameOverPanel(score);
    }

    public void AddScore(int points)
    {
        score += points;
        UIManager.Instance.UpdateScore(score);
    }

    public void RestartGame()
    {
        GameManager.Instance.RestartGame();
    }
}
