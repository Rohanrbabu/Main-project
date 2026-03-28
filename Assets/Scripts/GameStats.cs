using UnityEngine;

public class GameStats : MonoBehaviour
{
    static GameStats instance;
    public static GameStats Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("GameStats");
                instance = obj.AddComponent<GameStats>();
            }
            return instance;
        }
    }

    public int ObstaclesSpawned { get; private set; }
    public int ObstaclesDodged { get; private set; }
    public int ObstaclesHit { get; private set; }
    public float MaxConcentrationStreak { get; private set; }
    float currentStreakStartTime;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        currentStreakStartTime = Time.unscaledTime;
    }

    public void RegisterSpawn()
    {
        ObstaclesSpawned++;
    }

    public void RegisterDodge()
    {
        ObstaclesDodged++;
    }

    public void RegisterHit()
    {
        ObstaclesHit++;
        UpdateMaxStreak(Time.unscaledTime);
        currentStreakStartTime = Time.unscaledTime;
    }

    public float GetMaxStreakAtTime(float now)
    {
        UpdateMaxStreak(now);
        return MaxConcentrationStreak;
    }

    void UpdateMaxStreak(float now)
    {
        float streak = Mathf.Max(0f, now - currentStreakStartTime);
        if (streak > MaxConcentrationStreak) MaxConcentrationStreak = streak;
    }
}
