using UnityEngine;

public class TankController : MonoBehaviour
{
    public float speed;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetMovementDirection(Vector3 direction)
    {
        rb.velocity = direction * speed;
    }
}
