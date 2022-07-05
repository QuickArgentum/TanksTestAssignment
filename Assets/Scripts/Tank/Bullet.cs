using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;

    private GameObject source;

    void Start()
    {
        Vector3 velocity = transform.forward * speed;
        if (source)
            velocity += source.GetComponent<Rigidbody>().velocity;
        GetComponent<Rigidbody>().velocity = velocity;
    }

    public void SetSource(GameObject source)
    {
        this.source = source;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != source)
            Destroy(gameObject);
    }
}
