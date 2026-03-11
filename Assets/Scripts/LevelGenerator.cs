using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] GameObject chunkPrefab;
    [SerializeField] int startingchunksamount = 12;
    [SerializeField] Transform chunkParent;

    [SerializeField] Transform cameraTransform;

    [SerializeField] float chunklength = 10f;

    [SerializeField] float movespeed = 8f;

    [SerializeField] int pointsPerChunk = 5;

    List<GameObject> chunks = new List<GameObject>();

    void Start()
    {
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        Spawnstartingchunks();

    }
    void Update()
    {
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
        if (cameraTransform == null)
        {
            return;
        }

        for (int i = chunks.Count - 1; i >= 0; i--)
        {
            GameObject chunk = chunks[i];

            if (chunk == null)
            {
                chunks.RemoveAt(i);
                continue;
            }

            chunk.transform.Translate(-transform.forward * (movespeed * Time.deltaTime));

            if (chunk.transform.position.z <= cameraTransform.position.z - chunklength)
            {
                if (ScoreManager.Instance != null)
                {
                    ScoreManager.Instance.AddPoints(pointsPerChunk);
                }

                Destroy(chunk);
                chunks.RemoveAt(i);
                Spawnchunk();
            }
        }
    }
}
