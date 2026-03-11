using UnityEngine;

public class Chunk : MonoBehaviour
{
[SerializeField] GameObject fenceprefab;
[SerializeField] float[] lanes={-2.5f,0f,2.5f};
void start()
    {
        spawnfence();
    }
void spawnfence()
    {
        int randomlaneindex= Random.Range(0,lanes.Length);
        Vector3 spawnposition=new Vector3(lanes[randomlaneindex],transform.position.y,transform.position.z);
        Instantiate(fenceprefab,spawnposition,Quaternion.identity,this.transform);
    }
}
