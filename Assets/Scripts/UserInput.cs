using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TankController))]
public class UserInput : MonoBehaviour
{
    private TankController controller;
    private List<AxisHandler> axisHandlers = new List<AxisHandler>();
    private bool disabled = false;

    private void Awake()
    {
        controller = GetComponent<TankController>();
        controller.DeadStateChanged += (object s, EventArgs args) =>
        {
            disabled = (args as TankController.DeadStateEventArgs).isDead;
        };

        axisHandlers.Add(HorizontalHandler);
        axisHandlers.Add(VerticalHandler);
    }

    private void Update()
    {
        if (disabled) return;

        Vector3 direction = Vector3.zero;

        foreach (AxisHandler handler in axisHandlers)
        {
            direction = handler();
            if (direction.magnitude > Mathf.Epsilon)
                break;
        }

        controller.SetMovementDirection(direction);

        if (Input.GetButtonDown("Fire"))
            controller.Fire();
    }

    private Vector3 HorizontalHandler()
    {
        return Vector3.right * Input.GetAxis("Horizontal");
    }

    private Vector3 VerticalHandler()
    {
        return Vector3.forward * Input.GetAxis("Vertical");
    }

    delegate Vector3 AxisHandler();
}
