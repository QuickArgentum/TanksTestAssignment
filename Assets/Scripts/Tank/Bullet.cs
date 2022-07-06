using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;

    private GameObject source;
    private Animation anim;
    private ContactDamage contactDamage;
    private Rigidbody rb;
    private Transform effect;
    private bool isDying = false;

    private void Awake()
    {
        anim = GetComponent<Animation>();
        contactDamage = GetComponent<ContactDamage>();
        rb = GetComponent<Rigidbody>();
        effect = transform.Find("BreakEffect");
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
        if (!isDying && other.gameObject != source)
            Break();
    }

    private void Break()
    {
        isDying = true;
        Destroy(contactDamage);
        rb.velocity = Vector3.zero;

        effect.localEulerAngles = new Vector3(0, 0, Random.Range(-180, 180));
        anim.Play();

        Destroy(gameObject, anim.clip.length);
    }
}
