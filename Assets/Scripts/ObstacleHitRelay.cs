using UnityEngine;

public class ObstacleHitRelay : MonoBehaviour
{
    ObstacleScore owner;

    public void SetOwner(ObstacleScore obstacleScore)
    {
        owner = obstacleScore;
    }

    void OnCollisionEnter(Collision collision)
    {
        owner?.RelayHit(collision.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        owner?.RelayHit(other.gameObject);
    }
}
