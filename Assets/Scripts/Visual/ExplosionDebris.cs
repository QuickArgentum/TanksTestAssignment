using System.Collections;
using UnityEngine;

public class ExplosionDebris : MonoBehaviour
{
    public float lifetime;
    public float hitPitchVariance;

    private Rigidbody rb;
    private MeshRenderer meshRenderer;
    private AudioSource audioSource;
    private float volume;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        audioSource = GetComponent<AudioSource>();
        volume = audioSource.volume;
    }

    private void Start()
    {
        gameObject.name = "Debris";
        StartCoroutine(QueueDestruction());
    }

    private void OnCollisionEnter(Collision collision)
    {
        audioSource.pitch = Random.Range(1.0f - hitPitchVariance, 1.0f + hitPitchVariance);
        audioSource.volume = volume * collision.impulse.magnitude;
        audioSource.Play();
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