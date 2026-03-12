using UnityEngine;

public class ObstacleScore : MonoBehaviour
{
    [SerializeField, HideInInspector] int pointsForDodge = 0;
    [SerializeField] int penaltyForHit = 100;
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

        AttachRelaysToChildColliders();
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
        if (!IsPlayerObject(other)) return;

        ScoreManager.Instance?.LoseScore(penaltyForHit);
        resolved = true;
    }

    public void RelayHit(GameObject other)
    {
        TryResolveHit(other);
    }

    void AttachRelaysToChildColliders()
    {
        Collider[] colliders = GetComponentsInChildren<Collider>(true);
        for (int i = 0; i < colliders.Length; i++)
        {
            Collider col = colliders[i];
            if (col == null || col.gameObject == gameObject) continue;

            ObstacleHitRelay relay = col.GetComponent<ObstacleHitRelay>();
            if (relay == null)
            {
                relay = col.gameObject.AddComponent<ObstacleHitRelay>();
            }

            relay.SetOwner(this);
        }
    }

    bool IsPlayerObject(GameObject other)
    {
        if (other == null) return false;

        if (other.CompareTag("Player")) return true;

        if (player != null)
        {
            Transform otherTransform = other.transform;
            if (otherTransform == player || otherTransform.IsChildOf(player)) return true;
        }

        return other.GetComponentInParent<PlayerController>() != null;
    }
}
