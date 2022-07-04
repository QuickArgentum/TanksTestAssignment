using UnityEngine;

public class TankController : MonoBehaviour
{
    public float speed;

    private Rigidbody rb;
    private Vector3 moveDirection = Vector3.zero;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + moveDirection * speed * Time.fixedDeltaTime);
    }

    public void SetMovementDirection(Vector3 direction)
    {
        moveDirection = direction;
    }
}
