using System.Collections;
using UnityEditor;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] GameObject obstacleprefab;
    [SerializeField] float st = 1f;
    void Start()
    {
        StartCoroutine(Spawnobstacleroutine());
    }
    IEnumerator Spawnobstacleroutine()
    {
         while(true)
        {
            yield return new WaitForSeconds(st);
            Instantiate(obstacleprefab, transform.position, Random.rotation);
            
        }
    }

}
