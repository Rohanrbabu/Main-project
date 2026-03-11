using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [SerializeField] int pointsPerSecond = 10;

    int score;
    float scoreBuffer;
    bool isRunning = true;

    public int Score => score;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Update()
    {
        if (!isRunning)
        {
            return;
        }

        scoreBuffer += pointsPerSecond * Time.deltaTime;

        if (scoreBuffer >= 1f)
        {
            int wholePoints = Mathf.FloorToInt(scoreBuffer);
            score += wholePoints;
            scoreBuffer -= wholePoints;
        }
    }

    public void AddPoints(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        score += amount;
    }

    public void StopScoring()
    {
        isRunning = false;
    }
}
