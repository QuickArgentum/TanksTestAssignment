using UnityEngine;

public class ExplosionDebris : MonoBehaviour
{
    [Header("Explosion tank piece affected by physics")]
    [Tooltip("How long will the piece persist before getting cleaned up")]
    public float lifetime;
    [Tooltip("Variance of the collision impact sound")]
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
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        audioSource.pitch = Random.Range(1.0f - hitPitchVariance, 1.0f + hitPitchVariance);
        audioSource.volume = volume * collision.impulse.magnitude;
        audioSource.Play();
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