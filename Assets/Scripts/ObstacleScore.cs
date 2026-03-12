using UnityEngine;

public class ObstacleScore : MonoBehaviour
{
    [SerializeField, HideInInspector] int pointsForDodge = 0;
    [SerializeField] int penaltyForHit = 100;
    [SerializeField] float dodgeZOffset = 1f;
    [SerializeField] Transform player;
    [Header("Audio Cue")]
    [SerializeField] float cueDistance = 8f;
    [SerializeField] float cueFrequency = 880f;
    [SerializeField] float cueDuration = 0.12f;
    [SerializeField] float cueVolume = 0.6f;

    bool resolved;
    bool cuePlayed;
    static AudioSource sharedCueSource;
    static AudioClip sharedCueClip;

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
        if (resolved) return;
        EnsurePlayer();
        if (player == null) return;

        TryPlayCue();

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

    void TryPlayCue()
    {
        if (cuePlayed) return;
        if (cueDistance <= 0f || cueDuration <= 0f || cueVolume <= 0f) return;

        float zDelta = transform.position.z - player.position.z;
        if (zDelta <= 0f) return;
        if (zDelta > cueDistance) return;

        cuePlayed = true;

        AudioSource source = GetOrCreateSharedCueSource();
        if (source == null) return;

        if (sharedCueClip == null)
        {
            sharedCueClip = CreateToneClip(cueFrequency, cueDuration);
        }

        source.volume = cueVolume;
        source.PlayOneShot(sharedCueClip);
    }

    static AudioSource GetOrCreateSharedCueSource()
    {
        if (sharedCueSource != null) return sharedCueSource;

        Camera cam = Camera.main;
        if (cam != null)
        {
            AudioListener listener = cam.GetComponent<AudioListener>();
            if (listener == null)
            {
                cam.gameObject.AddComponent<AudioListener>();
            }

            sharedCueSource = cam.GetComponent<AudioSource>();
            if (sharedCueSource == null)
            {
                sharedCueSource = cam.gameObject.AddComponent<AudioSource>();
            }
        }
        else
        {
            GameObject audioObj = new GameObject("ObstacleCueAudio");
            audioObj.AddComponent<AudioListener>();
            sharedCueSource = audioObj.AddComponent<AudioSource>();
        }

        if (sharedCueSource != null)
        {
            sharedCueSource.playOnAwake = false;
            sharedCueSource.spatialBlend = 0f;
            sharedCueSource.rolloffMode = AudioRolloffMode.Logarithmic;
            sharedCueSource.bypassEffects = true;
            sharedCueSource.bypassListenerEffects = true;
        }

        return sharedCueSource;
    }

    void EnsurePlayer()
    {
        if (player != null) return;
        PlayerController controller = FindObjectOfType<PlayerController>();
        if (controller != null)
        {
            player = controller.transform;
        }
    }

    static AudioClip CreateToneClip(float frequency, float durationSeconds)
    {
        int sampleRate = 44100;
        int sampleCount = Mathf.Max(1, Mathf.CeilToInt(sampleRate * durationSeconds));
        float[] samples = new float[sampleCount];
        float increment = 2f * Mathf.PI * frequency / sampleRate;

        for (int i = 0; i < sampleCount; i++)
        {
            samples[i] = Mathf.Sin(increment * i) * 0.5f;
        }

        AudioClip clip = AudioClip.Create("ObstacleCue", sampleCount, 1, sampleRate, false);
        clip.SetData(samples, 0);
        return clip;
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
