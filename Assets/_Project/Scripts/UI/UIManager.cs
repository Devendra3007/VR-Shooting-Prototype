using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static UIManager Instance { get; private set; }

    
    [Header("GameOver Panel")]  
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;

    [Header("Gun Change Panel")]
    [SerializeField] private GunMenuUI gunChangePanel;

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

    private void Start()
    {
        restartButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(QuitGame);

        gunChangePanel.Initialize();
    }

    public void ToggleGunChangePanel(bool visible)
    {
        gunChangePanel.Show(visible);
    }

    public void UpdateScore(int score)
    {
        // TODO: Implement score update in UI
    }
    public void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }

    public void ShowGameOverPanel(int score)
    {
        finalScoreText.text = $"Final Score: {score}";
        ShowGameOverPanel();
    }

    public void RestartGame()
    {
        GameManager.Instance.RestartGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
