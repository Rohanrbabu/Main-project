using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(TMP_Text))]
public class ScoreDisplay : MonoBehaviour
{
    TMP_Text scoreText;
    ScoreManager scoreManager;
    [SerializeField] float gameOverDelaySeconds = 60f;
    [SerializeField] string gameOverMessage = "Game Over";
    bool gameOver;
    int finalScore;
    float startTime;
    GameObject gameOverPanel;
    TMP_Text gameOverText;
    GameObject startPanel;
    TMP_Text startText;
    bool gameStarted;
    InputAction startAction;
    InputAction retryAction;

    void Awake()
    {
        scoreText = GetComponent<TMP_Text>();
        startAction = new InputAction("Start", InputActionType.Button, "<Keyboard>/space");
        retryAction = new InputAction("Retry", InputActionType.Button, "<Keyboard>/r");
    }

    void OnEnable()
    {
        TryBind();
        startAction.Enable();
        retryAction.Enable();
    }

    void Start()
    {
        ShowStartScreen();
        UpdateScoreText();
    }

    void OnDisable()
    {
        startAction.Disable();
        retryAction.Disable();
        Unbind();
    }
    
    void Update()
    {
        if (!gameStarted)
        {
            if (startAction.WasPerformedThisFrame())
            {
                HideStartScreen();
            }
            return;
        }

        if (gameOver)
        {
            if (retryAction.WasPerformedThisFrame())
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            return;
        }
        if (scoreManager == null) TryBind();

        if (Time.unscaledTime - startTime >= gameOverDelaySeconds)
        {
            gameOver = true;
            finalScore = scoreManager != null ? scoreManager.Score : 0;
            Unbind();
            ShowGameOverScreen();
            Time.timeScale = 0f;
        }
    }

    void TryBind()
    {
        if (scoreManager != null) return;

        scoreManager = ScoreManager.Instance != null
            ? ScoreManager.Instance
            : FindObjectOfType<ScoreManager>();

        if (scoreManager != null)
        {
            scoreManager.OnScoreChanged += HandleScoreChanged;
        }
    }

    void Unbind()
    {
        if (scoreManager == null) return;
        scoreManager.OnScoreChanged -= HandleScoreChanged;
        scoreManager = null;
    }

    void HandleScoreChanged(int newScore)
    {
        if (gameOver) return;
        UpdateScoreText(newScore);
    }

    void UpdateScoreText()
    {
        if (scoreManager != null)
        {
            UpdateScoreText(scoreManager.Score);
        }
        else
        {
            scoreText.text = "Score: 0";
        }
    }

    void UpdateScoreText(int score)
    {
        scoreText.text = $"Score: {score}";
    }

    void ShowStartScreen()
    {
        gameStarted = false;
        Time.timeScale = 0f;

        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            return;
        }

        startPanel = new GameObject("StartPanel");
        startPanel.transform.SetParent(canvas.transform, false);

        RectTransform panelRect = startPanel.AddComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;

        Image panelImage = startPanel.AddComponent<Image>();
        panelImage.color = new Color(0f, 0f, 0f, 0.75f);

        GameObject textObj = new GameObject("StartText");
        textObj.transform.SetParent(startPanel.transform, false);

        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0.5f, 0.5f);
        textRect.anchorMax = new Vector2(0.5f, 0.5f);
        textRect.anchoredPosition = Vector2.zero;
        textRect.sizeDelta = new Vector2(700f, 400f);

        startText = textObj.AddComponent<TextMeshProUGUI>();
        startText.font = scoreText.font;
        startText.fontSize = scoreText.fontSize + 10f;
        startText.alignment = TextAlignmentOptions.Center;
        startText.color = Color.white;
        startText.text = "Ready?\nPress SPACE to Start";
    }

    void HideStartScreen()
    {
        gameStarted = true;
        Time.timeScale = 1f;
        startTime = Time.unscaledTime;
        if (startPanel != null)
        {
            startPanel.SetActive(false);
        }
    }

    void ShowGameOverScreen()
    {
        if (gameOverPanel == null)
        {
            Canvas canvas = GetComponentInParent<Canvas>();
            if (canvas == null)
            {
                scoreText.text = "Game Over";
                return;
            }

            gameOverPanel = new GameObject("GameOverPanel");
            gameOverPanel.transform.SetParent(canvas.transform, false);

            RectTransform panelRect = gameOverPanel.AddComponent<RectTransform>();
            panelRect.anchorMin = Vector2.zero;
            panelRect.anchorMax = Vector2.one;
            panelRect.offsetMin = Vector2.zero;
            panelRect.offsetMax = Vector2.zero;

            Image panelImage = gameOverPanel.AddComponent<Image>();
            panelImage.color = new Color(0f, 0f, 0f, 0.75f);

            GameObject textObj = new GameObject("GameOverText");
            textObj.transform.SetParent(gameOverPanel.transform, false);

            RectTransform textRect = textObj.AddComponent<RectTransform>();
            textRect.anchorMin = new Vector2(0.5f, 0.5f);
            textRect.anchorMax = new Vector2(0.5f, 0.5f);
            textRect.anchoredPosition = Vector2.zero;
            textRect.sizeDelta = new Vector2(600f, 400f);

            gameOverText = textObj.AddComponent<TextMeshProUGUI>();
            gameOverText.font = scoreText.font;
            gameOverText.fontSize = scoreText.fontSize + 8f;
            gameOverText.alignment = TextAlignmentOptions.Center;
            gameOverText.color = Color.white;
        }

        if (scoreText != null) scoreText.enabled = false;

        string performanceNote = GetPerformanceNote();
        gameOverText.text =
            $"{gameOverMessage}\n" +
            $"Score: {finalScore}\n" +
            $"Time: {Mathf.CeilToInt(gameOverDelaySeconds)}s\n" +
            $"Dodged: {GameStats.Instance.ObstaclesDodged}\n" +
            $"Hit: {GameStats.Instance.ObstaclesHit}\n" +
            $"Spawned: {GameStats.Instance.ObstaclesSpawned}\n" +
            $"Max Focus Streak: {Mathf.CeilToInt(GameStats.Instance.GetMaxStreakAtTime(Time.unscaledTime))}s\n" +
            $"{performanceNote}\n" +
            $"Press R to Retry";
    }

    string GetPerformanceNote()
    {
        int spawned = GameStats.Instance.ObstaclesSpawned;
        int dodged = GameStats.Instance.ObstaclesDodged;
        int hit = GameStats.Instance.ObstaclesHit;

        if (spawned <= 0) return "Performance: Getting started - you can do this.";

        float dodgeRate = spawned > 0 ? (float)dodged / spawned : 0f;

        if (dodgeRate >= 0.8f && hit <= 2) return "Performance: Focus strong - excellent dodging.";
        if (dodgeRate >= 0.6f) return "Performance: Steady focus - nice control.";
        if (dodgeRate >= 0.4f) return "Performance: Warming up - keep the rhythm.";
        return "Performance: Tough run - try shorter bursts.";
    }
}
