using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] GameObject chunkPrefab;
    [SerializeField] int startingchunksamount = 12;
    [SerializeField] Transform chunkParent;

    [SerializeField] float chunklength = 10f;

    [SerializeField] float movespeed = 8f;

    List<GameObject> chunks = new List<GameObject>();

    void Start()
    {
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
}
