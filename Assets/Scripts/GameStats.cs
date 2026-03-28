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

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
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
    }
}
