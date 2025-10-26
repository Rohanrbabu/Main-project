using System.Collections;
using UnityEditor;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] GameObject obstacleprefab;
    [SerializeField] float st = 1f;
    int obstaclespawned = 0;
    void Start()
    {
        StartCoroutine(Spawnobstacleroutine());
    }
    IEnumerator Spawnobstacleroutine()
    {
         while(obstaclespawned<5)
        {
            yield return new WaitForSeconds(st);
            Instantiate(obstacleprefab, transform.position, Quaternion.identity);
            obstaclespawned++;
        }
    }

}
