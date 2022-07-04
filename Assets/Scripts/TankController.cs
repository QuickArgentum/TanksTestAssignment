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
    private Collider collider;
    private GameObject view;
    private Quaternion rotation;
    private bool isDead = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        view = transform.Find("View").gameObject;
    }

    public void SetMovementDirection(Vector3 direction)
    {
        if (isDead) return;

        rb.velocity = direction * speed;

        if (direction.magnitude > Mathf.Epsilon)
            rotation = Quaternion.LookRotation(direction, Vector3.up);
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
        RespawnManager.Instance.RequestRespawn(this);
    }

    public void Respawn(Vector3 position)
    {
        UpdateDeadState(false);
        transform.position = position;
    }

    public void UpdateDeadState(bool value)
    {
        isDead = value;
        view.SetActive(!value);
        collider.enabled = !value;

        DeadStateChanged?.Invoke(this, new DeadStateEventArgs { isDead = value });
    }

    public class DeadStateEventArgs : EventArgs
    {
        public bool isDead;
    }
}
