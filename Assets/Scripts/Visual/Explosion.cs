using System.Collections;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Header("Tank death animation")]
    [Tooltip("List of physics tank pieces")]
    public ExplosionDebris[] debris;
    [Tooltip("Variance of the debris pieces lifetime")]
    public float lifetimeVariance;
    public float forceMultiplier;
    public float forceVariance;
    public float forceDirectionRandomness;
    public float torqueMultiplier;
    public Vector3 forceOffset;

    private Vector3 velocity;
    private Material material;

    private void Start()
    {
        foreach (ExplosionDebris item in debris)
        {
            Vector3 vector = item.transform.position - transform.position;
            item.transform.parent = transform.parent;

            item.lifetime += Random.Range(-lifetimeVariance / 2, lifetimeVariance / 2);
            item.SetMaterial(material);
            item.AddForce
            (
                (vector + Random.insideUnitSphere * Random.Range(0, forceDirectionRandomness)) * (forceMultiplier + Random.Range(-forceVariance / 2, forceVariance / 2)) / Mathf.Pow(vector.magnitude, 2) + velocity + forceOffset
            );
            item.AddTorque(Random.insideUnitSphere * Random.Range(0, torqueMultiplier));
        }

        StartCoroutine(QueueDestruction(GetComponent<Animation>().clip.length));
    }

    public void Init(Vector3 velocity, Material material)
    {
        this.velocity = velocity;
        this.material = material;
    }

    private IEnumerator QueueDestruction(float time)
    {
        yield return new WaitForSeconds(time);

        Destroy(gameObject);
    }
}
