using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class ScoreDisplay : MonoBehaviour
{
    TMP_Text scoreText;
    ScoreManager scoreManager;
    [SerializeField] float gameOverDelaySeconds = 60f;
    [SerializeField] string gameOverMessage = "Game Over";
    bool gameOver;
    int finalScore;

    void Awake()
    {
        scoreText = GetComponent<TMP_Text>();
    }

    void OnEnable()
    {
        TryBind();
    }

    void Start()
    {
        UpdateScoreText();
    }

    void OnDisable()
    {
        Unbind();
    }
    
    void Update()
    {
        if (gameOver) return;
        if (scoreManager == null) TryBind();

        if (Time.timeSinceLevelLoad >= gameOverDelaySeconds)
        {
            gameOver = true;
            finalScore = scoreManager != null ? scoreManager.Score : 0;
            Unbind();
            scoreText.text = $"{gameOverMessage}\nScore: {finalScore}";
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
}
