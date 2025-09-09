using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static UIManager Instance { get; private set; }

    private const string SHOW_INSTRUCTIONS_KEY = "ShowInstructions";

    [Header("GameOver Panel")]  
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;

    [Header("Gun Change Panel")]
    [SerializeField] private GunMenuUI gunChangePanel;

    [Header("instructions Panel")]
    [SerializeField] private GameObject instructionsPanel;
    [SerializeField] private Button playButton;
    [SerializeField] private Toggle showInstructionsToggle;

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
        playButton.onClick.AddListener(PlayGame);
        gunChangePanel.Initialize();

        // Check PlayerPrefs for showing instructions
        if (PlayerPrefs.HasKey(SHOW_INSTRUCTIONS_KEY))
        {
            bool showInstructions = PlayerPrefs.GetInt(SHOW_INSTRUCTIONS_KEY) == 1;
            instructionsPanel.SetActive(showInstructions);

            // If not showing instructions, start the game immediately
            if (!showInstructions)
            {
                Invoke(nameof(DelayedStart), 0.1f); // Slight delay to ensure everything is initialized
            }
        }
        else // First time playing, show instructions
        {
            instructionsPanel.SetActive(true);
        }
    }

    private void DelayedStart()
    {
        GameManager.Instance.StartGame();
    }

    public void PlayGame()
    {
        instructionsPanel.SetActive(false);
        GameManager.Instance.StartGame();
        PlayerPrefs.SetInt(SHOW_INSTRUCTIONS_KEY, showInstructionsToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
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
