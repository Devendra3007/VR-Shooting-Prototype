using UnityEngine;
using UnityEngine.SceneManagement;

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

    private void Update()
    {
        float moveX = 0f;

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
        PlayerTransform.Translate(Vector3.right * moveX * speed * Time.deltaTime);
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
        SceneManager.LoadScene(0);
    }
}
