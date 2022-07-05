using System.Collections;
using UnityEngine;

public class TankView : MonoBehaviour
{
    public float rotationRadianDelta;

    private Vector3 direction;

    public void SetDirection(Vector3 direction)
    {
        this.direction = direction;
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, direction, rotationRadianDelta, 0.0f));
    }
}