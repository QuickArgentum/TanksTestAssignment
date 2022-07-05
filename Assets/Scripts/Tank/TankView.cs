using System.Collections;
using UnityEngine;

public class TankView : MonoBehaviour
{
    public float rotationRadianDelta;
    public GameObject explosionPrefab;
    public Material material;

    private Vector3 direction;

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, direction, rotationRadianDelta, 0.0f));
    }

    public void SetDirection(Vector3 direction)
    {
        this.direction = direction;
    }

    public void PlayDeathAnimation(Vector3 velocity)
    {
        GameObject explosion = Instantiate(explosionPrefab);
        explosion.transform.position = transform.position;
        explosion.transform.rotation = transform.rotation;
        explosion.GetComponent<Explosion>().Init(velocity, material);
    }
}