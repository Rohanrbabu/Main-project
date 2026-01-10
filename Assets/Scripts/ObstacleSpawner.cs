using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] obstacleprefabs;
    [SerializeField] float st = 1f;
    [SerializeField] Transform obstacleparent;
    [SerializeField] float spawnwidth=4f;
    void Start()
    {
        StartCoroutine(Spawnobstacleroutine());
    }
    IEnumerator Spawnobstacleroutine()
    {
         while(true)
        {
            GameObject obstacleprefab=obstacleprefabs[Random.Range(0,obstacleprefabs.Length)];
            Vector3 spawnposition= new Vector3(Random.Range(-spawnwidth,spawnwidth),transform.position.y,transform.position.z);
            yield return new WaitForSeconds(st);
            Instantiate(obstacleprefab, transform.position, Random.rotation,obstacleparent);
            
        }
    }

}
