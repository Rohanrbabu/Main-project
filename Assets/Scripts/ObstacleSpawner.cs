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
            SnapToGround(obstacleInstance, transform.position.y);
            GameStats.Instance?.RegisterSpawn();
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

    void SnapToGround(GameObject obstacle, float groundY)
    {
        if (obstacle == null) return;

        Collider[] colliders = obstacle.GetComponentsInChildren<Collider>();
        if (colliders == null || colliders.Length == 0) return;

        float lowest = float.PositiveInfinity;
        for (int i = 0; i < colliders.Length; i++)
        {
            Collider col = colliders[i];
            if (col == null || !col.enabled) continue;
            if (col.isTrigger) continue;
            if (col.bounds.min.y < lowest) lowest = col.bounds.min.y;
        }

        if (float.IsInfinity(lowest)) return;

        float delta = groundY - lowest;
        if (Mathf.Abs(delta) > 0.0001f)
        {
            obstacle.transform.position += new Vector3(0f, delta, 0f);
        }
    }

}
