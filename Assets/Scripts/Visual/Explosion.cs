using System.Collections;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public ExplosionDebris[] debris;
    public float lifetimeVariance;
    public float forceMultiplier;
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
            item.AddForce(vector * forceMultiplier / Mathf.Pow(vector.magnitude, 2) + velocity + forceOffset);
        }

        StartCoroutine(QueueDestruction(GetComponent<Animation>().GetClip("Explosion").length));
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
