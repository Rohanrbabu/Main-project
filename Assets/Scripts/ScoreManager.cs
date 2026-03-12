using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [SerializeField] int startingScore = 0;
    [SerializeField] bool clampToZero = true;
    [SerializeField] float pointsPerSecond = 75f;

    public int Score { get; private set; }

    public event Action<int> OnScoreChanged;

    float scoreRemainder;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Score = startingScore;
        OnScoreChanged?.Invoke(Score);
    }

    void Update()
    {
        if (pointsPerSecond <= 0f) return;

        scoreRemainder += pointsPerSecond * Time.deltaTime;
        if (scoreRemainder < 1f) return;

        int add = Mathf.FloorToInt(scoreRemainder);
        scoreRemainder -= add;
        AddScore(add);
    }

    public void AddScore(int amount)
    {
        if (amount <= 0) return;
        Score += amount;
        OnScoreChanged?.Invoke(Score);
    }

    public void LoseScore(int amount)
    {
        if (amount <= 0) return;
        Score -= amount;
        if (clampToZero && Score < 0) Score = 0;
        OnScoreChanged?.Invoke(Score);
    }
}
