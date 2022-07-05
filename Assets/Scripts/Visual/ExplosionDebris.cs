using System.Collections;
using UnityEngine;

public class ExplosionDebris : MonoBehaviour
{
    public float lifetime;

    private Rigidbody rb;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        gameObject.name = "Debris";
        StartCoroutine(QueueDestruction());
    }

    private IEnumerator QueueDestruction()
    {
        yield return new WaitForSeconds(lifetime);

        Destroy(gameObject);
    }

    public void AddForce(Vector3 force)
    {
        rb.AddForce(force);
    }

    public void AddTorque(Vector3 torque)
    {
        rb.AddTorque(torque);

    }

    public void SetMaterial(Material material)
    {
        meshRenderer.material = material;
    }
}