using System.Collections;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] obstacleprefabs;
    [SerializeField] float st = 1f;
    [SerializeField] Transform obstacleparent;
    [SerializeField] float spawnwidth=4f;
    [SerializeField] Transform player;
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
            Quaternion spawnRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            GameObject obstacleInstance = Instantiate(obstacleprefab, spawnposition, spawnRotation,obstacleparent);
            ObstacleScore obstacleScore = obstacleInstance.GetComponent<ObstacleScore>();
            if (obstacleScore == null)
            {
                obstacleScore = obstacleInstance.AddComponent<ObstacleScore>();
            }
            if (player != null)
            {
                obstacleScore.SetPlayer(player);
            }
            
        }
    }

}
