using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float movespeed = 5f;
    [SerializeField] float xclamp = 3f;
     [SerializeField] float zclamp = 2f;
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
        TryApplyCollisionPenalty(collision.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        TryApplyCollisionPenalty(other.gameObject);
    }

    void TryApplyCollisionPenalty(GameObject other)
    {
        if (!IsObstacle(other)) return;
        LevelGenerator.Instance?.ApplyCollisionPenalty();
    }

    bool IsObstacle(GameObject other)
    {
        if (other == null) return false;
        return other.GetComponentInParent<ObstacleScore>() != null;
    }
}
