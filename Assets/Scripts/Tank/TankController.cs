using System;
using UnityEngine;

public class TankController : MonoBehaviour
{
    public Alignment alignment;
    public float speed;
    public GameObject bulletPrefab;
    public Vector3 bulletSpawnOffset;

    public event EventHandler DeadStateChanged;

    private Rigidbody rb;
    private Collider col;
    private TankView view;
    private Quaternion rotation;
    private bool isDead = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        view = transform.Find("View").GetComponent<TankView>();
    }

    public void SetMovementDirection(Vector3 direction)
    {
        if (isDead) return;

        rb.velocity = direction * speed;

        if (direction.magnitude > Mathf.Epsilon)
        {
            rotation = Quaternion.LookRotation(direction, Vector3.up);
            view.SetDirection(direction);
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
    }

    public void Kill()
    {
        UpdateDeadState(true);
        rb.velocity = Vector3.zero;
        RespawnManager.Instance.RequestRespawn(this);
        ScoreManager.Instance.HandleKill(this);
    }

    public void Respawn(Vector3 position)
    {
        UpdateDeadState(false);
        transform.position = position;
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
