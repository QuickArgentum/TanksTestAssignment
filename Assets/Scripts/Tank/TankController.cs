using System;
using UnityEngine;

public class TankController : MonoBehaviour
{
    public Alignment alignment;
    public float speed;
    public GameObject bulletPrefab;
    public Vector3 bulletSpawnOffset;
    public AnimationCurve accelerationCurve;
    public AnimationCurve brakingCurve;
    public float accelerationTime;
    public float brakingTime;

    public event EventHandler DeadStateChanged;

    private Rigidbody rb;
    private Collider col;
    private TankView view;
    private Vector3 direction;
    private Quaternion rotation;
    private bool isDead = false;
    private float throttle = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        view = transform.Find("View").GetComponent<TankView>();
    }

    private void Update()
    {
        bool accelerating = direction.magnitude > Mathf.Epsilon;

        if (accelerating)
            throttle += 1 / accelerationTime;
        else
            throttle -= 1 / brakingTime;

        throttle = Mathf.Clamp01(throttle);
        float multiplier = (accelerating ? accelerationCurve : brakingCurve).Evaluate(throttle);
        rb.velocity = direction * multiplier * speed;

        view.SetSpeed(rb.velocity.magnitude);
    }

    public void SetMovementDirection(Vector3 direction)
    {
        if (isDead) return;

        this.direction = direction;

        if (direction.magnitude > Mathf.Epsilon)
        {
            view.SetDirection(direction);
            rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }

    public void Fire()
    {
        if (isDead) return;

        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.rotation = rotation;
        bullet.transform.position = transform.position + rotation * bulletSpawnOffset;
        bullet.GetComponent<ContactDamage>().alignment = alignment;
        bullet.GetComponent<Bullet>().SetSource(gameObject);

        view.PlayFireAnimation();
    }

    public void Kill()
    {
        UpdateDeadState(true);
        throttle = 0;
        direction = Vector3.zero;
        RespawnManager.Instance.RequestRespawn(this);
        ScoreManager.Instance.HandleKill(this);
        DecalManager.Instance.CreateDecal(transform.position);
        view.PlayDeathAnimation(rb.velocity);
    }

    public void Respawn(Vector3 position)
    {
        UpdateDeadState(false);
        transform.position = position;
        view.PlaySpawnAnimation();
    }

    public void UpdateDeadState(bool value)
    {
        isDead = value;
        view.gameObject.SetActive(!value);
        col.enabled = !value;

        DeadStateChanged?.Invoke(this, new DeadStateEventArgs { isDead = value });
    }

    public class DeadStateEventArgs : EventArgs
    {
        public bool isDead;
    }
}
