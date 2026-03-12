using UnityEngine;

public class ObstacleScore : MonoBehaviour
{
    [SerializeField, HideInInspector] int pointsForDodge = 0;
    [SerializeField] int penaltyForHit = 1;
    [SerializeField] float dodgeZOffset = 1f;
    [SerializeField] Transform player;

    bool resolved;

    void Awake()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
    }

    public void SetPlayer(Transform playerTransform)
    {
        if (player == null)
        {
            player = playerTransform;
        }
    }

    void Update()
    {
        if (resolved || player == null) return;

        if (transform.position.z < player.position.z - dodgeZOffset)
        {
            resolved = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        TryResolveHit(collision.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        TryResolveHit(other.gameObject);
    }

    void TryResolveHit(GameObject other)
    {
        if (resolved) return;
        if (!other.CompareTag("Player")) return;

        ScoreManager.Instance?.LoseScore(penaltyForHit);
        resolved = true;
    }
}
