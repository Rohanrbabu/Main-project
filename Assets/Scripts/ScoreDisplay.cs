using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class ScoreDisplay : MonoBehaviour
{
    TMP_Text scoreText;
    ScoreManager scoreManager;

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
