using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float movespeed = 5f;
    [SerializeField] float xclamp = 3f;
     [SerializeField] float zclamp = 2f;
    [SerializeField] string obstacleTag = "Obstacle";

    Vector2 movement;
    Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Handlemovement();
    }
    public void Move(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    void Handlemovement()
    {
        Vector3 currentposition = rigidbody.position;
        Vector3 movedirection = new Vector3(movement.x, 0f, movement.y);
        Vector3 newposition = currentposition + movedirection * (movespeed * Time.fixedDeltaTime);
        newposition.x = Mathf.Clamp(newposition.x, -xclamp, xclamp);
        newposition.z = Mathf.Clamp(newposition.z, -zclamp, zclamp);
        rigidbody.MovePosition(newposition);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!string.IsNullOrEmpty(obstacleTag) && collision.collider.CompareTag(obstacleTag))
        {
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.StopScoring();
            }

            enabled = false;
        }
    }
}
