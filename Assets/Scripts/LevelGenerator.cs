using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator Instance { get; private set; }

    [SerializeField] GameObject chunkPrefab;
    [SerializeField] int startingchunksamount = 12;
    [SerializeField] Transform chunkParent;

    [SerializeField] float chunklength = 10f;

    [SerializeField] float movespeed = 8f;
    [SerializeField] float maxSpeed = 30f;
    [SerializeField] float minSpeed = 6f;
    [SerializeField] float speedIncreasePerSecond = 2f;
    [SerializeField] float collisionSpeedPenalty = 4f;
    [SerializeField] float collisionCooldown = 0.2f;
    float nextCollisionAllowedTime;

    List<GameObject> chunks = new List<GameObject>();

    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Spawnstartingchunks();

    }
    void Update()
    {
        IncreaseSpeed();
        movechunks();   
    }
    void Spawnstartingchunks()
    {
        for (int i = 0; i < startingchunksamount; i++)
        {
            Spawnchunk();
        }
    }

    private void Spawnchunk()
    {
        float spawnpositionz = Calculatespawnpos();
        Vector3 chunkspawnposition = new Vector3(transform.position.x, transform.position.y, spawnpositionz);
        GameObject newchunk = Instantiate(chunkPrefab, chunkspawnposition, Quaternion.identity, chunkParent);
        chunks.Add(newchunk);
    }

    float Calculatespawnpos()
    {
        float spawnpositionz;
        if (chunks.Count == 0)
        {
            spawnpositionz = transform.position.z;
        }
        else
        {
            spawnpositionz = chunks[chunks.Count - 1].transform.position.z + chunklength;
        }

        return spawnpositionz;
    }
    void movechunks()
    {
        for (int i = 0;  i < chunks.Count; i++)
        {
            GameObject chunk = chunks[i];
            chunk.transform.Translate(-transform.forward * (movespeed * Time.deltaTime)); 
            if(chunk.transform.position.z<=Camera.main.transform.position.z-chunklength)
            {
                chunks.Remove(chunk);
                Destroy(chunk);
                Spawnchunk();
            }  
        }
    }

    void IncreaseSpeed()
    {
        if (speedIncreasePerSecond <= 0f) return;
        movespeed = Mathf.Min(maxSpeed, movespeed + speedIncreasePerSecond * Time.deltaTime);
        if (movespeed < minSpeed) movespeed = minSpeed;
    }

    public void ApplyCollisionPenalty()
    {
        if (Time.time < nextCollisionAllowedTime) return;
        movespeed = Mathf.Max(minSpeed, movespeed - collisionSpeedPenalty);
        nextCollisionAllowedTime = Time.time + collisionCooldown;
    }
}
