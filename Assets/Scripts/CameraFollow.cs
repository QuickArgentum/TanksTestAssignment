using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Smoothly follows set target")]
    public Transform target;
    [Range(0, 1)]
    public float smoothTime;

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, smoothTime);
    }
}
