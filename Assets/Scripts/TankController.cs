using UnityEngine;

public class TankController : MonoBehaviour
{
    public Alignment alignment;
    public float speed;
    public GameObject bulletPrefab;
    public Vector3 bulletSpawnOffset;

    private Rigidbody rb;
    private Quaternion rotation;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetMovementDirection(Vector3 direction)
    {
        rb.velocity = direction * speed;

        if (direction.magnitude > Mathf.Epsilon)
            rotation = Quaternion.LookRotation(direction, Vector3.up);
    }

    public void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.rotation = rotation;
        bullet.transform.position = transform.position + rotation * bulletSpawnOffset;
        bullet.GetComponent<ContactDamage>().alignment = alignment;
        bullet.GetComponent<Bullet>().SetSource(gameObject);
    }

    public void Kill()
    {
        Debug.Log("Me ded :(");
    }
}
