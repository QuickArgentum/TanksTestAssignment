using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;

    private GameObject source;
    private Animation anim;
    private ContactDamage contactDamage;
    private Rigidbody rb;

    private void Awake()
    {
        anim = GetComponent<Animation>();
        contactDamage = GetComponent<ContactDamage>();
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Vector3 velocity = transform.forward * speed;
        if (source)
            velocity += source.GetComponent<Rigidbody>().velocity;
        rb.velocity = velocity;
    }

    public void SetSource(GameObject source)
    {
        this.source = source;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != source)
            Break();
    }

    private void Break()
    {
        contactDamage.enabled = false;
        rb.velocity = Vector3.zero;
        anim.Play();

        StartCoroutine(QueueDestruction(anim.clip.length));
    }

    private IEnumerator QueueDestruction(float time)
    {
        yield return new WaitForSeconds(time);

        Destroy(gameObject);
    }
}
